// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a binding of a component property to a channel.
    /// <para>Представляет привязку свойства компонента к каналу.</para>
    /// </summary>
    public class PropertyBinding
    {
        /// <summary>
        /// Gets or sets the name of the target property to which the data source is bound.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the data source, which is a channel number, channel code, or tag code.
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets the data member that provides the property value.
        /// </summary>
        public string DataMember { get; set; }

        /// <summary>
        /// Gets or sets the format that determines how a value is to be displayed.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the channel number that corresponds to the data source.
        /// </summary>
        public int CnlNum { get; set; }
    }
}
