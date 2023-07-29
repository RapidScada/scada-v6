
using System.ComponentModel;

namespace Scada.Web.Plugins.PlgSchShapeComp.Code
{
	[TypeConverter(typeof(EnumConverter))]
	public enum PopupWidth
	{
		/// <summary>
		/// Normal
		/// </summary>
		#region Attributes
		[Description("Normal")]
		#endregion
		Normal,

		/// <summary>
		/// Small
		/// </summary>
		#region Attributes
		[Description("Small")]
		#endregion
		Small,

		/// <summary>
		/// Large
		/// </summary>
		#region Attributes
		[Description("Large")]
		#endregion
		Large
	}
}