/*
 * Copyright 2025 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Provides JSON formatting that uses camel-casing for property names and dictionary keys
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2024
 * Modified : 2024
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Scada.Web.Api
{
    /// <summary>
    /// Provides JSON formatting that uses camel-casing for property names and dictionary keys.
    /// <para>Предоставляет форматирование JSON, использующее camel-casing для имен свойств и ключей словарей.</para>
    /// </summary>
    public class CamelCaseJsonFormatterAttribute : ActionFilterAttribute
    {
        private static readonly SystemTextJsonOutputFormatter Formatter = new(
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            });

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
                objectResult.Formatters.Add(Formatter);
        }
    }
}
