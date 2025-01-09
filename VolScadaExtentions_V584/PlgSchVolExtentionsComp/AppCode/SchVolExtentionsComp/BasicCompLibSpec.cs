using Scada.Scheme;
using Scada.Web.Properties;
using System.Collections.Generic;

namespace Scada.Web.Plugins.SchVolExtentionsComp
{
    /// <summary>
    /// Specification of the basic scheme components library
    /// </summary>
    public class BasicCompLibSpec : CompLibSpec
    {
        /// <summary>
        /// xml前缀
        /// </summary>
        public override string XmlPrefix
        {
            get
            {
                return "vol";
            }
        }

        /// <summary>
        /// xml命名空间
        /// </summary>
        public override string XmlNs
        {
            get
            {
                return "urn:rapidscada:scheme:vol";
            }
        }

        /// <summary>
        /// 组标题
        /// </summary>
        public override string GroupHeader
        {
            get
            {
                return "Vol Exts";
            }
        }

        /// <summary>
        /// 样式表
        /// </summary>
        public override List<string> Styles
        {
            get
            {
                return new List<string>()
                {
                    "SchVolExtentionsComp/css/volcomp.css"
                };
            }
        }

        /// <summary>
        /// js库
        /// </summary>
        public override List<string> Scripts
        {
            get
            {
                return new List<string>()
                {
                    "SchVolExtentionsComp/js/volcomprender.js"
                };
            }
        }


        /// <summary>
        /// 创建选择列表
        /// </summary>
        protected override List<CompItem> CreateCompItems()
        {
            return new List<CompItem>()
            {
                new CompItem(Resources.comp_st, typeof(BorderText))
            };
        }

        /// <summary>
        /// 创建组件工厂
        /// </summary>
        protected override CompFactory CreateCompFactory()
        {
            return new BasicCompFactory();
        }
    }
}