﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Scada.Comm.Drivers.DrvSms.Logic.Protocol
{
    /// <summary>
    /// Provides methods for encoding and decoding messages in the SMS PDU mode.
    /// <para>Предоставляет методы для кодирования и декодирования сообщений в режиме SMS PDU.</para>
    /// </summary>
    public static class PduConverter
    {
        // The masks for 7-bit decoding.
        private static readonly byte[] MaskL = new byte[] { 0xFE, 0xFC, 0xF8, 0xF0, 0xE0, 0xC0, 0x80 };
        private static readonly byte[] MaskR = new byte[] { 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F, 0x7F };


        /// <summary>
        /// Encodes the phone number.
        /// </summary>
        private static string EncodePhone(string phoneNumber)
        {
            StringBuilder result = new StringBuilder();
            int phoneLen = phoneNumber.Length;

            if (phoneLen > 0)
            {
                if (phoneNumber[0] == '+')
                {
                    phoneNumber = phoneNumber.Substring(1);
                    result.Append("91");
                    phoneLen--;
                }
                else
                {
                    result.Append("81");
                }

                int i = 1;

                while (i < phoneLen)
                {
                    result.Append(phoneNumber[i]);
                    result.Append(phoneNumber[i - 1]);
                    i += 2;
                }

                if (i == phoneLen)
                {
                    result.Append('F');
                    result.Append(phoneNumber[i - 1]);
                }
            }

            return phoneLen.ToString("X2") + result.ToString();
        }

        /// <summary>
        /// Decodes the phone number.
        /// </summary>
        private static string DecodePhone(string phoneNumber)
        {
            StringBuilder result = new StringBuilder();

            if (phoneNumber.StartsWith("91"))
                result.Append("+");

            for (int i = 2; i < phoneNumber.Length; i += 2)
            {
                if (i + 1 < phoneNumber.Length)
                {
                    char c = phoneNumber[i + 1];
                    if ('0' <= c && c <= '9')
                        result.Append(c);

                    c = phoneNumber[i];
                    if ('0' <= c && c <= '9')
                        result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Encodes the text in 7-bit encoding.
        /// </summary>
        private static List<byte> Encode7bitText(string text)
        {
            List<byte> result = new List<byte>();
            byte[] bytes = Encoding.ASCII.GetBytes(text);

            byte bit = 7; // from 7 to 1
            int i = 0;
            int len = bytes.Length;

            while (i < len)
            {
                byte sym = (byte)(bytes[i] & 0x7F);
                byte nextSym = i < len - 1 ? (byte)(bytes[i + 1] & 0x7F) : (byte)0;
                byte code = (byte)(sym >> 7 - bit | nextSym << bit);

                if (bit == 1)
                {
                    i++;
                    bit = 7;
                }
                else
                {
                    bit--;
                }

                result.Add(code);
                i++;
            }

            return result;
        }

        /// <summary>
        /// Decodes the text from 7-bit encoding.
        /// </summary>
        private static string Decode7bitText(string text)
        {
            // replace pairs of characters with their hexadecimal values and write to buffer
            byte[] buf = new byte[text.Length / 2];
            int bufPos = 0;

            for (int i = 0; i < text.Length - 1; i += 2)
            {
                buf[bufPos++] = byte.Parse(text.Substring(i, 2), NumberStyles.HexNumber);
            }

            // decode
            byte[] result = new byte[text.Length];
            int resPos = 0;
            byte part = 0;
            byte bit = 7;

            for (int i = 0; i < bufPos; i++)
            {
                byte b = buf[i];
                byte sym = (byte)(part >> bit + 1 | (b & MaskR[bit - 1]) << 7 - bit);
                part = (byte)(b & MaskL[bit - 1]);
                result[resPos++] = sym;

                if (--bit == 0)
                {
                    sym = (byte)((b & 0xFE) >> 1);
                    part = 0;

                    result[resPos++] = sym;
                    bit = 7;
                }
            }

            return resPos > 0 ? new string(Encoding.ASCII.GetChars(result, 0, resPos)) : "";
        }

        /// <summary>
        /// Decodes the text from 8-bit encoding.
        /// </summary>
        private static string Decode8bitText(string text)
        {
            byte[] buf = new byte[text.Length / 2];
            int bufPos = 0;

            for (int i = 0; i < text.Length - 1; i += 2)
            {
                buf[bufPos++] = byte.Parse(text.Substring(i, 2), NumberStyles.HexNumber);
            }

            return bufPos > 0 ? new string(Encoding.Default.GetChars(buf, 0, bufPos)) : "";
        }

        /// <summary>
        /// Encodes the text in Unicode.
        /// </summary>
        private static List<byte> EncodeUnicodeText(string text)
        {
            List<byte> result = new List<byte>();

            for (int i = 0; i < text.Length; i++)
            {
                int val = char.ConvertToUtf32(text, i);
                result.Add((byte)(val >> 8 & 0xFF));
                result.Add((byte)(val & 0xFF));
            }

            return result;
        }

        /// <summary>
        /// Decodes the text from Unicode.
        /// </summary>
        private static string DecodeUnicodeText(string text)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i += 4)
            {
                int val = int.Parse(text.Substring(i, 4), NumberStyles.HexNumber);
                string s;
                try { s = char.ConvertFromUtf32(val); }
                catch { s = " "; }
                result.Append(s);
            }

            return result.ToString();
        }


        /// <summary>
        /// Encodes a Protocol Data Unit for sending message.
        /// </summary>
        public static Pdu EncodePDU(string phoneNumber, string messageText)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));
            if (messageText == null)
                throw new ArgumentNullException(nameof(messageText));

            // choose encoding
            bool sevenBit = true;
            for (int i = 0; i < messageText.Length && sevenBit; i++)
            {
                char c = messageText[i];
                if ((c < ' ' || c > 'z') && c != '\n')
                    sevenBit = false;
            }

            // set text length allowed for transmission
            const int Max7bitMsgLen = 160;
            const int MaxUnicodeMsgLen = 70;
            int maxMsgLen = sevenBit ? Max7bitMsgLen : MaxUnicodeMsgLen;

            if (messageText.Length > maxMsgLen)
                messageText = messageText.Substring(0, maxMsgLen);

            // build PDU
            StringBuilder sbPdu = new StringBuilder();
            sbPdu.Append("00");                     // Service Center Adress (SCA)
            sbPdu.Append("01");                     // PDU-type
            sbPdu.Append("00");                     // Message Reference (MR)
            sbPdu.Append(EncodePhone(phoneNumber)); // Destination Adress (DA)
            sbPdu.Append("00");                     // Protocol Identifier (PID)

            byte dcs;                               // Data Coding Scheme (DCS)
            List<byte> ud;                          // User Data (UD)
            byte udl;                               // User Data Length (UDL)

            if (sevenBit)
            {
                dcs = 0x00;
                ud = Encode7bitText(messageText);
                udl = (byte)messageText.Length;
            }
            else
            {
                dcs = 0x08;
                ud = EncodeUnicodeText(messageText);
                udl = (byte)ud.Count;
            }

            sbPdu.Append(dcs.ToString("X2"));
            sbPdu.Append(udl.ToString("X2"));

            foreach (byte b in ud)
            {
                sbPdu.Append(b.ToString("X2"));
            }

            return new Pdu
            {
                Data = sbPdu.ToString(),
                Length = (sbPdu.Length - 2) / 2
            };
        }

        /// <summary>
        /// Decodes the PDU and writes results to the message.
        /// </summary>
        public static bool DecodePDU(string pdu, Message msg, out string logMsg)
        {
            try
            {
                int scaLen = int.Parse(pdu.Substring(0, 2));         // length of SMSC number
                int oaPos = scaLen * 2 + 4;                          // position of sender number

                if (msg.Length == (pdu.Length - oaPos + 2) / 2)
                {
                    int oaLen = int.Parse(pdu.Substring(oaPos, 2),
                        NumberStyles.HexNumber);                     // length of sender number
                    if (oaLen % 2 > 0) oaLen++;
                    msg.Phone = DecodePhone(pdu.Substring(oaPos + 2, oaLen + 2));

                    int sctsPos = oaPos + oaLen + 8;                 // position of timestamp
                    msg.Timestamp = new DateTime(int.Parse("20" + pdu[sctsPos + 1] + pdu[sctsPos]),
                        int.Parse(pdu[sctsPos + 3].ToString() + pdu[sctsPos + 2]),
                        int.Parse(pdu[sctsPos + 5].ToString() + pdu[sctsPos + 4]),
                        int.Parse(pdu[sctsPos + 7].ToString() + pdu[sctsPos + 6]),
                        int.Parse(pdu[sctsPos + 9].ToString() + pdu[sctsPos + 8]),
                        int.Parse(pdu[sctsPos + 11].ToString() + pdu[sctsPos + 10]));

                    string dcs = pdu.Substring(sctsPos - 2, 2);      // encoding
                    int udPos = sctsPos + 16;                        // position of message text
                    int udl = int.Parse(pdu.Substring(udPos - 2, 2),
                        NumberStyles.HexNumber);                     // length of message text
                    string ud = pdu.Substring(udPos);                // message text

                    // check length of message text, different modems calculate UDL differently
                    if (dcs == "00" && ud.Length * 4 / 7 == udl || 
                        dcs != "00" && ud.Length == udl * 2)
                    {
                        logMsg = "";
                    }
                    else
                    {
                        logMsg = string.Format(Locale.IsRussian ?
                            "Некорректная длина текста сообщения" :
                            "Incorrect message length");
                    }

                    // decode message
                    if (dcs == "00")
                        msg.Text = Decode7bitText(ud);
                    else if (dcs == "F6")
                        msg.Text = Decode8bitText(ud);
                    else if (dcs == "08")
                        msg.Text = DecodeUnicodeText(ud);

                    return true;
                }
                else
                {
                    logMsg = string.Format(Locale.IsRussian ?
                        "Некорректная длина PDU" :
                        "Incorrect PDU length");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logMsg = string.Format(Locale.IsRussian ?
                    "Невозможно расшифровать PDU: {0}" :
                    "Unable to decode PDU: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Fills the list of messages from the device response to AT+CMGL command.
        /// </summary>
        public static void FillMessageList(List<Message> messages, List<string> response, out string logMsg)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            StringBuilder sbLogMsg = new StringBuilder();
            int lineNum = 1;
            int lineCnt = response.Count;

            while (lineNum <= lineCnt)
            {
                string line = response[lineNum - 1].Trim();
                if (line.StartsWith("+CMGL: ") && line.Length > 7)
                {
                    // get message index, status and length
                    Message msg = new Message();
                    bool paramsOK = false;
                    string[] parts = line.Substring(7).Split(new char[] { ',' }, StringSplitOptions.None);

                    if (parts.Length >= 3)
                    {
                        if (int.TryParse(parts[0], out int val1) &&
                            int.TryParse(parts[1], out int val2) &&
                            int.TryParse(parts[parts.Length - 1], out int val3))
                        {
                            paramsOK = true;
                            msg.Index = val1;
                            msg.Status = val2;
                            msg.Length = val3;
                            lineNum++;
                        }
                    }

                    // decode PDU
                    if (paramsOK)
                    {
                        if (lineNum <= lineCnt)
                        {
                            if (DecodePDU(response[lineNum - 1].Trim(), msg, out string logMsg2))
                            {
                                messages.Add(msg);

                                if (!string.IsNullOrEmpty(logMsg2))
                                {
                                    sbLogMsg.AppendLine(string.Format(Locale.IsRussian ?
                                        "Предупреждение в строке {0}: {1}" :
                                        "Warning in line {0}: {1}", lineNum, logMsg2));
                                }
                            }
                            else
                            {
                                sbLogMsg.AppendLine(string.Format(Locale.IsRussian ?
                                    "Ошибка в строке {0}: {1}" :
                                    "Error in line {0}: {1}", lineNum, logMsg2));
                            }
                        }
                        else
                        {
                            sbLogMsg.AppendLine(Locale.IsRussian ?
                                "Ошибка: Некорректное завершение входных данных" :
                                "Error: Incorrect termination of the input data");
                        }
                    }
                    else
                    {
                        sbLogMsg.AppendLine(string.Format(Locale.IsRussian ?
                            "Ошибка в строке {0}: Некорректные параметры сообщения" :
                            "Error in line {0}: Incorrect message parameters", lineNum));
                    }
                }

                lineNum++;
            }

            logMsg = sbLogMsg.ToString();
        }
    }
}
