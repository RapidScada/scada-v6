// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Lang;
using Scada.Storages;
using System.Text;
using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Logic.Replication
{
    /// <summary>
    /// Represents a state of archive replication.
    /// <para>Представляет состояние репликации архива.</para>
    /// </summary>
    internal class ArcReplicationState
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcReplicationState()
            : base()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the current data position, UTC.
        /// </summary>
        public DateTime PositionDT { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the state was modified and should be saved.
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether replication is in error state.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current step is in progress and not completed.
        /// </summary>
        public bool StepInProgress { get; set; }

        /// <summary>
        /// Gets or sets the time range of the current step.
        /// </summary>
        public TimeRange StepTimeRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether data should be exported.
        /// </summary>
        public bool ExportRequired { get; set; }

        /// <summary>
        /// Gets or sets the archive index: 0 - historical, 1 - events.
        /// </summary>
        public int ArchiveIndex { get; set; }

        /// <summary>
        /// Gets or sets the channel group index.
        /// </summary>
        public int GroupIndex { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            PositionDT = DateTime.MinValue;
            Modified = false;
            HasError = false;
            StepInProgress = false;
            StepTimeRange = new TimeRange();
            ExportRequired = false;
            ArchiveIndex = 0;
            GroupIndex = 0;
        }

        /// <summary>
        /// Loads the state from the specified storage.
        /// </summary>
        public bool Load(IStorage storage, string fileName, out string errMsg)
        {
            ArgumentNullException.ThrowIfNull(storage, nameof(storage));

            try
            {
                SetToDefault();

                if (storage.GetFileInfo(DataCategory.Storage, fileName).Exists)
                {
                    using TextReader reader = storage.OpenText(DataCategory.Storage, fileName);
                    XmlDocument xmlDoc = new();
                    xmlDoc.Load(reader);
                    PositionDT = xmlDoc.DocumentElement.GetChildAsDateTime("PositionDT");
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при загрузке состояния репликации" :
                    "Error loading replication state");
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        public bool Save(IStorage storage, string fileName, out string errMsg)
        {
            ArgumentNullException.ThrowIfNull(storage, nameof(storage));

            try
            {
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ArcReplicationState");
                xmlDoc.AppendChild(rootElem);
                rootElem.AppendElem("PositionDT", PositionDT);

                storage.WriteText(DataCategory.Storage, fileName, xmlDoc.GetFormattedXml());
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(Locale.IsRussian ?
                    "Ошибка при сохранении состояния репликации" :
                    "Error saving replication state");
                return false;
            }
        }

        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sbInfo)
        {
            ArgumentNullException.ThrowIfNull(sbInfo, nameof(sbInfo));

            if (Locale.IsRussian)
            {
                sbInfo
                    .Append("Состояние       : ").AppendLine(HasError ? "ошибка" : "норма")
                    .Append("Позиция         : ").Append(PositionDT.ToLocalTime().ToLocalizedString()).AppendLine()
                    .Append("Индекс архива   : ").Append(ArchiveIndex).AppendLine()
                    .Append("Индекс группы   : ").Append(GroupIndex).AppendLine();
            }
            else
            {
                sbInfo
                    .Append("Status         : ").AppendLine(HasError ? "Error" : "Normal")
                    .Append("Position       : ").Append(PositionDT.ToLocalTime().ToLocalizedString()).AppendLine()
                    .Append("Archive index  : ").Append(ArchiveIndex).AppendLine()
                    .Append("Group index    : ").Append(GroupIndex).AppendLine();
            }
        }
    }
}
