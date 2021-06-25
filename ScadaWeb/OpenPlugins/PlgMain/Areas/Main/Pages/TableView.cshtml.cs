using Microsoft.AspNetCore.Mvc.RazorPages;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMain.Areas.Main.Pages
{
    public class TableViewModel : PageModel
    {
        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly IViewLoader viewLoader;

        public bool ViewError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public int ViewID { get; set; }

        public TableViewModel(IWebContext webContext, IUserContext userContext, IViewLoader viewLoader)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            this.viewLoader = viewLoader;
        }

        public void OnGet(int? id)
        {
            ViewID = id ?? userContext.Views.GetFirstViewID() ?? 0;

            if (viewLoader.GetView(ViewID, out TableView view, out string errMsg))
            {
                ErrorMessage = "Loaded OK!";
            }
            else
            {
                ErrorMessage = errMsg;
            }
        }
    }
}
