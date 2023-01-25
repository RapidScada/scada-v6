// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB;
using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Data.Models;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvSms.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevSmsLogic : DeviceLogic
    {
        /// <summary>
        /// The line ending when communicating with a modem.
        /// </summary>
        private const string ModemNewLine = "\x0D\x0A";
        /// <summary>
        /// The condition to stop reading when OK is received.
        /// </summary>
        private readonly TextStopCondition OkStopCond = new TextStopCondition("OK");
        /// <summary>
        /// The condition to stop reading when OK or ERROR are received.
        /// </summary>
        private readonly TextStopCondition OkErrStopCond = new TextStopCondition("OK", "ERROR");

        private readonly List<Message> messageList; // contains messages received by the device
        private AddressBook addressBook;            // the address book shared for the communication line


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSmsLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;

            messageList = new List<Message>();
            addressBook = null;
        }


        /// <summary>
        /// Parses the specified command to extract phone numbers and message text.
        /// </summary>
        private bool ParseMessageCommand(TeleCommand cmd, out List<string> phoneNumbers, out string messageText)
        {
            const char CmdSep = ';';
            string cmdDataStr = cmd.GetCmdDataString();
            int sepIdx = cmdDataStr.IndexOf(CmdSep);

            if (sepIdx >= 0)
            {
                // get phone numbers
                string recipient = cmdDataStr.Substring(0, sepIdx);
                phoneNumbers = new List<string>();

                if (addressBook == null)
                {
                    // add phone number from command data
                    phoneNumbers.Add(recipient);
                }
                else
                {
                    // search for phone numbers in address book
                    if (addressBook.FindContactGroup(recipient) is ContactGroup contactGroup)
                    {
                        // add phone numbers from contact group
                        foreach (Contact contact in contactGroup.Contacts)
                        {
                            phoneNumbers.AddRange(contact.PhoneNumbers);
                        }
                    }
                    else if (addressBook.FindContact(recipient) is Contact contact)
                    {
                        // add phone number from contact
                        phoneNumbers.AddRange(contact.PhoneNumbers);
                    }
                    else
                    {
                        // add phone number from command data
                        phoneNumbers.Add(recipient);
                    }
                }

                messageText = cmdDataStr.Substring(sepIdx + 1);
                return true; // TODO: validate results
            }
            else
            {
                phoneNumbers = null;
                messageText = "";
                return false;
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            addressBook = AddressBookUtils.GetOrLoad(LineContext.SharedData, Storage, Log);
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            DeviceTags.AddGroup(CnlPrototypeFactory.GetCnlPrototypeGroup().ToTagGroup());
            DeviceTags.FlattenGroups = true;
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public override void InitDeviceData()
        {
            base.InitDeviceData();

            // reset counters
            DeviceData.Set(TagCode.Msg, 0);
            DeviceData.Set(TagCode.AtCmd, 0);
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();


            FinishSession();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            base.SendCommand(cmd);
            LastRequestOK = false;
            Connection.NewLine = ModemNewLine;

            if (cmd.CmdCode == TagCode.Msg || cmd.CmdNum == 1)
            {
                if (ParseMessageCommand(cmd, out List<string> phoneNumbers, out string messageText))
                {
                    foreach (string phoneNumber in phoneNumbers)
                    {

                    }
                }
                else
                {
                    Log.WriteLine(CommPhrases.InvalidCommand);
                }
            }
            else if (cmd.CmdCode == TagCode.AtCmd || cmd.CmdNum == 2)
            {
                // send custom AT command
                Connection.WriteLine(cmd.GetCmdDataString());
                Connection.ReadLines(PollingOptions.Timeout, OkErrStopCond, out _);
                LastRequestOK = true;
                FinishRequest();
            }
            else
            {
                Log.WriteLine(CommPhrases.InvalidCommand);
            }

            FinishCommand();
        }
    }
}
