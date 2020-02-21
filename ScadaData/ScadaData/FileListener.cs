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
 * Summary  : Represents a listener that detects the presence of a file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System;
using System.IO;
using System.Threading;

namespace Scada
{
    /// <summary>
    /// Represents a listener that detects the presence of a file.
    /// <para>Представляет прослушиватель, который обнаруживает присутствие файла.</para>
    /// <remarks>The class is used by console applications to receive a stop command.</remarks>
    /// </summary>
    public class FileListener
    {
        private readonly string fileName; // the observed file
        private readonly Thread thread;   // the thread to detect the file
        private volatile bool terminated; // necessary to stop the thread

        /// <summary>
        /// A value indicating whether the file is found.
        /// </summary>
        public volatile bool FileFound;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FileListener(string fileName)
        {
            this.fileName = fileName ?? throw new ArgumentNullException("fileName");
            terminated = false;
            FileFound = false;

            thread = new Thread(new ThreadStart(() =>
            {
                while (!(terminated || FileFound))
                {
                    if (File.Exists(fileName))
                        FileFound = true;
                    else
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }));

            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }


        /// <summary>
        /// Deletes the observed file.
        /// </summary>
        public void DeleteFile()
        {
            try { File.Delete(fileName); }
            catch { }
        }

        /// <summary>
        /// Terminates the file detecting thread.
        /// </summary>
        public void Terminate()
        {
            terminated = true;
        }
    }
}
