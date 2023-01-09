/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : Builds database connection strings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Lang;
using System;

namespace Scada.Dbms
{
    /// <summary>
    /// Builds database connection strings.
    /// <para>Формирует строки подключения к базам данных.</para>
    /// </summary>
    public static class ConnectionStringBuilder
    {
        /// <summary>
        /// Gets the default TCP port depending on the DBMS.
        /// </summary>
        private static int GetDefaultPort(KnownDBMS knownDBMS)
        {
            switch (knownDBMS)
            {
                case KnownDBMS.PostgreSQL:
                    return 5432;

                case KnownDBMS.MySQL:
                    return 3306;

                case KnownDBMS.MSSQL:
                    return 1433;

                case KnownDBMS.Oracle:
                    return 1521;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Builds a connection string.
        /// </summary>
        public static string Build(DbConnectionOptions options, bool hidePassword)
        {
            if (options == null)
                return "";

            string password = hidePassword ? CommonPhrases.HiddenPassword : options.Password;

            switch (options.KnownDBMS)
            {
                case KnownDBMS.PostgreSQL:
                    ScadaUtils.RetrieveHostAndPort(options.Server, GetDefaultPort(options.KnownDBMS),
                        out string host, out int port);
                    return string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}",
                        host, port, options.Database, options.Username, password);

                case KnownDBMS.MySQL:
                    return string.Format("server={0};uid={1};pwd={2};database={3}",
                        options.Server, options.Username, password, options.Database);

                case KnownDBMS.MSSQL:
                    return string.Format("Server={0};Database={1};User ID={2};Password={3}",
                        options.Server, options.Database, options.Username, password);

                case KnownDBMS.Oracle:
                    return string.Format("User Id={0};Password={1};Data Source=//{2}/{3}",
                        options.Username, password, options.Server, options.Database);

                default:
                    return "";
            }
        }
    }
}
