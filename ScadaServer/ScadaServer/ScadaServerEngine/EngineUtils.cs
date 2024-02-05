﻿/*
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
 * Module   : ScadaServerEngine
 * Summary  : The class provides helper methods for this library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System.Reflection;

namespace Scada.Server.Engine
{
    /// <summary>
    /// The class provides helper methods for this library.
    /// <para>Класс, предоставляющий вспомогательные методы для данной библиотеки.</para>
    /// </summary>
    internal static class EngineUtils
    {
        /// <summary>
        /// The application version.
        /// </summary>
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
