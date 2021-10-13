// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Lang;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtCommConfigLogic : ExtensionLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtCommConfigLogic(IAdminContext adminContext)
            : base(adminContext)
        {
        }


        /// <summary>
        /// Gets the extension code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "ExtCommConfig";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);

            //ExtensionPhrases.Init();
        }

        /// <summary>
        /// Gets tree nodes to add to the explorer tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes(object relatedObject)
        {
            if (relatedObject is not CommApp commApp)
                return null;

            return null;
        }

        /// <summary>
        /// Gets the images used by the explorer tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return null;
        }
    }
}
