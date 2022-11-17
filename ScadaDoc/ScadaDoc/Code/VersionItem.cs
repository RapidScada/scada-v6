namespace Scada.Doc.Code
{
    /// <summary>
    /// Represents a menu item that specifies a version.
    /// <para>Представляет элемент меню с указанием версии.</para>
    /// </summary>
    public class VersionItem : MenuItem
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public KnownVersion Version { get; set; } = KnownVersion.None;
    }
}
