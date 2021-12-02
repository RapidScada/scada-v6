// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// The class for transfer scheme components
    /// <para>Класс для передачи компонентов схемы</para>
    /// </summary>
    public class ComponentPacket
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ComponentPacket(int capacity)
        {
            EndOfComponents = false;
            Components = new List<object>(capacity);
        }


        /// <summary>
        /// Получить или установить признак, что считаны все компоненты схемы
        /// </summary>
        public bool EndOfComponents { get; set; }

        /// <summary>
        /// Получить компоненты схемы
        /// </summary>
        /// <remarks>Use the Object type to transfer the complete hierarchy of component properties.</remarks>
        public List<object> Components { get; protected set; }


        /// <summary>
        /// Копировать заданные компоненты в объект для передачи данных
        /// </summary>
        public void CopyComponents(IList<BaseComponent> srcComponents, int startIndex, int count)
        {
            int srcCnt = srcComponents.Count;
            EndOfComponents = startIndex + count >= srcCnt;

            for (int i = startIndex, j = 0; i < srcCnt && j < count; i++, j++)
                Components.Add(srcComponents[i]);
        }
    }
}
