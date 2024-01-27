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
 * Module   : Administrator
 * Summary  : Creates project files
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using System.IO;
using System.Xml;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates project files.
    /// <para>Создаёт файлы проекта.</para>
    /// </summary>
    internal static class FileCreator
    {
        /// <summary>
        /// Creates a new file of the specified type.
        /// </summary>
        public static void CreateFile(string fileName, KnownFileType fileType)
        {
            using FileStream stream = new(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read);

            if (fileType == KnownFileType.SchemeView ||
                fileType == KnownFileType.TableView ||
                fileType == KnownFileType.XmlFile)
            {
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                if (fileType == KnownFileType.SchemeView)
                    xmlDoc.AppendChild(xmlDoc.CreateElement("SchemeView"));
                else if (fileType == KnownFileType.TableView)
                    xmlDoc.AppendChild(xmlDoc.CreateElement("TableView"));

                xmlDoc.Save(stream);
            }
        }

        /// <summary>
        /// Gets an extension corresponds to the file type.
        /// </summary>
        public static string GetExtension(KnownFileType fileType)
        {
            return fileType switch
            {
                KnownFileType.SchemeView => "sch",
                KnownFileType.TableView => "tbl",
                KnownFileType.TextFile => "txt",
                KnownFileType.XmlFile => "xml",
                _ => ""
            };
        }

        /// <summary>
        /// Gets a file type by the extension.
        /// </summary>
        public static KnownFileType GetFileType(string extension)
        {
            return extension?.ToLowerInvariant() switch
            {
                "sch" => KnownFileType.SchemeView,
                "tbl" => KnownFileType.TableView,
                "txt" => KnownFileType.TextFile,
                "xml" => KnownFileType.XmlFile,
                _ => KnownFileType.None
            };
        }

        /// <summary>
        /// Checks whether the extension is known.
        /// </summary>
        public static bool ExtensionIsKnown(string extension)
        {
            return GetFileType(extension) != KnownFileType.None;
        }
    }
}
