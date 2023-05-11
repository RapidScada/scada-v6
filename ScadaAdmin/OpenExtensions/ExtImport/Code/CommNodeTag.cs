using Scada.Admin.Project;
using Scada.Forms;
using System;

namespace Scada.Admin.Extensions.ExtImport.Code
{
	internal class CommNodeTag : TreeNodeTag
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public CommNodeTag(CommApp commApp, object relatedObject, string nodeType)
			: base(relatedObject, nodeType)
		{
			CommApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
		}

		/// <summary>
		/// Gets the Communicator application in the project.
		/// </summary>
		public CommApp CommApp { get; }
	}
}