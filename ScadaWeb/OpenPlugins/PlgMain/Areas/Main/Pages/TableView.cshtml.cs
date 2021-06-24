using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    public class TableViewModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IMemoryCache memoryCache;

        public TableViewModel(IWebContext webContext, IUserContext userContext, IMemoryCache memoryCache)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.memoryCache = memoryCache;
        }

        public void OnGet(int? id)
        {
        }
    }
}
