// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// Defines properties of publishing options.
    /// <para>Определяет свойства параметров публикации.</para>
    /// </summary>
    public interface IPublishOptions
    {
        /// <summary>
        /// Gets or sets the root topic used as a prefix for all device topics.
        /// </summary>
        string RootTopic { get; }

        /// <summary>
        /// Gets or sets the payload to send if channel value is undefined.
        /// </summary>
        string UndefinedValue { get; }

        /// <summary>
        /// Gets or sets the format of published channel data.
        /// </summary>
        string PublishFormat { get; }
    }
}
