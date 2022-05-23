// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Web.Plugins.PlgScheme.Model;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a package containing scheme document properties.
    /// <para>Представляет пакет, содержащий свойства документа схемы.</para>
    /// </summary>
    public class DocumentPacket : SchemePacket
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DocumentPacket(SchemeView schemeView)
        {
            ArgumentNullException.ThrowIfNull(schemeView, nameof(schemeView));
            ViewStamp = schemeView.ViewStamp.ToString();
            SchemeDoc = schemeView.SchemeDoc;
            LoadErrors = schemeView.LoadErrors;
            CnlProps = new List<CnlProps>();
        }


        /// <summary>
        /// Gets the scheme document properties.
        /// </summary>
        public SchemeDocument SchemeDoc { get; }

        /// <summary>
        /// Gets the errors occurred during download.
        /// </summary>
        public List<string> LoadErrors { get; }

        /// <summary>
        /// Gets the properties of the scheme channels.
        /// </summary>
        public List<CnlProps> CnlProps { get; }


        /// <summary>
        /// Fills the channel properties.
        /// </summary>
        public void FillCnlProps(ConfigDataset configDataset)
        {
            ArgumentNullException.ThrowIfNull(configDataset, nameof(configDataset));

            if (SchemeDoc?.SchemeView == null)
                throw new InvalidOperationException("Scheme view must not be null.");

            foreach (int cnlNum in SchemeDoc.SchemeView.CnlNumList)
            {
                if (configDataset.CnlTable.GetItem(cnlNum) is Cnl cnl)
                {
                    CnlProps.Add(new CnlProps(cnl, configDataset));
                }
            }
        }
    }
}
