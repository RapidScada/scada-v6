using Scada.Scheme;
using Scada.Web.Plugins.SchVolExtentionsComp;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Basic scheme components plugin specification
    /// </summary>
    public class PlgSchVolExtentionsCompSpec : PluginSpec, ISchemeComp
    {
        /// <summary>
        /// 版本号
        /// </summary>
        internal const string PlgVersion = "5.0.0.0";

        /// <summary>
        /// 名称
        /// </summary>
        public override string Name
        {
            get
            {
                return "Vol extentions Scheme Components";
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Descr
        {
            get
            {
                return "A set of Vol extentions components for display on schemes.";
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public override string Version
        {
            get
            {
                return PlgVersion;
            }
        }

        /// <summary>
        /// 库
        /// </summary>
        CompLibSpec ISchemeComp.CompLibSpec
        {
            get
            {
                return new BasicCompLibSpec();
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            if (SchemeContext.GetInstance().EditorMode)
            {
                
            }
        }
    }
}
