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
 * Module   : Webstation Application
 * Summary  : Contains the predefined URLs of the web application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Web
{
    /// <summary>
    /// Contains the predefined URLs of the web application.
    /// <para>Содержит предопределенные URL-адреса веб-приложения.</para>
    /// </summary>
    public static class WebUrl
    {
        public const string Root = "/";
        public const string IndexPage = "/Index";
        public const string LoginPage = "/Login";
        public const string LogoutPage = "/Logout";
        public const string DefaultStartPage = "/View";
    }
}
