// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Log;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Manages editor instances.
    /// <para>Управляет экземплярами редактора.</para>
    /// </summary>
    public class EditorManager
    {
        //private readonly EditorConfig appConfig; // the application configuration
        private readonly ILog log;               // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EditorManager(/*EditorConfig appConfig,*/ ILog log)
        {
            //this.appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        /// <summary>
        /// Opens a mimic diagram from the specified file.
        /// </summary>
        public OpenResult OpenMimic(string fileName)
        {
            return new OpenResult
            {
                IsSuccessful = false,
                ErrorMessage = "Not implemented"
            };
        }
    }
}
