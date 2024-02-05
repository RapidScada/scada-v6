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
 * Summary  : Provides extensions for database classes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

using System;
using System.Data.Common;
using System.Diagnostics;

namespace Scada.Dbms
{
    /// <summary>
    /// Provides extensions for database classes.
    /// <para>Предоставляет расширения для классов баз данных.</para>
    /// </summary>
    public static class DbExtensions
    {
        /// <summary>
        /// Closes the connection silently.
        /// </summary>
        public static void SilentClose(this DbConnection conn)
        {
            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Rolls back the transaction silently.
        /// </summary>
        public static void SilentRollback(this DbTransaction trans)
        {
            if (trans != null)
            {
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
