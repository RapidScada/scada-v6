﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents a menu that contains version items.
    /// <para>Представляет меню, которое содержит элементы версий.</para>
    /// </summary>
    public class VersionMenu : List<VersionItem>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public VersionMenu(KnownLang lang)
        {
            Lang = lang;
            Title = lang.ConvertToString();
        }

        /// <summary>
        /// Gets the menu langugage.
        /// </summary>
        public KnownLang Lang { get; }

        /// <summary>
        /// Gets the menu title.
        /// </summary>
        public string Title { get; }


        /// <summary>
        /// English versions of the documentation.
        /// </summary>
        public static readonly VersionMenu En = new(KnownLang.En)
        {
            new VersionItem
            {
                Version = KnownVersion.V62,
                Text = KnownVersion.V62.ConvertToString(),
                Url = "/en/latest/"
            },
            new VersionItem
            {
                Version = KnownVersion.V61,
                Text = KnownVersion.V61.ConvertToString(),
                Url = "/en/6.1/"
            },
            new VersionItem
            {
                Version = KnownVersion.V58,
                Text = KnownVersion.V58.ConvertToString(),
                Url = "/en/5.8/"
            }
        };

        /// <summary>
        /// Spanish versions of the documentation.
        /// </summary>
        public static readonly VersionMenu Es = new(KnownLang.Es)
        {
            new VersionItem
            {
                Version = KnownVersion.V58,
                Text = KnownVersion.V58.ConvertToString(),
                Url = "/es/5.8/"
            }
        };

        /// <summary>
        /// French versions of the documentation.
        /// </summary>
        public static readonly VersionMenu Fr = new(KnownLang.Fr)
        {
            new VersionItem
            {
                Version = KnownVersion.V62,
                Text = KnownVersion.V62.ConvertToString(),
                Url = "/fr/latest/"
            },
            new VersionItem
            {
                Version = KnownVersion.V61,
                Text = KnownVersion.V61.ConvertToString(),
                Url = "/fr/6.1/"
            }
        };

        /// <summary>
        /// Russian versions of the documentation.
        /// </summary>
        public static readonly VersionMenu Ru = new(KnownLang.Ru)
        {
            new VersionItem
            {
                Version = KnownVersion.V62,
                Text = KnownVersion.V62.ConvertToString(),
                Url = "/ru/latest/"
            },
            new VersionItem
            {
                Version = KnownVersion.V61,
                Text = KnownVersion.V61.ConvertToString(),
                Url = "/ru/6.1/"
            },
            new VersionItem
            {
                Version = KnownVersion.V58,
                Text = KnownVersion.V58.ConvertToString(),
                Url = "/ru/5.8/"
            }
        };

        /// <summary>
        /// Chinese versions of the documentation.
        /// </summary>
        public static readonly VersionMenu Zh = new(KnownLang.Zh)
        {
            new VersionItem
            {
                Version = KnownVersion.V60,
                Text = KnownVersion.V60.ConvertToString(),
                Url = "/zh/6.0/"
            }
        };

        /// <summary>
        /// The empty menu.
        /// </summary>
        public static readonly VersionMenu Empty = new(KnownLang.None);

        /// <summary>
        /// The menus for all languages.
        /// </summary>
        public static readonly List<VersionMenu> All = new() 
        {
            // In alphabet order
            En, // English 
            Fr, // French
            Ru, // Russian
            Es, // English
            Zh  // 简体中文
        };


        /// <summary>
        /// Gets the menu according to the specified language.
        /// </summary>
        public static VersionMenu GetMenu(KnownLang lang)
        {
            return lang switch
            {
                KnownLang.En => En,
                KnownLang.Es => Es,
                KnownLang.Fr => Fr,
                KnownLang.Ru => Ru,
                KnownLang.Zh => Zh,
                _ => Empty
            };
        }
    }
}
