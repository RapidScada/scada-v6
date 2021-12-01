// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// The class contains utility methods for the schemes.
    /// <para>Класс, содержащий вспомогательные методы для схем.</para>
    /// </summary>
    public static class SchemeUtils
    {
        /// <summary>
        /// The schemes version.
        /// </summary>
        public const string SchemeVersion = "6.0.0.0";
        /// <summary>
        /// The color name which means that color depends on channel status.
        /// </summary>
        public const string StatusColor = "Status";

        /// <summary>
        /// Checks that the view stamps are matched.
        /// </summary>
        public static bool ViewStampsMatched(long browserViewStamp, long serverViewStamp)
        {
            return serverViewStamp > 0 && (browserViewStamp == 0 || browserViewStamp == serverViewStamp);
        }
    }
}
