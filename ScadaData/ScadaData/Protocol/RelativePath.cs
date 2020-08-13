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
 * Summary  : Represents a relative file or directory path
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

namespace Scada.Protocol
{
    /// <summary>
    /// Represents a relative file or directory path.
    /// <para>Представляет относительный путь файла или директории.</para>
    /// </summary>
    public class RelativePath
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath()
        {
            TopFolder = TopFolder.Undefined;
            AppFolder = AppFolder.Root;
            Path = "";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RelativePath(TopFolder topFolder, AppFolder appFolder, string path = "")
        {
            TopFolder = topFolder;
            AppFolder = appFolder;
            Path = path ?? "";
        }


        /// <summary>
        /// Gets or sets the top folder.
        /// </summary>
        public TopFolder TopFolder { get; set; }

        /// <summary>
        /// Gets or sets the application folder.
        /// </summary>
        public AppFolder AppFolder { get; set; }

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
        /// Gets the directory ID corresponding to the top and application directories.
        /// </summary>
        public int GetDirectoryID()
        {
            return (byte)TopFolder | ((byte)AppFolder << 8);
        }

        /// <summary>
        /// Returns a relative path corresponding to the directory ID.
        /// </summary>
        public static RelativePath FromDirectoryID(int directoryID)
        {
            return new RelativePath(
                (TopFolder)(byte)directoryID,
                (AppFolder)(byte)(directoryID >> 8),
                "");
        }
    }
}
