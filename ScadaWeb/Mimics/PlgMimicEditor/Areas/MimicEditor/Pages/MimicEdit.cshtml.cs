// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Authorization;
using Scada.Web.Plugins.PlgMimicEditor.Code;

namespace Scada.Web.Plugins.PlgMimicEditor.Areas.MimicEditor.Pages
{
    /// <summary>
    /// Represents a page for editing a mimic diagram.
    /// <para>������������ �������� ��� �������������� ����������.</para>
    /// </summary>
    [Authorize(Policy = PolicyName.Administrators)]
    public class MimicEditModel(EditorManager editorManager) : PageModel
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";
        public long MimicKey { get; private set; } = 0;

        public void OnGet(long key)
        {
            MimicKey = key;

            if (editorManager.FindMimic(key, out MimicInstance mimicInstance, out string errMsg))
            {
                ViewData["Title"] = Path.GetFileName(mimicInstance.FileName);
            }
            else
            {
                ViewData["Title"] = key.ToString();
                ErrorMessage = errMsg;
            }
        }
    }
}
