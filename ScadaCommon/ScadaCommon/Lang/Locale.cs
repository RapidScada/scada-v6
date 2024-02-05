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
 * Summary  : Localization mechanism
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2023
 */

using Scada.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Scada.Lang
{
    /// <summary>
    /// Provides information about a culture of the software package.
    /// <para>Предоставляет информацию о культуре программного комплекса.</para>
    /// </summary>
    public static class Locale
    {
        /// <summary>
        /// The name of the English culture.
        /// </summary>
        private const string EnglishCultureName = "en-GB";
        /// <summary>
        /// The name of the Russian culture.
        /// </summary>
        private const string RussianCultureName = "ru-RU";


        /// <summary>
        /// Initializes the class.
        /// </summary>
        static Locale()
        {
            IsRussian = CultureIsRussian(CultureInfo.CurrentCulture);
            DefaultCulture = Culture = CultureInfo.GetCultureInfo(IsRussian ? RussianCultureName : EnglishCultureName);
            Dictionaries = new Dictionary<string, LocaleDict>();
        }


        /// <summary>
        /// Gets a value indicating whether Russian locale is selected.
        /// </summary>
        public static bool IsRussian { get; private set; }

        /// <summary>
        /// Gets the default culture.
        /// </summary>
        public static CultureInfo DefaultCulture { get; }

        /// <summary>
        /// Gets the culture of the software package.
        /// </summary>
        public static CultureInfo Culture { get; private set; }

        /// <summary>
        /// Gets the loaded dictionaries.
        /// </summary>
        public static Dictionary<string, LocaleDict> Dictionaries { get; }


        /// <summary>
        /// Checks that the specified culture is Russian.
        /// </summary>
        private static bool CultureIsRussian(CultureInfo cultureInfo)
        {
            return cultureInfo != null &&
                (cultureInfo.Name.Equals("ru", StringComparison.OrdinalIgnoreCase) ||
                cultureInfo.Name.StartsWith("ru-", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the dictionary file name depending on the selected culture.
        /// </summary>
        private static string GetDictFileName(string directory, string fileNamePrefix, string cultureName)
        {
            return Path.Combine(directory, fileNamePrefix + "." + cultureName + ".xml");
        }


        /// <summary>
        /// Sets the culture.
        /// </summary>
        public static void SetCulture(string cultureName)
        {
            try
            {
                Culture = string.IsNullOrEmpty(cultureName) 
                    ? DefaultCulture 
                    : CultureInfo.GetCultureInfo(cultureName);
            }
            catch
            {
                Culture = DefaultCulture;
            }
            finally
            {
                IsRussian = CultureIsRussian(Culture);
            }
        }

        /// <summary>
        /// Sets the culture to the English culture.
        /// </summary>
        public static void SetCultureToEnglish()
        {
            Culture = CultureInfo.GetCultureInfo(EnglishCultureName);
            IsRussian = false;
        }

        /// <summary>
        /// Sets the culture to the default.
        /// </summary>
        public static void SetCultureToDefault()
        {
            Culture = DefaultCulture;
            IsRussian = CultureIsRussian(Culture);
        }

        /// <summary>
        /// Loads culture from the instance configuration file.
        /// </summary>
        public static bool LoadCulture(string fileName, out string errMsg)
        {
            InstanceConfig instanceConfig = new InstanceConfig();
            bool result = instanceConfig.Load(fileName, out errMsg);
            SetCulture(instanceConfig.Culture);
            return result;
        }

        /// <summary>
        /// Saves the specified culture to the instance configuration file.
        /// </summary>
        public static bool SaveCulture(string fileName, string cultureName, out string errMsg)
        {
            InstanceConfig instanceConfig = new InstanceConfig();

            if (instanceConfig.Load(fileName, out errMsg))
            {
                instanceConfig.Culture = cultureName;

                if (instanceConfig.Save(fileName, out errMsg))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Loads dictionaries from the specified file.
        /// </summary>
        /// <remarks>
        /// If several dictionaries have the same key, they are merged.
        /// If several phrases within a dictionary have the same key, the last value is taken.
        /// </remarks>
        public static bool LoadDictionaries(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                foreach (XmlElement dictElem in xmlDoc.DocumentElement.SelectNodes("Dictionary"))
                {
                    string dictKey = dictElem.GetAttribute("key");

                    if (!Dictionaries.TryGetValue(dictKey, out LocaleDict dict))
                    {
                        dict = new LocaleDict(dictKey);
                        Dictionaries.Add(dictKey, dict);
                    }

                    foreach (XmlElement phraseElem in dictElem.SelectNodes("Phrase"))
                    {
                        string phraseKey = phraseElem.GetAttribute("key");
                        dict.Phrases[phraseKey] = phraseElem.InnerText;
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format(IsRussian ?
                    "Ошибка при загрузке словарей из файла {0}: {1}" :
                    "Error loading dictionaries from file {0}: {1}", fileName, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Loads dictionaries of the selected culture.
        /// </summary>
        public static bool LoadDictionaries(string directory, string fileNamePrefix, out string errMsg)
        {
            string fileName = GetDictFileName(directory, fileNamePrefix, Culture.Name);
            string fallbackFileName = GetDictFileName(directory, fileNamePrefix, DefaultCulture.Name);

            if (File.Exists(fileName))
            {
                return LoadDictionaries(fileName, out errMsg);
            }
            else if (File.Exists(fallbackFileName))
            {
                if (LoadDictionaries(fallbackFileName, out errMsg))
                {
                    errMsg = string.Format(IsRussian ?
                        "Файл словарей не найден и заменён файлом по умолчанию: {0}" :
                        "Dictionary file not found and replaced with default file: {0}", fileName);
                }

                return false;
            }
            else
            {
                errMsg = string.Format(IsRussian ?
                    "Не найден файл словарей: {0}" :
                    "Dictionary file not found: {0}", fileName);
                return false;
            }
        }

        /// <summary>
        /// Gets the dictionary by the specified key, or an empty dictionary if the key is not found.
        /// </summary>
        public static LocaleDict GetDictionary(string key)
        {
            return Dictionaries.TryGetValue(key, out LocaleDict dict) ? dict : new LocaleDict(key);
        }
    }
}
