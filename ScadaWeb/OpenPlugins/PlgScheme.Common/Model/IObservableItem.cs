// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Specifies items whose changes are observed
    /// <para>Определяет элементы, изменения которых отслеживаются</para>
    /// </summary>
    public interface IObservableItem
    {
        /// <summary>
        /// Вызвать событие ItemChanged
        /// </summary>
        void OnItemChanged(SchemeChangeTypes changeType, object changedObject, object oldKey = null);

        /// <summary>
        /// Событие возникающее при изменении элемента
        /// </summary>
        event ItemChangedEventHandler ItemChanged;
    }
}
