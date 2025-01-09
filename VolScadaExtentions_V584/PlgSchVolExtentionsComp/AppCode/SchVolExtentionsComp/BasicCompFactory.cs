using Scada.Scheme;
using Scada.Scheme.Model;

namespace Scada.Web.Plugins.SchVolExtentionsComp
{
    /// <summary>
    /// Factory for creating vol scheme components.
    /// </summary>
    public class BasicCompFactory : CompFactory
    {
        /// <summary>
        /// 创建组件
        /// </summary>
        public override BaseComponent CreateComponent(string typeName, bool nameIsShort)
        {
            if (NameEquals("BorderText", typeof(BorderText).FullName, typeName, nameIsShort))
                return new BorderText();
            else
                return null;
        }
    }
}
