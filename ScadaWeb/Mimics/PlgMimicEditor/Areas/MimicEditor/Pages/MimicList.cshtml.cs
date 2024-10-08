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
    /// <para>������������ �������� ��� ����������� �������� ���������.</para>
    /// </summary>
    [Authorize(Policy = PolicyName.Administrators)]
    public class MimicListModel(EditorManager editorManager) : PageModel
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";

        [BindProperty]
        public string FileName { get; set; }


        public void OnPostOpen()
        {
            OpenResult result = editorManager.OpenMimic(FileName);

            if (result.IsSuccessful)
            {
                // clear file name
                FileName = "";
                ModelState.Remove(nameof(FileName));
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }

        public void OnPostSave(long mimicKey)
        {
            if (!editorManager.SaveMimic(mimicKey, out string errMsg))
                ErrorMessage = errMsg;
        }

        public void OnPostSaveAndClose(long mimicKey)
        {
            if (editorManager.SaveMimic(mimicKey, out string errMsg))
                editorManager.CloseMimic(mimicKey);
            else
                ErrorMessage = errMsg;
        }

        public void OnPostCloseWithoutSaving(long mimicKey)
        {
            editorManager.CloseMimic(mimicKey);
        }
    }
}
