// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents a group of mimic diagrams belonging to the same project.
    /// <para>Представляет группу мнемосхем, принадлежащих одному проекту.</para>
    /// </summary>
    internal class MimicGroup
    {
        private readonly Dictionary<string, MimicInstance> mimics;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicGroup()
        {
            mimics = [];
        }


        /// <summary>
        /// Gets the group name.
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Gets a snapshot containing the mimics of the group.
        /// </summary>
        public MimicInstance[] GetMimics()
        {
            lock (mimics)
            {
                return [.. mimics.Values.OrderBy(m => m.FileName)];
            }
        }
    }
}
