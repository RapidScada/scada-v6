// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// Specifies the contextual information about a scheme.
    /// <para>Определяет контекстную информацию о схеме.</para>
    /// </summary>
    public sealed class SchemeContext
    {
        /// <summary>
        /// The current scheme context.
        /// </summary>
        private static readonly SchemeContext instance;


        /// <summary>
        /// Initializes the static data of the class.
        /// </summary>
        static SchemeContext()
        {
            instance = new SchemeContext();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private SchemeContext()
        {
            EditorMode = GetEditorMode();
            AppDirs = null;
        }


        /// <summary>
        /// Gets a value indicating that the running application is Scheme Editor.
        /// </summary>
        public bool EditorMode { get; private set; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; private set; }


        /// <summary>
        /// Determines whether the editor mode is running.
        /// </summary>
        private static bool GetEditorMode()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            return asm != null && asm.GetName().Name == "ScadaSchemeEditor";
        }

        /// <summary>
        /// Initializes the scheme context.
        /// </summary>
        public void Init(AppDirs appDirs)
        {
            AppDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
        }

        /// <summary>
        /// Gets the current scheme context.
        /// </summary>
        public static SchemeContext GetInstance()
        {
            return instance;
        }
    }
}
