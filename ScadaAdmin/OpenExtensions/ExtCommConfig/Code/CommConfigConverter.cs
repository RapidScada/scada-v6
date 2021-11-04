// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// Converts entities of the configuration database to Communicator configuration.
    /// <para>Преобразует сущности базы конфигурации в настройки Коммуникатора.</para>
    /// </summary>
    public static class CommConfigConverter
    {
        /// <summary>
        /// Creates a communication line configuration based on the specified entity.
        /// </summary>
        public static LineConfig CreateLineConfig(CommLine commLineEntity)
        {
            if (commLineEntity == null)
                throw new ArgumentNullException(nameof(commLineEntity));

            return new LineConfig
            {
                CommLineNum = commLineEntity.CommLineNum,
                Name = commLineEntity.Name
            };
        }

        /// <summary>
        /// Creates a device settings configuration on the specified entity.
        /// </summary>
        public static DeviceConfig CreateDeviceConfig(Device deviceEntity, BaseTable<DevType> devTypeTable)
        {
            if (deviceEntity == null)
                throw new ArgumentNullException(nameof(deviceEntity));

            DeviceConfig deviceConfig = new() { DeviceNum = deviceEntity.DeviceNum };
            CopyDeviceProps(deviceEntity, deviceConfig, devTypeTable);
            return deviceConfig;
        }

        /// <summary>
        /// Copies the properties from the device entity to the device configuration.
        /// </summary>
        public static void CopyDeviceProps(Device srcDeviceEntity, DeviceConfig destDeviceConfig,
            BaseTable<DevType> devTypeTable)
        {
            if (srcDeviceEntity == null)
                throw new ArgumentNullException(nameof(srcDeviceEntity));
            if (destDeviceConfig == null)
                throw new ArgumentNullException(nameof(destDeviceConfig));

            destDeviceConfig.Name = srcDeviceEntity.Name;
            destDeviceConfig.Driver = devTypeTable != null && srcDeviceEntity.DevTypeID.HasValue &&
                devTypeTable.Items.TryGetValue(srcDeviceEntity.DevTypeID.Value, out DevType devTypeEntity)
                ? devTypeEntity.Driver 
                : "";
            destDeviceConfig.NumAddress = srcDeviceEntity.NumAddress ?? 0;
            destDeviceConfig.StrAddress = srcDeviceEntity.StrAddress;
        }
    }
}
