// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvSimulator
{
    /// <summary>
    /// Specifies the tag codes of a simulator device.
    /// <para>Задаёт коды тегов симулятора устройства.</para>
    /// </summary>
    internal static class TagCode
    {
        public const string Sin = nameof(Sin);
        public const string Sqr = nameof(Sqr);
        public const string Tri = nameof(Tri);
        public const string DO = nameof(DO);
        public const string AO = nameof(AO);
        public const string RA = nameof(RA);
    }
}
