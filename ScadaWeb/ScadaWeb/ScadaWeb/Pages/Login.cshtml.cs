using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Scada.Web.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [StringLength(5)]
        public string Username { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Debug.WriteLine("!!! Username = " + Username);
            return RedirectToPage("/Privacy");
        }
    }
}
