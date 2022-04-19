// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvEmail
{
    /// <summary>
    /// Specifies the display names of the device tags.
    /// <para>Задаёт отображаемые наименования тегов устройства.</para>
    /// </summary>
    internal static class TagName
    {
        public static string Mail { get; }
        public static string MailAttach { get; }

        static TagName()
        {
            if (Locale.IsRussian)
            {
                Mail = "Письма";
                MailAttach = "Письма с вложениями";
            }
            else
            {
                Mail = "Mails";
                MailAttach = "Mails with attachments";
            }
        }
    }
}
