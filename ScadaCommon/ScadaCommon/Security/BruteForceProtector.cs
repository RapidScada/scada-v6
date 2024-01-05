/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Represents a mechanism for protecting against brute force attacks
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Lang;
using System;
using System.Collections.Generic;

namespace Scada.Security
{
    /// <summary>
    /// Represents a mechanism for protecting against brute force attacks.
    /// <para>Представляет собой механизм защиты от брутфорс атак.</para>
    /// </summary>
    public class BruteForceProtector
    {
        /// <summary>
        /// The queue for protection against brute forcing.
        /// </summary>
        protected readonly Queue<DateTime> protectionQueue;
        /// <summary>
        /// The login unblocking time (UTC).
        /// </summary>
        protected DateTime loginUnblockDT;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BruteForceProtector(int maxFailsPerMinute, int blockingDuration)
        {
            protectionQueue = new Queue<DateTime>();
            loginUnblockDT = DateTime.MinValue;

            MaxFailsPerMinute = maxFailsPerMinute;
            BlockingDuration = blockingDuration;
            Blocked = false;
            Owner = null;
        }


        /// <summary>
        /// The maximum number of failed login attempts per minute.
        /// </summary>
        public int MaxFailsPerMinute { get; }

        /// <summary>
        /// Gets the duration of login blocking, min.
        /// </summary>
        public int BlockingDuration { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to restrict user access.
        /// </summary>
        public bool Blocked { get; protected set; }

        /// <summary>
        /// Gets the number of failed login attempts in the last minute.
        /// </summary>
        public int FailCount
        {
            get
            {
                lock (protectionQueue)
                {
                    return protectionQueue.Count;
                }
            }
        }

        /// <summary>
        /// Gets or sets the object that owns the protector.
        /// </summary>
        public object Owner { get; set; }


        /// <summary>
        /// Processes the protection queue, blocks and unblocks user access.
        /// </summary>
        protected void Process()
        {
            // remove outdated login attempts
            int failCount;

            lock (protectionQueue)
            {
                if (protectionQueue.Count > 0)
                {
                    DateTime startDT = DateTime.UtcNow.AddMinutes(-1);

                    while (protectionQueue.Count > 0)
                    {
                        DateTime loginDT = protectionQueue.Peek();

                        if (loginDT < startDT)
                            protectionQueue.Dequeue();
                        else
                            break;
                    }
                }

                failCount = protectionQueue.Count;
            }

            // block or unblock user access
            if (loginUnblockDT > DateTime.MinValue)
            {
                if (failCount <= MaxFailsPerMinute && loginUnblockDT <= DateTime.UtcNow)
                {
                    loginUnblockDT = DateTime.MinValue;
                    Blocked = false;
                    OnBlockedChanged(false, Locale.IsRussian ?
                        "Вход в систему разблокирован" :
                        "Login unblocked");
                }
            }
            else if (failCount > MaxFailsPerMinute)
            {
                loginUnblockDT = DateTime.UtcNow.AddMinutes(BlockingDuration);
                Blocked = true;
                OnBlockedChanged(true, string.Format(Locale.IsRussian ?
                    "Вход в систему заблокирован до {0} в целях безопасности" :
                    "Login blocked until {0} for security reasons",
                    loginUnblockDT.ToLocalTime().ToLocalizedTimeString()));
            }
        }

        /// <summary>
        /// Raises the BlockedChanged event.
        /// </summary>
        protected void OnBlockedChanged(bool blocked, string msg)
        {
            BlockedChanged?.Invoke(this, new BlockedChangedEventArgs
            {
                Timestamp = DateTime.UtcNow,
                Blocked = blocked,
                Message = msg
            });
        }

        /// <summary>
        /// Determines whether to restrict user access.
        /// </summary>
        public bool IsBlocked()
        {
            Process();
            return Blocked;
        }

        /// <summary>
        /// Determines whether to restrict user access and returns a message.
        /// </summary>
        public bool IsBlocked(out string errMsg)
        {
            Process();

            if (Blocked)
            {
                errMsg = Locale.IsRussian ?
                    "Вход в систему заблокирован в целях безопасности" :
                    "Login blocked for security reasons";
                return true;
            }
            else
            {
                errMsg = "";
                return false;
            }
        }

        /// <summary>
        /// Registers a failed login attempt.
        /// </summary>
        public void RegisterFailedLogin(DateTime nowDT)
        {
            lock (protectionQueue)
            {
                protectionQueue.Enqueue(nowDT);
            }

            Process();
        }

        /// <summary>
        /// Registers a failed login attempt.
        /// </summary>
        public void RegisterFailedLogin()
        {
            RegisterFailedLogin(DateTime.UtcNow);
        }


        /// <summary>
        /// Occurs when the Blocked property value has changed.
        /// </summary>
        public event EventHandler<BlockedChangedEventArgs> BlockedChanged;
    }
}
