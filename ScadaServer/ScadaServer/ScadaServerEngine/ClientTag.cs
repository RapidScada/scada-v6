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
 * Module   : ScadaServerEngine
 * Summary  : Represents an object associated with a connected client
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents an object associated with a connected client.
    /// <para>Представляет объект, связанный с подключенным клиентом.</para>
    /// </summary>
    internal class ClientTag
    {
        private readonly Queue<TeleCommand> commandQueue;
        private bool commandsEnabled;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClientTag()
        {
            commandQueue = new Queue<TeleCommand>();
            commandsEnabled = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the client can receive commands from the server.
        /// </summary>
        public bool CommandsEnabled
        {
            get
            {
                return commandsEnabled;
            }
            set
            {
                commandsEnabled = value;

                if (!commandsEnabled)
                {
                    lock (commandQueue)
                    {
                        commandQueue.Clear();
                        commandQueue.TrimExcess();
                    }
                }
            }
        }


        /// <summary>
        /// Removes outdated commands.
        /// </summary>
        private void RemoveOutdatedCommands()
        {
            DateTime utcNow = DateTime.UtcNow;

            while (commandQueue.Count > 0 && utcNow - commandQueue.Peek().CreationTime > ScadaUtils.CommandLifetime)
            {
                commandQueue.Dequeue();
            }
        }

        /// <summary>
        /// Adds the command to the end of the queue.
        /// </summary>
        public void AddCommand(TeleCommand command)
        {
            if (commandsEnabled)
            {
                lock (commandQueue)
                {
                    RemoveOutdatedCommands();
                    commandQueue.Enqueue(command);
                }
            }
        }

        /// <summary>
        /// Removes the specified command at the beginning of the queue and returns the next command.
        /// </summary>
        public TeleCommand GetCommand(long commandToRemove)
        {
            lock (commandQueue)
            {
                if (commandQueue.Count > 0)
                {
                    // remove the specified command
                    if (commandToRemove > 0 && commandQueue.Count > 0 && commandQueue.Peek().CommandID == commandToRemove)
                        commandQueue.Dequeue();

                    RemoveOutdatedCommands();
                    return commandQueue.Count > 0 ? commandQueue.Peek() : null;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <remarks>This value is included in the client information.</remarks>
        public override string ToString()
        {
            return "";
        }
    }
}
