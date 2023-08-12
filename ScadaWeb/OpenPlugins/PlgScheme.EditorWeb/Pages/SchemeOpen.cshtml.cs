// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgScheme.Editor.Code;

namespace Scada.Web.Plugins.PlgScheme.Editor.Pages
{
    /// <summary>
    /// Represents a page for opening a scheme.
    /// <para>Представляет страницу для открытия схемы.</para>
    /// </summary>
    public class SchemeOpenModel : PageModel
    {
        private readonly EditorContext _editorContext;


        public SchemeOpenModel(EditorContext editorContext)
        {
            _editorContext = editorContext;
        }


        public string ErrorMessage { get; private set; } = "";
        public string FileName { get; private set; } = "";


        public IActionResult OnGet(string fileName)
        {
            FileName = fileName;
            OpenResult result = _editorContext.Manager.OpenScheme(fileName);

            if (result.IsSuccessful)
            {
                return LocalRedirect("~/SchemeEditor/" + result.EditorID);
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                return Page();
            }
        }
    }
}
