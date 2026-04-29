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
    /// Represents a page for displaying open mimic diagrams.
    /// <para>Представляет страницу для отображения открытых мнемосхем.</para>
    /// </summary>
    [Authorize(Policy = PolicyName.Administrators)]
    public class MimicListModel(EditorManager editorManager) : PageModel
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";
        public MimicGroup[] MimicGroups { get; private set; } = [];

        [BindProperty]
        public string FileName { get; set; }


        public void OnGet()
        {
            MimicGroups = editorManager.GetMimicGroups();
        }

        public IActionResult OnPostOpen()
        {
            OpenResult result = editorManager.OpenMimic(FileName);

            if (result.IsSuccessful)
            {
                return RedirectToPage(); // Post-Redirect-Get pattern
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                MimicGroups = editorManager.GetMimicGroups();
                return Page();
            }
        }

        public IActionResult OnPostSave(long mimicKey)
        {
            if (editorManager.SaveMimic(mimicKey, out string errMsg))
            {
                return RedirectToPage();
            }
            else
            {
                ErrorMessage = errMsg;
                MimicGroups = editorManager.GetMimicGroups();
                return Page();
            }
        }

        public IActionResult OnPostSaveAndClose(long mimicKey)
        {
            if (editorManager.SaveMimic(mimicKey, out string errMsg))
            {
                editorManager.CloseMimic(mimicKey);
                return RedirectToPage();
            }
            else
            {
                ErrorMessage = errMsg;
                MimicGroups = editorManager.GetMimicGroups();
                return Page();
            }
        }

        public IActionResult OnPostCloseWithoutSaving(long mimicKey)
        {
            editorManager.CloseMimic(mimicKey);
            return RedirectToPage();
        }
    }
}
