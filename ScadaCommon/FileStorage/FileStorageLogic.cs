// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Adapters;
using Scada.Data.Tables;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scada.Storages.FileStorage
{
    /// <summary>
    /// Represents a storage logic.
    /// <para>Представляет логику хранилища.</para>
    /// </summary>
    public class FileStorageLogic : StorageLogic
    {
        private readonly string baseDir; // the directory of the configuration database in DAT format
        private readonly string viewDir; // the directory of views


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FileStorageLogic(StorageContext storageContext)
            : base(storageContext)
        {
            string instanceDir = storageContext.AppDirs.InstanceDir;
            baseDir = Path.Combine(instanceDir, "BaseDAT");
            viewDir = Path.Combine(instanceDir, "Views");
            IsFileStorage = true;
        }


        /// <summary>
        /// Gets the fully qualified file name for the specified data category and path.
        /// </summary>
        private string GetFileName(DataCategory category, string path)
        {
            path = ScadaUtils.NormalPathSeparators(path);

            switch (category)
            {
                case DataCategory.Config:
                    return Path.Combine(StorageContext.AppDirs.ConfigDir, path);

                case DataCategory.Storage:
                    return Path.Combine(StorageContext.AppDirs.StorageDir, path);

                case DataCategory.View:
                    return Path.Combine(viewDir, path);

                default:
                    throw new ScadaException("Data category not supported.");
            }
        }

        /// <summary>
        /// Reads text from the file.
        /// </summary>
        public override string ReadText(DataCategory category, string path)
        {
            string fileName = GetFileName(category, path);
            return File.ReadAllText(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// Reads a byte array from the file.
        /// </summary>
        public override byte[] ReadBytes(DataCategory category, string path)
        {
            string fileName = GetFileName(category, path);
            return File.ReadAllBytes(fileName);
        }

        /// <summary>
        /// Reads the table of the configuration database.
        /// </summary>
        public override void ReadBaseTable(IBaseTable baseTable)
        {
            BaseTableAdapter adapter = new BaseTableAdapter
            {
                FileName = Path.Combine(baseDir, baseTable.FileNameDat)
            };
            adapter.Fill(baseTable);
        }

        /// <summary>
        /// Writes the text to the file.
        /// </summary>
        public override void WriteText(DataCategory category, string path, string contents)
        {
            string fileName = GetFileName(category, path);
            File.WriteAllText(fileName, contents, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the byte array to the file.
        /// </summary>
        public override void WriteBytes(DataCategory category, string path, byte[] bytes)
        {
            string fileName = GetFileName(category, path);
            File.WriteAllBytes(fileName, bytes);
        }

        /// <summary>
        /// Opens a text file for reading.
        /// </summary>
        public override TextReader OpenText(DataCategory category, string path)
        {
            string fileName = GetFileName(category, path);
            return File.OpenText(fileName);
        }

        /// <summary>
        /// Opens a binary file for reading.
        /// </summary>
        public override BinaryReader OpenBinary(DataCategory category, string path)
        {
            string fileName = GetFileName(category, path);
            return new BinaryReader(File.OpenRead(fileName), Encoding.UTF8, false);
        }

        /// <summary>
        /// Gets information associated with the file.
        /// </summary>
        public override ShortFileInfo GetFileInfo(DataCategory category, string path)
        {
            FileInfo fileInfo = new FileInfo(GetFileName(category, path));
            return fileInfo.Exists
                ? new ShortFileInfo
                {
                    Exists = true,
                    LastWriteTime = fileInfo.LastWriteTimeUtc,
                    Length = fileInfo.Length
                }
                : ShortFileInfo.FileNotExists;
        }

        /// <summary>
        /// Gets a list of file paths that match the specified pattern.
        /// </summary>
        public override ICollection<string> GetFileList(DataCategory category, string path, string searchPattern)
        {
            return Directory.GetFiles(GetFileName(category, path), searchPattern, SearchOption.AllDirectories);
        }
    }
}
