// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimicEditor.Code;

namespace Scada.Web.Plugins.PlgMimicEditor.Models
{
    /// <summary>
    /// Represents a data transfer object containing mimic changes.
    /// <para>Представляет объект передачи данных, содержащий изменения мнемосхемы.</para>
    /// </summary>
    public class UpdateDto
    {
        public long MimicKey { get; set; }

        public Change[] Changes { get; set; }
    }
}
