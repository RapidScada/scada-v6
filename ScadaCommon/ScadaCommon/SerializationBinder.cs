/*
 * Copyright 2024 Rapid Software LLC
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Controls class loading during serialization
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2020
 */

using System;
using System.Reflection;

namespace Scada
{
    /// <summary>
    /// Controls class loading during serialization.
    /// <para>Контролирует загружаемые классы в процессе сериализации.</para>
    /// </summary>
    /// <remarks>The class is required due to some .NET bugs. 
    /// An instance of this class must be created in an assembly where it is used.</remarks>
    public class SerializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        /// <summary>
        /// The assembly where type searches are performed.
        /// </summary>
        protected Assembly assembly;
        /// <summary>
        /// The method to resolve an assembly by name.
        /// </summary>
        protected Func<AssemblyName, Assembly> assemblyResolver;
        /// <summary>
        /// The method to resolve a type by name.
        /// </summary>
        protected Func<Assembly, string, bool, Type> typeResolver;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerializationBinder()
            : base()
        {
            assembly = Assembly.GetExecutingAssembly();
            InitResolvers();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerializationBinder(Assembly assembly)
            : base()
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            InitResolvers();
        }


        /// <summary>
        /// Initializes the methods that resolve assemblies and types.
        /// </summary>
        protected void InitResolvers()
        {
            assemblyResolver = (AssemblyName asmName) =>
            {
                return string.Equals(asmName.FullName, assembly.FullName, StringComparison.Ordinal) ?
                    assembly :
                    Assembly.Load(asmName);
            };

            typeResolver = (Assembly asm, string typeName, bool ignoreCase) =>
            {
                return asm.GetType(typeName, false, ignoreCase);
            };
        }

        /// <summary>
        /// Controls type binding.
        /// </summary>
        public override Type BindToType(string assemblyName, string typeName)
        {
            return string.Equals(assemblyName, assembly.FullName, StringComparison.Ordinal) ?
                assembly.GetType(typeName, true, false) :
                Type.GetType(string.Format("{0}, {1}", typeName, assemblyName),
                    assemblyResolver, typeResolver, true, false);
        }
    }
}
