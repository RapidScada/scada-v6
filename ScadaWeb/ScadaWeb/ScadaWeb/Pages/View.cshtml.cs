using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Scada.Web.Pages
{
    public class ViewModel : PageModel
    {
        [BindProperty]
        public string UserInfo { get; set; }

        public void OnGet()
        {
            var sb = new StringBuilder();
            sb.Append("Claims:<br/>");

            foreach (var claim in User.Claims)
            {
                sb.Append(claim.ToString()).Append("<br/>");
            }

            UserInfo = sb.ToString();
        }
    }
}
