// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgMimicEditor.Code;

namespace Scada.Web.Plugins.PlgMimicEditor.Areas.MimicEditor.Pages
{
    /// <summary>
    /// Represents a page for displaying open mimic diagrams.
    /// <para>Представляет страницу для отображения открытых мнемосхем.</para>
    /// </summary>
    public class MimicListModel(EditorManager editorManager) : PageModel
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";

        [BindProperty]
        public string FileName { get; set; }


        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            OpenResult result = editorManager.OpenMimic(FileName);

            if (result.IsSuccessful)
            {
                return LocalRedirect("~/MimicEdit/" + result.EditorKey);
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }
        }
    }
}
