// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a data transfer object containing user credentials.
    /// <para>Представляет объект передачи данных, содержащий учётные данные пользователя.</para>
    /// </summary>
    public class CredentialsDTO
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
