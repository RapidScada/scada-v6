/*
 * Copyright 2021 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommCommon
 * Summary  : Converts entities of the configuration database to Communicator configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using System;

namespace Scada.Comm.Config
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

            DeviceConfig deviceConfig = new DeviceConfig { DeviceNum = deviceEntity.DeviceNum };
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
