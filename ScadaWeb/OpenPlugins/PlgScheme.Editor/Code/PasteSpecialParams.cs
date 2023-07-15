// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Represents special paste parameters.
    /// <para>Представляет параметры специальной вставки.</para>
    /// </summary>
    public class PasteSpecialParams
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PasteSpecialParams()
        {
            InCnlOffset = 0;
            CtrlCnlOffset = 0;
        }


        /// <summary>
        /// Gets or sets the offset of input channel numbers.
        /// </summary>
        public int InCnlOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset of output channel numbers.
        /// </summary>
        public int CtrlCnlOffset { get; set; }
    }
}
