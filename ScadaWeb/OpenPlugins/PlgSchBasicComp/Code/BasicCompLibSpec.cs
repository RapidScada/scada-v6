// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Web.Plugins.PlgScheme;

namespace Scada.Web.Plugins.PlgSchBasicComp.Code
{
    /// <summary>
    /// Specification of the basic scheme components library
    /// <para>Спецификация библиотеки основных компонентов схемы</para>
    /// </summary>
    public class BasicCompLibSpec : CompLibSpec
    {
        /// <summary>
        /// Получить префикс XML-элементов
        /// </summary>
        public override string XmlPrefix
        {
            get
            {
                return "basic";
            }
        }

        /// <summary>
        /// Получить пространство имён XML-элементов
        /// </summary>
        public override string XmlNs
        {
            get
            {
                return "urn:rapidscada:scheme:basic";
            }
        }

        /// <summary>
        /// Получить заголовок группы в редакторе
        /// </summary>
        public override string GroupHeader
        {
            get
            {
                return Locale.IsRussian ? "Основные" : "Basic";
            }
        }

        /// <summary>
        /// Получить ссылки на файлы CSS, которые необходимы для работы компонентов
        /// </summary>
        public override List<string> Styles
        {
            get
            {
                return new List<string>()
                {
                    "SchBasicComp/css/basiccomp.min.css"
                };
            }
        }

        /// <summary>
        /// Получить ссылки на файлы JavaScript, которые необходимы для работы компонентов
        /// </summary>
        public override List<string> Scripts
        {
            get
            {
                return new List<string>()
                {
                    "SchBasicComp/js/basiccomp-render.js"
                };
            }
        }


        /// <summary>
        /// Создать элементы списка компонентов
        /// </summary>
        protected override List<CompItem> CreateCompItems()
        {
            return new List<CompItem>()
            {
                new CompItem(null /*Resources.button*/, typeof(Button)),
                new CompItem(null /*Resources.led*/, typeof(Led)),
                new CompItem(null /*Resources.link*/, typeof(Link)),
                new CompItem(null /*Resources.toggle*/, typeof(Toggle))
            };
        }

        /// <summary>
        /// Создать фабрику компонентов
        /// </summary>
        protected override CompFactory CreateCompFactory()
        {
            return new BasicCompFactory();
        }
    }
}