using Scada.Data.Entities;
using Scada.Web.Services;
using Scada.Web.TreeView;
using Scada.Web.Users;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain
{
    public class PlgMainLogic : PluginLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgMainLogic(IWebContext webContext)
            : base(webContext)
        {
        }

        public override string Code => "PlgMain";

        public override List<MenuItem> GetUserMenuItems(User user, UserRights userRights)
        {
            return new List<MenuItem>()
            {
                new MenuItem { Text = "My Page", Url = "/Main/MyPage" }
            };
        }
    }
}
