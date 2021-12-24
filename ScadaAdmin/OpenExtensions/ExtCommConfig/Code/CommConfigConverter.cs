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
    /// <para>Преобразует сущности базы конфигурации в конфигурацию Коммуникатора.</para>
    /// </summary>
    public static class CommConfigConverter
    {
        /// <summary>
        /// Gets a driver name from the device type table.
        /// </summary>
        private static string GetDriver(int? devTypeID, BaseTable<DevType> devTypeTable)
        {
            return devTypeTable != null && devTypeID.HasValue &&
                devTypeTable.GetItem(devTypeID.Value) is DevType devTypeEntity
                ? devTypeEntity.Driver ?? ""
                : "";
        }

        /// <summary>
        /// Creates a communication line configuration based on the specified entity.
        /// </summary>
        public static LineConfig CreateLineConfig(CommLine commLineEntity)
        {
            ArgumentNullException.ThrowIfNull(commLineEntity, nameof(commLineEntity));

            return new LineConfig
            {
                CommLineNum = commLineEntity.CommLineNum,
                Name = commLineEntity.Name ?? ""
            };
        }

        /// <summary>
        /// Creates a device settings configuration on the specified entity.
        /// </summary>
        public static DeviceConfig CreateDeviceConfig(Device deviceEntity, BaseTable<DevType> devTypeTable)
        {
            ArgumentNullException.ThrowIfNull(deviceEntity, nameof(deviceEntity));
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
            ArgumentNullException.ThrowIfNull(srcDeviceEntity, nameof(srcDeviceEntity));
            ArgumentNullException.ThrowIfNull(destDeviceConfig, nameof(destDeviceConfig));

            destDeviceConfig.Name = srcDeviceEntity.Name ?? "";
            destDeviceConfig.Driver = GetDriver(srcDeviceEntity.DevTypeID, devTypeTable);
            destDeviceConfig.NumAddress = srcDeviceEntity.NumAddress ?? 0;
            destDeviceConfig.StrAddress = srcDeviceEntity.StrAddress ?? "";
        }
    }
}
