// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a binding of a component property to a channel.
    /// <para>Представляет привязку свойства компонента к каналу.</para>
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// Gets or sets the name of the component property to which the channel is bound.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the channel number, channel code, or tag code.
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the data member provinding the property value.
        /// </summary>
        public string DataMember { get; set; }
    }
}
