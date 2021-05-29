using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Scada.Web.Pages
{
    public class UserProfileModel : PageModel
    {
        [BindProperty(Name = "id", SupportsGet = true)]
        public int? UserID { get; set; }

        public void OnGet(/*int? id*/)
        {         
        }
    }
}
