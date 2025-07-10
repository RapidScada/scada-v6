﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents data bindings of a component.
    /// <para>Представляет привязки данных компонента.</para>
    /// </summary>
    public class ComponentBindings
    {
        /// <summary>
        /// Gets or sets the input channel number.
        /// Specifies the default data source for the component.
        /// </summary>
        public int InCnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the output channel number.
        /// By default it is used for sending commands.
        /// </summary>
        public int OutCnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the object number. It is used for binding channels by code.
        /// </summary>
        public int ObjNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the device number. It is used for binding channels by tag code.
        /// </summary>
        public int DeviceNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to check user rights to display the component.
        /// </summary>
        public bool CheckRights { get; set; } = false;

        /// <summary>
        /// Gets the property bindings of the compoment.
        /// </summary>
        public List<PropertyBinding> PropertyBindings { get; } = [];
    }
}
