// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Defines functionality for containers. Containers are objects that contain zero or more components.
    /// <para>
    /// Определяет функциональность контейнеров.
    /// Контейнеры - это объекты, которые содержат ноль или более компонентов.
    /// </para>
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Gets the top-level components in the container.
        /// </summary>
        public List<Component> Components { get; }
    }
}
