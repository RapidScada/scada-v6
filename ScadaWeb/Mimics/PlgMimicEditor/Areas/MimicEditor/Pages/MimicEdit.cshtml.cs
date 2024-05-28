// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgMimicEditor.Code;

namespace Scada.Web.Plugins.PlgMimicEditor.Areas.MimicEditor.Pages
{
    /// <summary>
    /// Represents a page for editing a mimic diagram.
    /// <para>Представляет страницу для редактирования мнемосхемы.</para>
    /// </summary>
    public class MimicEditModel(EditorManager editorManager) : PageModel
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";
        public long EditorKey { get; private set; } = 0;

        public void OnGet(long key)
        {
            EditorKey = key;
            //MimicInstance mimicInstance = editorManager.FindMimic(key);

            //if (mimicInstance == null)
            //{
            //    ErrorMessage = "Mimic diagram not found.";
            //}
        }
    }
}
