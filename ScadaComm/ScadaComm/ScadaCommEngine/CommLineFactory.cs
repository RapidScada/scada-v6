/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Creates communication lines
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Lang;
using System;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Creates communication lines.
    /// <para>Создает линии связи.</para>
    /// </summary>
    internal static class CommLineFactory
    {
        /// <summary>
        /// Gets a new communication line including communication channel and devices.
        /// </summary>
        public static bool GetCommLine(LineConfig lineConfig, CoreLogic coreLogic, DriverHolder driverHolder, 
            out CommLine commLine, out string errMsg)
        {
            if (lineConfig == null)
                throw new ArgumentNullException(nameof(lineConfig));
            if (coreLogic == null)
                throw new ArgumentNullException(nameof(coreLogic));
            if (driverHolder == null)
                throw new ArgumentNullException(nameof(driverHolder));

            try
            {
                // create communication line
                commLine = new CommLine(lineConfig, coreLogic);

                // create communication channel
                if (string.IsNullOrEmpty(lineConfig.Channel.Driver))
                {
                    commLine.Channel = new ChannelLogicStub(commLine, lineConfig.Channel);
                }
                else if (driverHolder.GetDriver(lineConfig.Channel.Driver, out DriverWrapper driverWrapper))
                {
                    if (driverWrapper.CreateChannel(commLine, lineConfig.Channel, out ChannelLogic channelLogic))
                    {
                        commLine.Channel = channelLogic;
                    }
                    else
                    {
                        errMsg = string.Format(Locale.IsRussian ?
                            "Не удалось создать канал связи." :
                            "Unable to create communication channel.");
                        return false;
                    }
                }
                else
                {
                    errMsg = string.Format(Locale.IsRussian ?
                        "Драйвер канала связи {0} не найден." :
                        "Communication channel driver {0} not found.", lineConfig.Channel.Driver);
                    return false;
                }

                // create devices
                foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
                {
                    if (deviceConfig.Active && !coreLogic.DeviceExists(deviceConfig.DeviceNum))
                    {
                        if (driverHolder.GetDriver(deviceConfig.Driver, out DriverWrapper driverWrapper))
                        {
                            if (driverWrapper.CreateDevice(commLine, deviceConfig, out DeviceLogic deviceLogic))
                            {
                                commLine.AddDevice(deviceLogic);
                            }
                            else
                            {
                                errMsg = string.Format(Locale.IsRussian ?
                                    "Не удалось создать устройство {0}." :
                                    "Unable to create device {0}.", deviceConfig.Title);
                                return false;
                            }
                        }
                        else
                        {
                            errMsg = string.Format(Locale.IsRussian ?
                                "Драйвер {0} для устройства {1} не найден." :
                                "Driver {0} for device {1} not found.",
                                deviceConfig.Driver, deviceConfig.Title);
                            return false;
                        }
                    }
                }

                // prepare channel after adding devices
                commLine.Channel.MakeReady();

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                commLine = null;
                errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка при создании линии связи {0}: {1}" :
                    "Error creating communication line {0}: {1}", lineConfig.Title, ex.Message);
                return false;
            }
        }
    }
}
