// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Authorization;
using Scada.Web.Plugins.PlgMimicEditor.Code;

namespace Scada.Web.Plugins.PlgMimicEditor.Areas.MimicEditor.Pages
{
    /// <summary>
    /// Represents a page for opening a mimic diagram.
    /// <para>Представляет страницу для открытия мнемосхемы.</para>
    /// </summary>
    [Authorize(Policy = PolicyName.Administrators)]
    public class MimicOpenModel(EditorManager editorManager) : PageModel
    {
        public string ErrorMessage { get; private set; } = "";
        public string FileName { get; private set; } = "";


        public IActionResult OnGet(string fileName)
        {
            FileName = fileName;
            OpenResult result = editorManager.OpenMimic(fileName);

            if (result.IsSuccessful)
            {
                return RedirectToPage("MimicEdit", new { key = result.MimicKey });
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }
        }
    }
}
