// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Represents arguments of a mimic view.
    /// <para>Представляет аргументы представления мнемосхемы.</para>
    /// </summary>
    public class MimicViewArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicViewArgs(IDictionary<string, string> args)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));
            CnlOffset = args.GetValueAsInt("cnlOffset");
            TitleCompID = args.GetValueAsInt("titleCompID");
        }


        /// <summary>
        /// Gets or sets the channel number offset.
        /// </summary>
        public int CnlOffset { get; set; }

        /// <summary>
        /// Gets or sets the ID of the component that displays a title.
        /// </summary>
        public int TitleCompID { get; set; }
    }
}
