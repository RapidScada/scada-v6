// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvHttpNotif.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Storages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Scada.Comm.Drivers.DrvHttpNotif.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevHttpNotifLogic : DeviceLogic
    {
        /// <summary>
        /// Specifies the predefined parameter names.
        /// </summary>
        private static class ParamName
        {
            public const string Address = "address";
            public const string Phone = "phone";
            public const string Email = "email";
            public const string Text = "text";
        }


        /// <summary>
        /// The parameter separator of the 1st command.
        /// </summary>
        private const char CmdSep = ';';
        /// <summary>
        /// The displayed lenght of a response content.
        /// </summary>
        private const int ResponseDisplayLenght = 100;

        private readonly Stopwatch stopwatch;      // measures the time of operations
        private readonly NotifDeviceConfig config; // the device configuration
        private AddressBook addressBook;           // the address book shared for the communication line
        private ParamString paramUri;              // the parametrized request URI
        private ParamString paramContent;          // the parametrized request content
        private HttpClient httpClient;             // sends HTTP requests
        private bool isReady;                      // indicates that the device is ready to send requests
        private bool loggingFlag;                  // indicates that a ready message should be logged


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevHttpNotifLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            ConnectionRequired = false;

            stopwatch = new Stopwatch();
            config = new NotifDeviceConfig();
            addressBook = null;
            paramUri = null;
            paramContent = null;
            httpClient = null;
            isReady = false;
            loggingFlag = false;
        }


        /// <summary>
        /// Validates the device configuration.
        /// </summary>
        private bool ValidateDeviceConfig(out string errMsg)
        {
            if (string.IsNullOrEmpty(config.Uri))
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка: {0}: URI не может быть пустым." :
                    "Error: {0}: URI must not be empty.", Title);
                return false;
            }
            else
            {
                try
                {
                    Uri uri = new Uri(config.Uri);
                }
                catch
                {
                    errMsg = string.Format(Locale.IsRussian ?
                        "Ошибка: {0}: некорректный URI" :
                        "Error: {0}: invalid URI.", Title);
                    return false;
                }
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Writes the readiness status to the log.
        /// </summary>
        private void WriteReadyStatus()
        {
            if (isReady)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0} ожидает команд..." :
                    "{0} is waiting for commands...", Title);
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}{1} не может отправлять письма" :
                    "{0}{1} unable to send emails", CommPhrases.ErrorPrefix, Title);
            }
        }

        /// <summary>
        /// Gets notification arguments from the command.
        /// </summary>
        private Dictionary<string, string> GetNotifArgs(TeleCommand cmd)
        {
            string cmdDataStr = cmd.GetCmdDataString();
            int sepInd = cmdDataStr.IndexOf(CmdSep);

            if (sepInd >= 0)
            {
                Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { ParamName.Address, cmdDataStr.Substring(0, sepInd) },
                    { ParamName.Text, cmdDataStr.Substring(sepInd + 1) }
                };

                AddContactDetails(args);
                return args;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets request arguments from the command.
        /// </summary>
        private Dictionary<string, string> GetRequestArgs(TeleCommand cmd)
        {
            Dictionary<string, string> args = cmd.GetCmdDataArgs();
            AddContactDetails(args);
            return args;
        }

        /// <summary>
        /// Adds the contact phones and emails.
        /// </summary>
        private void AddContactDetails(Dictionary<string, string> args)
        {
            if (args.TryGetValue(ParamName.Address, out string address) &&
                !(args.ContainsKey(ParamName.Phone) && args.ContainsKey(ParamName.Email)))
            {
                List<string> phoneNumbers = new List<string>();
                List<string> emails = new List<string>();

                if (addressBook == null)
                {
                    // add the known address as phone number and email
                    phoneNumbers.Add(address);
                    emails.Add(address);
                }
                else
                {
                    // search in the address book
                    if (addressBook.FindContactGroup(address) is ContactGroup contactGroup)
                    {
                        // add all contacts from the group
                        foreach (Contact contact in contactGroup.Contacts)
                        {
                            phoneNumbers.AddRange(contact.PhoneNumbers);
                            emails.AddRange(contact.Emails);
                        }
                    }
                    else if (addressBook.FindContact(address) is Contact contact)
                    {
                        // add the contact phone numbers and emails
                        phoneNumbers.AddRange(contact.PhoneNumbers);
                        emails.AddRange(contact.Emails);
                    }
                    else
                    {
                        // add the known address as phone number and email
                        phoneNumbers.Add(address);
                        emails.Add(address);
                    }
                }

                if (!args.ContainsKey(ParamName.Phone))
                    args.Add(ParamName.Phone, string.Join(config.AddrSep, phoneNumbers));

                if (!args.ContainsKey(ParamName.Email))
                    args.Add(ParamName.Email, string.Join(config.AddrSep, emails));
            }
        }

        /// <summary>
        /// Creates a request for sending a notification.
        /// </summary>
        private bool CreateRequest(Dictionary<string, string> args, out HttpRequestMessage request)
        {
            try
            {
                // initialize HTTP client
                if (httpClient == null)
                {
                    httpClient = new HttpClient();

                    foreach (Header header in config.Headers)
                    {
                        if (!string.IsNullOrEmpty(header.Name))
                            httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }
                }

                // create request
                paramUri?.ResetParams(args, EscapingMethod.EncodeUrl);
                paramContent?.ResetParams(args, config.ContentEscaping);

                string uri = paramUri == null ? config.Uri : paramUri.ToString();
                string content = paramContent == null ? config.Content : paramContent.ToString();

                request = new HttpRequestMessage(
                    config.Method == RequestMethod.Post ? HttpMethod.Post : HttpMethod.Get,
                    uri);

                if (config.Method == RequestMethod.Post)
                {
                    request.Content = string.IsNullOrEmpty(config.ContentType) ?
                        new StringContent(content, Encoding.UTF8) :
                        new StringContent(content, Encoding.UTF8, config.ContentType);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при создании запроса: {0}" :
                    "Error creating request: {0}", ex.Message);
                httpClient = null;
                request = null;
                return false;
            }
        }

        /// <summary>
        /// Sends a notification using the specified request.
        /// </summary>
        private bool SendNotification(HttpRequestMessage request, string tagCode)
        {
            try
            {
                // send request and receive response
                Log.WriteLine(Locale.IsRussian ?
                    "Отправка запроса:" :
                    "Send request:");
                Log.WriteLine(request.RequestUri.ToString());

                stopwatch.Restart();
                HttpResponseMessage response = httpClient.SendAsync(request).Result;
                HttpStatusCode responseStatus = response.StatusCode;
                string responseContent = response.Content.ReadAsStringAsync().Result;
                stopwatch.Stop();

                // output response to log
                Log.WriteLine(Locale.IsRussian ?
                    "Ответ получен за {0} мс. Статус: {1} ({2})" :
                    "Response received in {0} ms. Status: {1} ({2})",
                    stopwatch.ElapsedMilliseconds, (int)responseStatus, responseStatus);

                if (responseContent.Length > 0)
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Содержимое ответа:" :
                        "Response content:");

                    if (responseContent.Length <= ResponseDisplayLenght)
                    {
                        Log.WriteLine(responseContent);
                    }
                    else
                    {
                        Log.WriteLine(responseContent.Substring(0, ResponseDisplayLenght));
                        Log.WriteLine("...");
                    }
                }

                // update tag values
                DeviceData.Add(tagCode, 1);
                DeviceData.Set(TagCode.Response, (int)responseStatus);

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при отправке запроса: {0}" :
                    "Error sending request: {0}", ex.Message);
                DeviceData.Invalidate(TagCode.Response);
                return false;
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            isReady = false;
            loggingFlag = false;

            // load device configuration
            string fileName = NotifDeviceConfig.GetFileName(DeviceNum);
            string errMsg;

            if (Storage.GetFileInfo(DataCategory.Config, fileName).Exists)
            {
                if (!config.Load(Storage, fileName, out errMsg))
                    Log.WriteLine(errMsg);
            }
            else
            {
                // get URI from command line for backward compatibility
                config.Uri = PollingOptions.CmdLine;
            }

            // initialize variables if configuration is valid
            if (ValidateDeviceConfig(out errMsg))
            {
                if (config.ParamEnabled)
                {
                    paramUri = new ParamString(config.Uri, config.ParamBegin, config.ParamEnd);
                    paramContent = new ParamString(config.Content, config.ParamBegin, config.ParamEnd);
                }
                else
                {
                    paramUri = null;
                    paramContent = null;
                }

                addressBook = AddressBookUtils.GetOrLoad(LineContext.SharedData, Storage, Log);
                isReady = true;
                loggingFlag = true;
            }
            else
            {
                Log.WriteLine(errMsg);
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            DeviceTags.AddGroup(CnlPrototypeFactory.GetCnlPrototypeGroup().ToTagGroup());
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public override void InitDeviceData()
        {
            base.InitDeviceData();

            // reset counters
            DeviceData.Set(TagCode.Notif, 0);
            DeviceData.Set(TagCode.Request, 0);

            // set device status
            if (isReady)
            {
                DeviceStatus = DeviceStatus.Normal;
                DeviceData.SetStatusTag(DeviceStatus);
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (loggingFlag)
            {
                loggingFlag = false;
                Log.WriteLine();
                WriteReadyStatus();
            }

            SleepPollingDelay();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            base.SendCommand(cmd);
            LastRequestOK = false;

            if (isReady)
            {
                string cmdCode = "";
                Dictionary<string, string> args = null;

                if (cmd.CmdCode == TagCode.Notif || cmd.CmdNum == 1)
                {
                    cmdCode = TagCode.Notif;
                    args = GetNotifArgs(cmd);
                }
                else if (cmd.CmdCode == TagCode.Request || cmd.CmdNum == 2)
                {
                    cmdCode = TagCode.Request;
                    args = GetRequestArgs(cmd);
                }

                if (cmdCode != "" && args != null)
                {
                    if (CreateRequest(args, out HttpRequestMessage request))
                    {
                        int tryNum = 0;

                        while (RequestNeeded(ref tryNum))
                        {
                            LastRequestOK = SendNotification(request, cmdCode);
                            FinishRequest();
                            tryNum++;
                        }
                    }
                }
                else
                {
                    Log.WriteLine(CommPhrases.InvalidCommand);
                }

                loggingFlag = true;
            }
            else
            {
                WriteReadyStatus();
            }

            FinishCommand();
        }
    }
}
