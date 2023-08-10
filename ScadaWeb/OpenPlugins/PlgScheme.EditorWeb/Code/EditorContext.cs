// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Log;

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Contains application level data.
    /// <para>Содержит данные уровня приложения.</para>
    /// </summary>
    public class EditorContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EditorContext()
        {
            AppDirs = new AppDirs();
            Log = LogStub.Instance;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; private set; }


        /// <summary>
        /// Initializes the context.
        /// </summary>
        public void Init()
        {

        }
    }
}
