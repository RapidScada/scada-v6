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
 * Module   : ScadaCommon
 * Summary  : Specifies the quantity codes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Data.Const
{
    /// <summary>
    /// Specifies the quantity codes.
    /// <para>Задаёт коды величин.</para>
    /// </summary>
    public static class QuantityCode
    {
        public const string Length = "l";
        public const string Mass = "m";
        public const string Time = "t";
        public const string Temperature = "T";
        public const string Velocity = "v";
        public const string Acceleration = "a";
        public const string RotationalVelocity = "rv";
        public const string State = "State";
        public const string RelayState = "RelayState";
        public const string SecurityAlarmState = "SecState";
        public const string FireAlarmState = "FireState";
        public const string ConnectionState = "ConnState";
        public const string Voltage = "V";
        public const string Current = "I";
        public const string Power = "Power";
        public const string ActivePower = "P";
        public const string ReactivePower = "Q";
        public const string ComplexPower = "S";
        public const string ApparentPower = "|S|";
        public const string EnergyConsumption = "E";
        public const string ActivePowerConsumption = "Ea";
        public const string ReactivePowerConsumption = "Er";
        public const string Frequency = "f";
        public const string PowerFactor = "cos";
        public const string Pressure = "p";
        public const string Volume = "Vol";
        public const string Density = "Density";
        public const string VolumetricFlowRate = "FlowRate";
        public const string Heat = "Heat";
        public const string Humidity = "Hum";
        public const string Concentration = "C";
    }
}
