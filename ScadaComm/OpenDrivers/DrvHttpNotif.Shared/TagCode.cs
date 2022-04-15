// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvHttpNotif
{
    /// <summary>
    /// Specifies the device tag codes.
    /// <para>Задаёт коды тегов устройства.</para>
    /// </summary>
    internal static class TagCode
    {
        public const string Notif = nameof(Notif);
        public const string Request = nameof(Request);
        public const string Response = nameof(Response);
    }
}
