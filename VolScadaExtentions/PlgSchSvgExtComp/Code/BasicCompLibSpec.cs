using Scada.Web.Plugins.PlgScheme;
using Scada.Web.Services;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Specification of the basic scheme components library
    /// </summary>
    public class BasicCompLibSpec : CompLibSpec
    {
        /// <summary>
        /// 插件版本号
        /// </summary>
        public const string PlgVersion = "6.0.0.1";

        private readonly IWebContext webContext;
        private string dynamicSvgPath;

        public BasicCompLibSpec(IWebContext webContext)
        {
            this.webContext = webContext;

            //AppData.Init();
            //dynamicSvgPath = Path.Combine(AppData.AppDirs.WebAppDir, "plugins\\SchSvgExtComp\\DynamicSvg");
            //if (!Directory.Exists(dynamicSvgPath)) Directory.CreateDirectory(dynamicSvgPath);
        }

        /// <summary>
        /// xml前缀
        /// </summary>
        public override string XmlPrefix
        {
            get
            {
                return "svgext";
            }
        }

        /// <summary>
        /// 节点
        /// </summary>
        public override string XmlNs
        {
            get
            {
                return "urn:rapidscada:scheme:svgext";
            }
        }

        /// <summary>
        /// 表头
        /// </summary>
        public override string GroupHeader
        {
            get
            {
                return "SvgExt";
            }
        }

        /// <summary>
        /// CSS
        /// </summary>
        public override List<string> Styles
        {
            get
            {
                return new List<string>()
                {
                    "SchSvgExtComp/css/svgextcomp.min.css?v="+UrlVersion(),
                    "SchSvgExtComp/css/font_1331132_5lvbai88wkb.css",
                    "SchSvgExtComp/css/font_1331132_g7tv7fmj6c9.css",
                    "SchSvgExtComp/css/font_2030495_qjucevcilen.css",
                    "SchSvgExtComp/css/font_2052340_9oz9rmhcdbr.css",
                    "SchSvgExtComp/css/font_2073009_5ilnjxypv6l.css",
                    "SchSvgExtComp/css/font_2395018_gox2nhzd3ca.css",
                    "SchSvgExtComp/lib/swiper/swiper.min.css?v=4.5.0",
                };
            }
        }

        /// <summary>
        /// JS
        /// </summary>
        public override List<string> Scripts
        {
            get
            {
                var jsList = new List<string>()
                {
                    "SchSvgExtComp/lib/d3.min.js",
                    "SchSvgExtComp/lib/swiper/swiper.min.js?v=4.5.0",
                    "SchSvgExtComp/js/svgextcomprender.js",
                    "SchSvgExtComp/js/svgextWindmillcomprender.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/WindmillUtil.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/svgextTurbinecomprender.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/TurbineUtil.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/svgextPipecomprender.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/PipeUtil.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/svgextWaterLevelcomprender.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/WaterLevelUtil.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/textScrollComprender.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/SignalLightUtil.js?v="+UrlVersion(),
                    "SchSvgExtComp/js/svgextConveyerBeltcomprender.js?v="+UrlVersion(),
                };
                return jsList;
            }
        }


        /// <summary>
        /// 组件列表
        /// </summary>
        protected override List<CompItem> CreateCompItems()
        {
            var compList = new List<CompItem>
            {
                new CompItem(null, "管路", typeof(BasePipe)),
                new CompItem(null, "液位", typeof(WaterLevel)),
                new CompItem(null, "涡轮", typeof(Turbine)),
                new CompItem(null, "文字轮播", typeof(TextScroll)),
                new CompItem(null, "文字映射", typeof(TextReflect)),
                new CompItem(null, "信号灯", typeof(SignalLight))
            };

            return compList;
        }

        /// <summary>
        /// 创建组件工厂
        /// </summary>
        protected override CompFactory CreateCompFactory()
        {
            return new BasicCompFactory();
        }

        private string UrlVersion()
        {
            return BasicCompLibSpec.PlgVersion;
        }
        public override bool Validate(out string errMsg)
        {
            if (!base.Validate(out errMsg))
            {
                return false;
            }
            return true;
            //var authRes = KeyGenUtil.GetInfo(Path.Combine(AppData.AppDirs.WebAppDir, "config"), "", new SvgExtAppReg(), out string regKey, out string compCode, out string statusMsg);
            //return authRes == "Valid";
        }
    }
}