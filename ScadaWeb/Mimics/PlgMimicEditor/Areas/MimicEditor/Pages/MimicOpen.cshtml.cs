// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgMimicEditor.Code;

namespace Scada.Web.Plugins.PlgMimicEditor.Areas.MimicEditor.Pages
{
    /// <summary>
    /// Represents a page for opening a mimic diagram.
    /// <para>Представляет страницу для открытия мнемосхемы.</para>
    /// </summary>
    public class MimicOpenModel : PageModel
    {
        public string ErrorMessage { get; private set; } = "";
        public string FileName { get; private set; } = "";


        public IActionResult OnGet(string fileName)
        {
            FileName = fileName;
            OpenResult result = new(); //_editorContext.Manager.OpenScheme(fileName);

            if (result.IsSuccessful)
            {
                return LocalRedirect("~/MimicEdit/" + result.EditorID);
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }
        }
    }
}
