// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a package containing scheme components.
    /// <para>Представляет пакет, содержащий компоненты схемы.</para>
    /// </summary>
    public class ComponentPacket : SchemePacket
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ComponentPacket(long viewStamp, int capacity)
        {
            ViewStamp = viewStamp.ToString();
            EndOfComponents = false;
            Components = new List<object>(capacity);
        }


        /// <summary>
        /// Получить признак, что считаны все компоненты схемы.
        /// </summary>
        public bool EndOfComponents { get; private set; }

        /// <summary>
        /// Получить компоненты схемы.
        /// </summary>
        /// <remarks>Use the Object type to transfer the complete hierarchy of component properties.</remarks>
        public List<object> Components { get; }


        /// <summary>
        /// Копировать заданные компоненты в объект для передачи данных.
        /// </summary>
        public void CopyComponents(IList<ComponentBase> srcComponents, int startIndex, int count)
        {
            int srcCnt = srcComponents.Count;
            EndOfComponents = startIndex + count >= srcCnt;

            for (int i = startIndex, j = 0; i < srcCnt && j < count; i++, j++)
            {
                Components.Add(srcComponents[i]);
            }
        }
    }
}
