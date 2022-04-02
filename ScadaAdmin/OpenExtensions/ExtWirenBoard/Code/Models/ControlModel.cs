// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code.Meta;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Models
{
    /// <summary>
    /// Represents a device control .
    /// <para>Представляет элемент устройства.</para>
    /// </summary>
    internal class ControlModel
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ControlModel(string code, string topic)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            Meta = new ControlMeta { Name = code };
        }


        /// <summary>
        /// Gets the device code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the control topic.
        /// </summary>
        public string Topic { get; }

        /// <summary>
        /// Gets the information associated with the control.
        /// </summary>
        public ControlMeta Meta { get; private set; }
        
        
        /// <summary>
        /// Updates the control information.
        /// </summary>
        public void UpdateMeta(ControlMeta controlMeta)
        {
            Meta = controlMeta ?? throw new ArgumentNullException(nameof(controlMeta));
        }
    }
}
