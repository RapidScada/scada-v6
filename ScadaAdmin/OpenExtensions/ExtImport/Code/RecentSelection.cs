namespace Scada.Admin.Extensions.ExtImport.Code
{
	/// <summary>
	/// Represents IDs of recently selected objects.
	/// </summary>
	public class RecentSelection
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public RecentSelection()
		{
			Reset();
		}


		/// <summary>
		/// Gets or sets the name of the recently selected instance.
		/// </summary>
		public string InstanceName { get; set; }

		/// <summary>
		/// Gets or sets the number of the recently selected communication line.
		/// </summary>
		public int CommLineNum { get; set; }

		/// <summary>
		/// Gets or sets the number of the recently selected device.
		/// </summary>
		public int DeviceNum { get; set; }

		/// <summary>
		/// Gets or sets the ID of the recently selected device type.
		/// </summary>
		public int DevTypeID { get; set; }

		/// <summary>
		/// Gets or sets the number of the recently selected object.
		/// </summary>
		public int ObjNum { get; set; }


		/// <summary>
		/// Resets the selected objects.
		/// </summary>
		public void Reset()
		{
			InstanceName = "";
			CommLineNum = 0;
			DeviceNum = 0;
			DevTypeID = 0;
			ObjNum = 0;
		}
	}
}