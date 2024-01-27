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
 * Summary  : Specifies the format codes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Data.Const
{
    /// <summary>
    /// Specifies the format codes.
    /// <para>Задаёт коды форматов.</para>
    /// </summary>
    public static class FormatCode
    {
        public const string G = nameof(G);
        public const string N0 = nameof(N0);
        public const string N1 = nameof(N1);
        public const string N2 = nameof(N2);
        public const string N3 = nameof(N3);
        public const string N4 = nameof(N4);
        public const string N5 = nameof(N5);
        public const string N6 = nameof(N6);
        public const string X = nameof(X);
        public const string X2 = nameof(X2);
        public const string X4 = nameof(X4);
        public const string X8 = nameof(X8);
        public const string OffOn = nameof(OffOn);
        public const string NoYes = nameof(NoYes);
        public const string Off = nameof(Off);
        public const string On = nameof(On);
        public const string Execute = nameof(Execute);
        public const string NormalError = nameof(NormalError);
        public const string DateTime = nameof(DateTime);
        public const string Date = nameof(Date);
        public const string Time = nameof(Time);
        public const string String = nameof(String);
    }
}
