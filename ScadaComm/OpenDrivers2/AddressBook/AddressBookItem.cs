// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;

namespace Scada.AB
{
    /// <summary>
    /// Represents the base class for an address book item.
    /// <para>Представляет базовый класс для элемента справочника.</para>
    /// </summary>
    public abstract class AddressBookItem : IComparable<AddressBookItem>, ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AddressBookItem()
        {
            Value = "";
            Parent = null;
            Children = null;
        }


        /// <summary>
        /// Gets the sort order of the item type.
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// Gets or sets the item value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        public ITreeNode Parent { get; set; }

        /// <summary>
        ///  Gets the child tree nodes.
        /// </summary>
        public IList Children { get; protected set; }


        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(AddressBookItem other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                int comp = Order.CompareTo(other.Order);
                return comp == 0 ? Value.CompareTo(other.Value) : comp;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}
