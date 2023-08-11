// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Plugins.PlgScheme.Editor.Code;

namespace Scada.Web.Plugins.PlgScheme.Editor.Pages
{
    /// <summary>
    /// Represents a main page.
    /// <para>Представляет главную страницу.</para>
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly EditorContext _editorContext;


        public IndexModel(EditorContext editorContext)
        {
            _editorContext = editorContext;
        }


        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; private set; } = "";

        [BindProperty]
        public string FileName { get; set; }


        public void OnGet()
        {
        }

        public void OnPost()
        {
            OpenResult result = _editorContext.Manager.OpenScheme(FileName);
            ErrorMessage = result.ErrorMessage;
        }
    }
}
