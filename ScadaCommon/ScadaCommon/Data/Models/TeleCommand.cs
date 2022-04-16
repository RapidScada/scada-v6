/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents a telecontrol command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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
            CreationTime = DateTime.MinValue;
            UserID = 0;
            CnlNum = 0;
            ObjNum = 0;
            DeviceNum = 0;
            CmdNum = 0;
            CmdCode = "";
            CmdVal = double.NaN;
            CmdData = null;
            RecursionLevel = 0;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TeleCommand(int cnlNum, double cmdVal, int userID)
            : this()
        {
            CnlNum = cnlNum;
            CmdVal = cmdVal;
            UserID = userID;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TeleCommand(int cnlNum, byte[] cmdData, int userID)
            : this()
        {
            CnlNum = cnlNum;
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
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who is sending the command.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

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


        /// <summary>
        /// Gets a string corresponding to the command data.
        /// </summary>
        public string GetCmdDataString()
        {
            return CmdDataToString(CmdData);
        }
        
        /// <summary>
        /// Retrieves arguments from the command data.
        /// </summary>
        public Dictionary<string, string> GetCmdDataArgs()
        {
            // command exmaple:
            // argument1 = val1
            // argument2 = val2
            Dictionary<string, string> args = new Dictionary<string, string>();

            foreach (string line in CmdDataToString(CmdData).Split('\n'))
            {
                int operIdx = line.IndexOf('=');

                if (operIdx > 0)
                {
                    string name = line.Substring(0, operIdx).Trim();

                    if (name != "")
                        args[name] = line.Substring(operIdx + 1).Trim();
                }
            }

            return args;
        }

        /// <summary>
        /// Loads the command from the specified stream.
        /// </summary>
        public void Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            List<string> lines = stream.ReadAllLines();

            if (lines.Count >= 2 &&
                lines[0] == "[TeleCommand]" &&
                lines[lines.Count - 1] == "End=")
            {
                foreach (string line in lines)
                {
                    int equalIdx = line.IndexOf('=');

                    if (equalIdx > 0)
                    {
                        string name = line.Substring(0, equalIdx).Trim();
                        string value = line.Substring(equalIdx + 1).Trim();

                        switch (name)
                        {
                            case "CommandID":
                                CommandID = long.Parse(value);
                                break;
                            case "CreationTime":
                                CreationTime = DateTime.Parse(value, DateTimeFormatInfo.InvariantInfo);
                                break;
                            case "UserID":
                                UserID = int.Parse(value);
                                break;
                            case "CnlNum":
                                CnlNum = int.Parse(value);
                                break;
                            case "ObjNum":
                                ObjNum = int.Parse(value);
                                break;
                            case "DeviceNum":
                                DeviceNum = int.Parse(value);
                                break;
                            case "CmdNum":
                                CmdNum = int.Parse(value);
                                break;
                            case "CmdCode":
                                CmdCode = value;
                                break;
                            case "CmdVal":
                                CmdVal = double.Parse(value, NumberFormatInfo.InvariantInfo);
                                break;
                            case "CmdData":
                                CmdData = ScadaUtils.HexToBytes(value, false, true);
                                break;
                            case "RecursionLevel":
                                RecursionLevel = int.Parse(value);
                                break;
                        }
                    }
                }
            }
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Неверный формат файла." :
                    "Invalid file format.");
            }
        }

        /// <summary>
        /// Loads the command from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                using (FileStream stream = 
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Load(stream);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при загрузке команды ТУ" :
                    "Error loading telecontrol command");
                return false;
            }
        }

        /// <summary>
        /// Saves the options to the specified writer.
        /// </summary>
        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            StringBuilder sb = new StringBuilder()
                .AppendLine("[TeleCommand]")
                .Append("CommandID=").AppendLine(CommandID.ToString())
                .Append("CreationTime=").AppendLine(CreationTime.ToString(DateTimeFormatInfo.InvariantInfo))
                .Append("UserID=").AppendLine(UserID.ToString())
                .Append("CnlNum=").AppendLine(CnlNum.ToString())
                .Append("ObjNum=").AppendLine(ObjNum.ToString())
                .Append("DeviceNum=").AppendLine(DeviceNum.ToString())
                .Append("CmdNum=").AppendLine(CmdNum.ToString())
                .Append("CmdCode=").AppendLine(CmdCode)
                .Append("CmdVal=").AppendLine(CmdVal.ToString(NumberFormatInfo.InvariantInfo))
                .Append("CmdData=").AppendLine(ScadaUtils.BytesToHex(CmdData))
                .Append("RecursionLevel=").AppendLine(RecursionLevel.ToString())
                .AppendLine("End=");

            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                writer.Write(sb.ToString());
            }
        }

        /// <summary>
        /// Saves the command to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                using (FileStream stream = 
                    new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    Save(stream);
                }

                errMsg = "";
                return false;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при сохранении команды ТУ" :
                    "Error saving telecontrol command");
                return false;
            }
        }


        /// <summary>
        /// Converts the specified command data to a string.
        /// </summary>
        public static string CmdDataToString(byte[] cmdData)
        {
            return cmdData == null ? "" : Encoding.UTF8.GetString(cmdData);
        }

        /// <summary>
        /// Converts the specified string to command data.
        /// </summary>
        public static byte[] StringToCmdData(string s)
        {
            return s == null ? new byte[0] : Encoding.UTF8.GetBytes(s);
        }
    }
}
