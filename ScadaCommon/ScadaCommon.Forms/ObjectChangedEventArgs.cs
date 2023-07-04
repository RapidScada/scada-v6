// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Forms
{
    /// <summary>
    /// Provides data for events caused by an object change.
    /// <para>Предоставляет данные для событий, вызванных изменением объекта.</para>
    /// </summary>
    public class ObjectChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjectChangedEventArgs(object changedObject)
            : this(changedObject, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjectChangedEventArgs(object changedObject, object changeArgument)
        {
            ChangedObject = changedObject;
            ChangeArgument = changeArgument;
        }


        /// <summary>
        /// Gets the changed object.
        /// </summary>
        public object ChangedObject { get; }

        /// <summary>
        /// Get the argument describing the changes.
        /// </summary>
        public object ChangeArgument { get; }
    }
}
