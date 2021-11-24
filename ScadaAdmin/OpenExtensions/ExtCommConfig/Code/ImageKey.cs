// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// Specifies the image keys for the explorer tree.
    /// <para>Задаёт ключи изображения для дерева проводника.</para>
    /// </summary>
    internal static class ImageKey
    {
        private const string ImagePrefix = "comm_config_";
        public const string DataSource = ImagePrefix + "data_source.png";
        public const string Device = ImagePrefix + "device.png";
        public const string Driver = ImagePrefix + "driver.png";
        public const string Options = ImagePrefix + "options.png";
        public const string Line = ImagePrefix + "line.png";
        public const string LineInactive = ImagePrefix + "line_inactive.png";
        public const string Lines = ImagePrefix + "lines.png";
        public const string Stats = ImagePrefix + "stats.png";
    }
}
