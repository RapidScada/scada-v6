using System.Reflection;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// The class contains utility methods for the plugin
    /// </summary>
    internal static class PlgUtils
    {
        /// <summary>
        /// 序列化设置
        /// </summary>
        public static readonly SerializationBinder SerializationBinder = 
            new SerializationBinder(Assembly.GetExecutingAssembly());
    }
}