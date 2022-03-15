// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvMqtt.Config
{
    /// <summary>
    /// Specifies the MQTT protocol versions.
    /// <para>Задаёт версии протокола MQTT.</para>
    /// </summary>
    /// <remarks>
    /// Based on https://github.com/dotnet/MQTTnet/blob/master/Source/MQTTnet/Formatter/MqttProtocolVersion.cs
    /// </remarks>
    internal enum ProtocolVersion
    {
        Unknown = 0,
        V310 = 3,
        V311 = 4,
        V500 = 5
    }
}
