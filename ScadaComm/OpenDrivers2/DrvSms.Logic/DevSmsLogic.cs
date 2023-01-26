// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB;
using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSms.Logic.Protocol;
using Scada.Comm.Lang;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        private readonly List<Message> messages; // contains messages received by the device
        private AddressBook addressBook;         // the address book shared for the communication line


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSmsLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;

            messages = new List<Message>();
            addressBook = null;
        }


        /// <summary>
        /// Creates events according to the received messages.
        /// </summary>
        private void CreateEvents()
        {
            foreach (Message message in messages)
            {
                DeviceData.EnqueueEvent(new DeviceEvent(DeviceTags[TagCode.Msg])
                {
                    Timestamp = DateTime.UtcNow,
                    CnlVal = 0.0,
                    CnlStat = CnlStatusID.Defined, // has informational severity
                    TextFormat = EventTextFormat.CustomText,
                    Text = message.Phone + "; " + message.Text,
                    Descr = string.Format(Locale.IsRussian ?
                        "Сообщение от {0}" :
                        "Message from {0}", message.Phone)
                });
            }
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

                // validate phone numbers
                bool commandIsValid = true;

                if (phoneNumbers.Count > 0)
                {
                    foreach (string phoneNumber in phoneNumbers)
                    {
                        if (!ValidatePhoneNumber(phoneNumber))
                            commandIsValid = false;
                    }
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "{0}Телефонные номера отсутствуют" :
                        "{0}Phone numbers are missing", CommPhrases.ErrorPrefix);
                }

                // get message text
                messageText = cmdDataStr.Substring(sepIdx + 1);

                if (string.IsNullOrEmpty(messageText))
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "{0}Текст сообщения отсутствует" :
                        "{0}Message text is missing", CommPhrases.ErrorPrefix);
                    commandIsValid = false;
                }

                return commandIsValid;
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Неверный формат команды" :
                    "{0}Invalid command format", CommPhrases.ErrorPrefix);
                phoneNumbers = null;
                messageText = "";
                return false;
            }
        }

        /// <summary>
        /// Validates the specified phone number.
        /// </summary>
        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Телефонный номер пуст" :
                    "{0}Phone number is empty", CommPhrases.ErrorPrefix);
                return false;
            }
            else
            {
                bool formatOK = phoneNumber[0] == '+'
                    ? phoneNumber.Length > 1 && phoneNumber.Substring(1).AsEnumerable().All(c => char.IsDigit(c))
                    : phoneNumber.AsEnumerable().All(c => char.IsDigit(c));

                if (formatOK)
                {
                    return true;
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "{0}Телефонный номер \"{1}\" имеет неверный формат" :
                        "{0}Phone number \"{1}\" is invalid", CommPhrases.ErrorPrefix, phoneNumber);
                    return false;
                }
            }
        }

        /// <summary>
        /// Tries to create a PDU.
        /// </summary>
        private bool TryCreatePdu(string phoneNumber, string messageText, out Pdu pdu)
        {
            try
            {
                pdu = PduConverter.MakePDU(phoneNumber, messageText);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при создании PDU: {0}" :
                    "Error creating PDU", ex.Message);
                pdu = null;
                return false;
            }
        }

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        private bool SendMessage(string phoneNumber, Pdu pdu)
        {
            Log.WriteLine(Locale.IsRussian ?
                "Отправка сообщения на номер {0}" :
                "Send message to {0}", phoneNumber);

            Connection.WriteLine("AT+CMGS=" + pdu.Length);
            Thread.Sleep(100);

            try
            {
                Connection.NewLine = "\x1A";
                Connection.WriteLine(pdu.Data);
            }
            finally
            {
                Connection.NewLine = ModemNewLine;
            }

            Connection.ReadLines(PollingOptions.Timeout, OkStopCond, out bool stopReceived);

            if (stopReceived)
            {
                DeviceData.Add(TagCode.Msg, 1);
                return true;
            }
            else
            {
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
            Connection.NewLine = ModemNewLine;

            // echo off
            if (DeviceStatus != DeviceStatus.Normal)
            {
                LastRequestOK = false;
                int tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    Log.WriteLine(Locale.IsRussian ? 
                        "Отключение эхо" : 
                        "Set echo off");
                    Connection.WriteLine("ATE0");
                    Connection.ReadLines(PollingOptions.Timeout, OkStopCond, out bool stopReceived);
                    LastRequestOK = stopReceived;
                    FinishRequest();
                    tryNum++;
                }
            }

            // drop call
            if (LastRequestOK)
            {
                LastRequestOK = false;
                int tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Сброс вызова" : 
                        "Drop call");
                    Connection.WriteLine("ATH"); // alternatively, AT+CHUP
                    Connection.ReadLines(PollingOptions.Timeout, OkStopCond, out bool stopReceived);
                    LastRequestOK = stopReceived;
                    FinishRequest();
                    tryNum++;
                }
            }

            // read received messages
            if (LastRequestOK)
            {
                LastRequestOK = false;
                int tryNum = 0;
                List<string> response = null;

                while (RequestNeeded(ref tryNum))
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Запрос списка сообщений" : 
                        "Request message list");
                    Connection.WriteLine("AT+CMGL=4");
                    response = Connection.ReadLines(PollingOptions.Timeout, OkStopCond, out bool stopReceived);
                    LastRequestOK = stopReceived;
                    FinishRequest();
                    tryNum++;
                }

                // decode messages
                if (LastRequestOK)
                {
                    messages.Clear();
                    PduConverter.FillMessageList(messages, response, out string logMsg);

                    if (!string.IsNullOrEmpty(logMsg))
                        Log.WriteLine(logMsg);

                    CreateEvents();
                }
            }

            // delete received messages
            if (LastRequestOK)
            {
                bool allMessagesDeleted = true;

                foreach (Message message in messages)
                {
                    LastRequestOK = false;
                    int tryNum = 0;

                    while (RequestNeeded(ref tryNum))
                    {
                        Log.WriteLine(Locale.IsRussian ?
                            "Удаление сообщения {0}" : 
                            "Delete message {0}", message.Index);
                        Connection.WriteLine("AT+CMGD=" + message.Index);
                        Connection.ReadLines(PollingOptions.Timeout, OkStopCond, out bool stopReceived);
                        LastRequestOK = stopReceived;
                        FinishRequest();
                        tryNum++;
                    }

                    if (!LastRequestOK)
                        allMessagesDeleted = false;
                }

                LastRequestOK = allMessagesDeleted;
            }

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
                    bool allMessagesSent = true;

                    foreach (string phoneNumber in phoneNumbers)
                    {
                        if (TryCreatePdu(phoneNumber, messageText, out Pdu pdu))
                        {
                            LastRequestOK = false;
                            int tryNum = 0;

                            while (RequestNeeded(ref tryNum))
                            {
                                LastRequestOK = SendMessage(phoneNumber, pdu);
                                FinishRequest();
                                tryNum++;
                            }

                            if (!LastRequestOK)
                                allMessagesSent = false;
                        }
                        else
                        {
                            allMessagesSent = false;
                        }
                    }

                    LastRequestOK = allMessagesSent;
                }
            }
            else if (cmd.CmdCode == TagCode.AtCmd || cmd.CmdNum == 2)
            {
                // send custom AT command
                Connection.WriteLine(cmd.GetCmdDataString());
                Connection.ReadLines(PollingOptions.Timeout, OkErrStopCond, out _);
                DeviceData.Add(TagCode.AtCmd, 1);
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
