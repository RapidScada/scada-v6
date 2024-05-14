// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Represents a mimic diagram in runtime mode.
    /// <para>Представляет мнемосхему в режиме выполнения.</para>
    /// </summary>
    public class MimicView : ViewBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicView(View viewEntity)
            : base(viewEntity)
        {
            Mimic = new Mimic();
        }


        /// <summary>
        /// Gets the mimic diagram.
        /// </summary>
        public Mimic Mimic { get; }


        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public override void LoadView(Stream stream)
        {
            Mimic.Load(stream);
        }

        /// <summary>
        /// Binds the view to the configuration database.
        /// </summary>
        public override void Bind(ConfigDataset configDataset)
        {

        }
    }
}
