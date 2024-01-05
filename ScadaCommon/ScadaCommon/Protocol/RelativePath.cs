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
 * Summary  : Represents a relative file or directory path
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using System.Text;

namespace Scada.Protocol
{
    /// <summary>
    /// Represents a relative file or directory path.
    /// <para>Представляет относительный путь файла или директории.</para>
    /// </summary>
    public class RelativePath
    {
        /// <summary>
        /// Represents an empty path.
        /// </summary>
        public static readonly RelativePath Empty = new RelativePath();


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath()
        {
            DirectoryID = 0;
            Path = "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath(int directoryID, string path = "")
        {
            DirectoryID = directoryID;
            Path = path ?? "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath(TopFolder topFolder)
        {
            DirectoryID = (byte)topFolder;
            Path = "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath(TopFolder topFolder, AppFolder appFolder, string path = "")
        {
            DirectoryID = (byte)topFolder | ((byte)appFolder << 8);
            Path = path ?? "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath(ServiceApp serviceApp, AppFolder appFolder, string path = "")
            : this(GetTopFolder(serviceApp), appFolder, path)
        {
        }


        /// <summary>
        /// Gets the directory ID based to the top and application folders.
        /// </summary>
        public int DirectoryID { get; set; }

        /// <summary>
        /// Gets or sets the top folder.
        /// </summary>
        public TopFolder TopFolder
        {
            get
            {
                return (TopFolder)(byte)DirectoryID;
            }
            set
            {
                DirectoryID |= (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the application folder.
        /// </summary>
        public AppFolder AppFolder
        {
            get
            {
                return (AppFolder)(byte)(DirectoryID >> 8);
            }
            set
            {
                DirectoryID |= (byte)value << 8;
            }
        }

        /// <summary>
        /// Gets or sets the path relative to the application folder.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets a value indicating whether the path is a file search mask.
        /// </summary>
        public bool IsMask
        {
            get
            {
                return Path != null && (Path.IndexOf('*') >= 0 || Path.IndexOf('?') >= 0);
            }
        }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sbPath = new StringBuilder();

            if (TopFolder != TopFolder.Root)
                sbPath.Append('[').Append(TopFolder).Append("]\\");

            if (AppFolder != AppFolder.Root)
                sbPath.Append('[').Append(AppFolder).Append("]\\");

            sbPath.Append(Path);
            return sbPath.ToString();
        }

        /// <summary>
        /// Gets the top folder corresponding to the specified service application.
        /// </summary>
        public static TopFolder GetTopFolder(ServiceApp serviceApp)
        {
            switch (serviceApp)
            {
                case ServiceApp.Server:
                    return TopFolder.Server;

                case ServiceApp.Comm:
                    return TopFolder.Comm;

                case ServiceApp.Web:
                    return TopFolder.Web;

                default:
                    throw new ScadaException("Unable to define top folder.");
            }
        }
    }
}
