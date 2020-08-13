/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Represents a telecontrol command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a telecontrol command.
    /// <para>Представляет команду телеуправления (ТУ).</para>
    /// </summary>
    public class TeleCommand
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TeleCommand()
        {
            CommandID = 0;
            CreationTime = DateTime.UtcNow;
            UserID = 0;
            OutCnlNum = 0;
            CmdTypeID = 0;
            ObjNum = 0;
            DeviceNum = 0;
            CmdNum = 0;
            CmdCode = "";
            CmdVal = 0.0;
            CmdData = null;
            RecursionLevel = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TeleCommand(int outCnlNum, double cmdVal, int userID)
            : this()
        {
            OutCnlNum = outCnlNum;
            CmdVal = cmdVal;
            UserID = userID;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TeleCommand(int outCnlNum, byte[] cmdData, int userID)
            : this()
        {
            OutCnlNum = outCnlNum;
            CmdData = cmdData;
            UserID = userID;
        }


        /// <summary>
        /// Gets or sets the server-assigned command ID.
        /// </summary>
        public long CommandID { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the command (UTC).
        /// </summary>
        public DateTime CreationTime { get; protected set; }

        /// <summary>
        /// Gets or sets the ID of the user who is sending the command.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the output channel number.
        /// </summary>
        public int OutCnlNum { get; set; }

        /// <summary>
        /// Gets or sets the command type ID.
        /// </summary>
        public int CmdTypeID { get; set; }

        /// <summary>
        /// Gets or sets the object number.
        /// </summary>
        public int ObjNum { get; set; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public int DeviceNum { get; set; }

        /// <summary>
        /// Gets or sets the command number.
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Gets or sets the command code.
        /// </summary>
        public string CmdCode { get; set; }

        /// <summary>
        /// Gets or sets the command value.
        /// </summary>
        public double CmdVal { get; set; }

        /// <summary>
        /// Gets or sets the command binary data.
        /// </summary>
        public byte[] CmdData { get; set; }

        /// <summary>
        /// Gets or sets the recursion level if the command is sent programmatically.
        /// </summary>
        public int RecursionLevel { get; set; }
    }
}
