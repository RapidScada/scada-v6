// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Model
{
    /// <summary>
    /// Represents a method that will handle an event raised when a scheme item is changed
    /// <para>Представляет метод для обработки события, возникающего при изменении элемента схемы</para>
    /// </summary>
    public delegate void ItemChangedEventHandler(
        object sender,
        SchemeChangeTypes changeType,
        object changedObject,
        object oldKey);
}
