// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Specifies the valid names of the XML root element of a mimic.
    /// <para>Задаёт допустимые имена корневого XML-элемента мнемосхемы.</para>
    /// </summary>
    public static class RootElement
    {
        public const string Mimic = nameof(Mimic);
        public const string Faceplate = nameof(Faceplate);
    }
}
