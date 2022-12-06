// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Server.Modules.ModActiveDirectory.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModActiveDirectoryView : ModuleView
    {
        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Locale.IsRussian ?
                    "Аутентификация Active Directory" :
                    "Active Directory Authentication";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Модуль проверяет имя и пароль пользователя, используя Active Directory." :
                    "The module validates username and password using Active Directory.";
            }
        }
    }
}
