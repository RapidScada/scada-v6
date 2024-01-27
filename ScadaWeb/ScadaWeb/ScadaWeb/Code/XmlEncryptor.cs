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
 * Module   : Webstation Application
 * Summary  : Encrypts and decrypts XML elements to protect authentication keys
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.DependencyInjection;
using Scada.Log;
using System;
using System.Xml.Linq;

namespace Scada.Web.Code
{
    /// <summary>
    /// Encrypts and decrypts XML elements to protect authentication keys.
    /// <para>Шифрует и дешифрует XML-элементы для защиты ключей аутентификации.</para>
    /// </summary>
    internal class XmlEncryptor : IXmlEncryptor, IXmlDecryptor
    {
        private readonly ILog log;


        public XmlEncryptor(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public XmlEncryptor(IServiceProvider services)
        {
            log = services.GetRequiredService<ILog>();
        }


        public EncryptedXmlInfo Encrypt(XElement plaintextElement)
        {
            ArgumentNullException.ThrowIfNull(plaintextElement, nameof(plaintextElement));

            try
            {
                XNamespace xNamespace = "http://schemas.asp.net/2015/03/dataProtection";
                string encryptedValue = ScadaUtils.Encrypt(plaintextElement.ToString());
                return new EncryptedXmlInfo(new XElement(xNamespace + "value", encryptedValue), typeof(XmlEncryptor));
            }
            catch (Exception ex)
            {
                log.WriteError(ex);
                throw;
            }
        }

        public XElement Decrypt(XElement encryptedElement)
        {
            ArgumentNullException.ThrowIfNull(encryptedElement, nameof(encryptedElement));

            try
            {
                string decryptedValue = ScadaUtils.Decrypt(encryptedElement.Value);
                return XElement.Parse(decryptedValue);
            }
            catch (Exception ex)
            {
                log.WriteError(ex);
                throw;
            }
        }
    }
}
