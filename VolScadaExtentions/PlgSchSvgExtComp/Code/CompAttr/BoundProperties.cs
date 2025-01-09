using System.ComponentModel;

namespace Scada.Web.Plugins.SchSvgExtComp
{
    /// <summary>
    /// Component properties that can be bound to an input channel
    /// </summary>
    [TypeConverter(typeof(EnumConverter))]
    public enum BoundProperties
    {
        /// <summary>
        /// Не задано
        /// </summary>
        #region Attributes
        [Description("None")]
        #endregion
        None,

        /// <summary>
        /// Доступность
        /// </summary>
        #region Attributes
        [Description("Enabled")]
        #endregion
        Enabled,

        /// <summary>
        /// Видимость
        /// </summary>
        #region Attributes
        [Description("Visible")]
        #endregion
        Visible
    }
}