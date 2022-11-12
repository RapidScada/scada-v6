// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents a menu that contains version items.
    /// <para>Представляет меню, которое содержит элементы версий.</para>
    /// </summary>
    public class VersionMenu : List<MenuItem>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public VersionMenu(string title = "")
        {
            Title = title;
        }

        /// <summary>
        /// Gets the menu title.
        /// </summary>
        public string Title { get; }


        /// <summary>
        /// English versions of the documentation.
        /// </summary>
        public static readonly VersionMenu En = new("English")
        {
            new MenuItem
            {
                Text = KnownVersion.V60.ConvertToString(),
                Url = "/en/6.0/"
            },
            new MenuItem
            {
                Text = KnownVersion.V58.ConvertToString(),
                Url = "/en/5.8/"
            }
        };

        /// <summary>
        /// Russian versions of the documentation.
        /// </summary>
        public static readonly VersionMenu Ru = new("Russian")
        {
            new MenuItem
            {
                Text = KnownVersion.V60.ConvertToString(),
                Url = "/ru/6.0/"
            },
            new MenuItem
            {
                Text = KnownVersion.V58.ConvertToString(),
                Url = "/ru/5.8/"
            }
        };

        /// <summary>
        /// Spanish versions of the documentation.
        /// </summary>
        public static readonly VersionMenu Es = new("Spanish")
        {
            new MenuItem
            {
                Text = KnownVersion.V58.ConvertToString(),
                Url = "/es/5.8/"
            }
        };

        /// <summary>
        /// The empty menu.
        /// </summary>
        public static readonly VersionMenu Empty = new();

        /// <summary>
        /// The menus for all languages.
        /// </summary>
        public static readonly List<VersionMenu> All = new() { En, Ru, Es };


        /// <summary>
        /// Gets the menu according to the specified language.
        /// </summary>
        public static VersionMenu GetMenu(KnownLang lang)
        {
            return lang switch
            {
                KnownLang.En => En,
                KnownLang.Ru => Ru,
                KnownLang.Es => Es,
                _ => Empty
            };
        }
    }
}
