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
 * Module   : ScadaServerEngine
 * Summary  : Represents access rights structured by roles and objects
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Data.Const;
using Scada.Data.Models;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents access rights structured by roles and objects.
    /// <para>Представляет права доступа, структурированные по ролям и объектам.</para>
    /// </summary>
    internal class ServerRightMatrix : RightMatrix
    {
        /// <summary>
        /// Gets the rights of the role on the object.
        /// </summary>
        public override Right GetRight(int roleID, int objID)
        {
            switch (roleID)
            {
                case RoleID.Disabled:
                    return Right.Empty;

                case RoleID.Administrator:
                    return Right.Full;

                case RoleID.Dispatcher:
                    return Right.Full;

                case RoleID.Guest:
                    return new Right(true, false);

                case RoleID.Application:
                    return Right.Full;

                default:
                    return base.GetRight(roleID, objID);
            }
        }
    }
}
