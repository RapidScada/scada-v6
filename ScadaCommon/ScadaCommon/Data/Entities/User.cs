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
 * Summary  : Represents a user as an entity of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents a user as an entity of the configuration database.
    /// <para>Представляет пользователя как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class User
    {
        public int UserID { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public int RoleID { get; set; }

        public string Descr { get; set; }


        /* 新增字段 */

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserRealName { get; set; }
        /// <summary>
        /// 性别 0女 1男
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否启用用户密码
        /// </summary>
        public bool UserPwdEnabled { get; set; }
        /// <summary>
        /// 是否启用2fa
        /// </summary>
        public bool FaEnabled { get; set; }
        /// <summary>
        /// 是否启用GoogleOauth
        /// </summary>
        public bool GoogleEnabled { get; set; }

        /// <summary>
        /// 2faSecret
        /// </summary>
        public string FaSecret { get; set; }
        /// <summary>
        /// 2fa认证标识成功
        /// </summary>
        public bool FaVerifySuccess { get; set; }

        // -----密码策略-------
        /// <summary>
        /// 密码定期修改
        /// </summary>
        public bool PwdPeriodModify { get; set; }
        /// <summary>
        /// 密码更新周期限制
        /// </summary>
        public int PwdPeriodLimit { get; set; }
        /// <summary>
        /// 密码长度限值
        /// </summary>
        public int PwdLenLimit { get; set; }
        /// <summary>
        /// 密码复杂度要求
        /// </summary>
        public bool PwdComplicatedRequire { get; set; }
        /// <summary>
        /// 密码复杂度设置 数字num 字母letter 特殊符号symbol
        /// </summary>
        public string PwdComplicatedFormat { get; set; }
        /// <summary>
        /// 密码要求与过去不同
        /// </summary>
        public bool PwdUsedDifferent { get; set; }
        /// <summary>
        /// 旧密码次数
        /// </summary>
        public int PwdUsedTimes { get; set; }
        /// <summary>
        /// 密码更新时间
        /// </summary>
        public DateTime PwdUpdateTime { get; set; }
    }
}
