/*
 * Copyright 2021 Rapid Software LLC
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
 * Modified : 2020
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
        private readonly Queue<TeleCommand> commands; // the command queue
        private bool commandsDisabled; // getting commands is disabled for the client


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClientTag()
        {
            commands = new Queue<TeleCommand>();
            commandsDisabled = false;
        }


        /// <summary>
        /// Removes outdated commands.
        /// </summary>
        private void RemoveOutdatedCommands()
        {
            DateTime utcNow = DateTime.UtcNow;
            while (commands.Count > 0 && utcNow - commands.Peek().CreationTime > ScadaUtils.CommandLifetime)
            {
                commands.Dequeue();
            }
        }

        /// <summary>
        /// Adds the command to the end of the queue.
        /// </summary>
        public void AddCommand(TeleCommand command)
        {
            if (!commandsDisabled)
            {
                lock (commands)
                {
                    RemoveOutdatedCommands();
                    commands.Enqueue(command);
                }
            }
        }

        /// <summary>
        /// Removes the specified command at the beginning of the queue and returns the next command.
        /// </summary>
        public TeleCommand GetCommand(long commandToRemove)
        {
            lock (commands)
            {
                if (commands.Count > 0)
                {
                    // remove the specified command
                    if (commandToRemove > 0 && commands.Count > 0 && commands.Peek().CommandID == commandToRemove)
                        commands.Dequeue();

                    RemoveOutdatedCommands();
                    return commands.Count > 0 ? commands.Peek() : null;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Disables getting commands for the client.
        /// </summary>
        public void DisableGettingCommands()
        {
            lock (commands)
            {
                commandsDisabled = true;
                commands.Clear();
                commands.TrimExcess();
            }
        }
    }
}
