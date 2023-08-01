using Scada.Lang;

namespace Scada.Admin.Extensions.ExtImport.Code
{
	/// <summary>
	/// The phrases used by the extension.
	/// <para>Фразы, используемые расширением.</para>
	/// </summary>
	internal class ExtensionPhrases
	{
		// Scada.Admin.Extensions.ExtImport.ExtImportLogic
		public static string GeneralOptionsNode { get; private set; }
		public static string DriversNode { get; private set; }
		public static string DataSourcesNode { get; private set; }
		public static string LinesNode { get; private set; }
		public static string LineOptionsNode { get; private set; }
		public static string LineStatsNode { get; private set; }
		public static string LogsNode { get; private set; }

		// Scada.Admin.Extensions.ExtImport.Code.ExtensionUtils
		public static string DeviceNotSupported { get; private set; }
		public static string UnableCreateDeviceView { get; private set; }
		public static string NoDeviceProperties { get; private set; }

		// Scada.Admin.Extensions.ExtImport.Controls.CtrlImport1
		public static string AllCommLines { get; private set; }
		public static string DeviceInfo { get; private set; }
		public static string DeviceNotFound { get; private set; }
		public static string NoDeviceSelected { get; private set; }

		//Scada.Admin.Extensions.ExtImport.Controls.CtrlExtensionMenu
		public static string BtnImport { get; private set; }
		public static string MiImport { get; private set; }

		// Scada.Admin.Extensions.ExtImport.Forms.FrmImport
		
		public static string ImportStep1 { get; private set; }
		public static string ImportStep2 { get; private set; }
		public static string ImportStep3 { get; private set; }
		public static string ImportCompleted { get; private set; }
		public static string FileWarning { get; private set; }
		

		// Scada.Admin.Extensions.ExtImport.Forms.FrmDataSources
		public static string DriverNotSpecified { get; private set; }

		//Scada.Admin.Extensions.ExtImport.Controls.CtrlImport3
		public static string RdBtnImport1 { get; private set; }
		public static string RdBtnImport2 { get; private set; }
		public static string GrpImportLbl { get; private set; }
		public static string GrpFormatLbl { get; private set; }
		public static string GbCnlNums { get; private set; }
		public static string LblStartCnlNum { get; private set; }
		public static string LblEndCnlNum { get; private set; }
		public static string BtnMap { get; private set; }
		public static string BtnReset { get; private set; }

		//Scada.Admin.Extensions.ExtImport.Forms.FrmCnlsMerge
		public static string FrmCnlsName { get; private set; }
		public static string LblNewCnls { get; private set; }
		public static string NumCol { get; private set; }
		public static string NewCnlsNameCol { get; private set; }
		public static string NewTypeCol { get; private set; }
		public static string NewCnlsTypeCol { get; private set; }
		public static string NewTagCodeCol { get; private set; }
		public static string LblEquipCnls { get; private set; }
		public static string EquipCnlsNameCol { get; private set; }
		public static string EquipTypeCol { get; private set; }
		public static string EquipCnlsTypeCol { get; private set; }
		public static string EquipTagCodeCol { get; private set; }
	


		// Scada.Admin.Extensions.ExtImport.Forms.FrmVariablesMerge
		public static string FrmVariablesMergeName { get; private set; }
		public static string ChkBoxMrgDesc { get; private set; }
		public static string SrcLblName { get; private set; }
		public static string AdressColName { get; private set; }
		public static string SrcMneColName { get; private set; }
		public static string SrcTypeColName { get; private set; }
		public static string SrcCmentColName { get; private set; }
		
		public static string DestLblName { get; private set; }
		public static string DestMneColName { get; private set; }
		public static string DestTypeColName { get; private set; }
		public static string DestCmentColName { get; private set; }
		public static string BtnSave { get; private set; }
		public static string BtnCancel { get; private set; }

		public static string SelectWarning { get; private set; }



		public static void Init()
		{
			LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.ExtImportLogic");
			GeneralOptionsNode = dict["GeneralOptionsNode"];
			DriversNode = dict["DriversNode"];
			DataSourcesNode = dict["DataSourcesNode"];
			LinesNode = dict["LinesNode"];
			LineOptionsNode = dict["LineOptionsNode"];
			LineStatsNode = dict["LineStatsNode"];
			LogsNode = dict["LogsNode"];

			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Code.ExtensionUtils");
			DeviceNotSupported = dict["DeviceNotSupported"];
			UnableCreateDeviceView = dict["UnableCreateDeviceView"];
			NoDeviceProperties = dict["NoDeviceProperties"];

			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Controls.CtrlImport1");
			AllCommLines = dict["AllCommLines"];
			DeviceInfo = dict["DeviceInfo"];
			DeviceNotFound = dict["DeviceNotFound"];
			NoDeviceSelected = dict["NoDeviceSelected"];



			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Controls.CtrlExtensionMenu");
			BtnImport = dict["btnImport"];
			MiImport = dict["miImport"];

			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Forms.FrmImport");
			ImportStep1 = dict["ImportStep1"];
			ImportStep2 = dict["ImportStep2"];
			ImportStep3 = dict["ImportStep3"];
			ImportCompleted = dict["ImportCompleted"];
			FileWarning = dict["fileWarning"];
			

			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Forms.FrmDataSources");
			DriverNotSpecified = dict["DriverNotSpecified"];
			

		
			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Forms.FrmCnlsMerge");
			FrmCnlsName = dict["frmName"];
			LblNewCnls = dict["lblNewCnls"];
			NumCol = dict["numCol"];
			NewCnlsNameCol = dict["newCnlsNameCol"];
			NewTypeCol = dict["newTypeCol"];
			NewCnlsTypeCol = dict["newCnlsTypeCol"];
			NewTagCodeCol = dict["newTagCodeCol"];

			LblEquipCnls = dict["lblEquipCnls"];
			EquipCnlsNameCol = dict["equiCnlsNameCol"];
			EquipTypeCol = dict["equipTypeCol"];
			EquipCnlsTypeCol = dict["equipCnlsTypeCol"];
			EquipTagCodeCol = dict["equipTagCodeCol"];


		dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Forms.FrmVariablesMerge");
			FrmVariablesMergeName = dict["frmName"];
			ChkBoxMrgDesc = dict["checkBoxDesc"];
			SrcLblName = dict["srcLbl"];
			AdressColName = dict["address"];
			SrcMneColName = dict["srcMne"];
			SrcTypeColName = dict["srcType"];
			SrcCmentColName = dict["srcCment"];

			DestLblName = dict["destLbl"];
			
			DestMneColName = dict["destMne"];
			DestTypeColName = dict["destType"];
			DestCmentColName = dict["destCment"];

			BtnSave = dict["btnSave"];
			BtnCancel = dict["btnCancel"];
			SelectWarning = dict["selectWarning"];

			dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtImport.Controls.CtrlImport3");
			RdBtnImport1 = dict["rdBtn1"];
			RdBtnImport2 = dict["rdBtn2"];
			GrpImportLbl = dict["grpImportLbl"];
			GrpFormatLbl = dict["grpFormatLbl"];

			GbCnlNums = dict["gbCnlNums"];
			LblStartCnlNum = dict["lblStartCnlNum"];
			LblEndCnlNum = dict["lblEndCnlNum"];
			BtnMap = dict["btnMap"];
			BtnReset = dict["btnReset"];	


		}
	}
}