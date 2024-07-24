// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtTableEditor.Forms;
using Scada.Admin.Lang;
using Scada.Lang;

namespace Scada.Admin.Extensions.ExtTableEditor
{
    /// <summary>
    /// Represents an extension logic.
    /// <para>Представляет логику расширения.</para>
    /// </summary>
    public class ExtTableEditorLogic : ExtensionLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExtTableEditorLogic(IAdminContext adminContext)
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
                return "ExtTableEditor";
            }
        }

        /// <summary>
        /// Gets the extension name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ? "Редактор Таблиц" : "Table Editor";
            }
        }

        /// <summary>
        /// Gets the extension description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Расширение предоставляет редактор для создания табличных представлений." :
                    "The extension provides an editor for creating table views.";
            }
        }

        /// <summary>
        /// Gets the file extensions for which the extension provides an editor.
        /// </summary>
        public override ICollection<string> FileExtensions => ["tbl"];


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AdminContext.AppDirs.LangDir, Code, out string errMsg))
                AdminContext.ErrLog.WriteError(AdminPhrases.ExtensionMessage, Code, errMsg);
        }

        /// <summary>
        /// Gets a form to edit the specified file.
        /// </summary>
        public override Form GetEditorForm(string fileName)
        {
            return new FrmTableEditor(AdminContext, fileName);
        }
    }
}
