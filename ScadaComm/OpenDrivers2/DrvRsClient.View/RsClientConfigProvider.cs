// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;

namespace Scada.Comm.Drivers.DrvRsClient.View
{
    /// <summary>
    /// Represents an intermediary between a driver configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией драйвера и формой конфигурации.</para>
    /// </summary>
    internal class RsClientConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Specifies the image keys for the configuration tree.
        /// </summary>
        private static class ImageKey
        {
            public const string FolderClosed = "folder_closed.png";
            public const string FolderClosedInactive = "folder_closed_inactive.png";
            public const string FolderOpen = "folder_open.png";
            public const string FolderOpenInactive = "folder_open_inactive.png";
            public const string Item = "item.png";
        }

        /// <summary>
        /// Specifies the button tags.
        /// </summary>
        private static class ButtonTag
        {
            public const string AddItemGroup = nameof(AddItemGroup);
            public const string AddItem = nameof(AddItem);
            public const string LineConfig = nameof(LineConfig);
            public const string FillChannelNames = nameof(FillChannelNames);
        }
    }
}
