
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Data.Entities;

namespace Scada.Admin.Extensions.ExtImport.Code
{
	internal class ConfigDictionaries
	{
		public static Dictionary<string, int> CnlDataType { get; } = new Dictionary<string, int>
		{
			{"BOOL", 1 },
			{"EBOOL", 1 },
			{"REAL", 0 },
			{"FLOAT", 0 },
			{"INT", 1 },
			{"LONG", 1 },
			{"SHORT", 1 },
			{"DWORD", 1 },
			{"QWORD", 1 },
			{"UNDEFINED", 3 },
			{"WORD", 1 },
		};

		public static Dictionary<int, string> CnlDataTypeDictionary { get; } = new Dictionary<int, string>
		{
			{1, "BOOL"},
			{2, "EBOOL"},
			{3, "REAL"},
			{4, "FLOAT"},
			{5, "INT"},
			{6, "LONG"},
			{7, "SHORT"},
			{8, "DWORD"},
			{9, "QWORD"},
			{10, "UNDEFINED"},
			{11, "WORD"}
		};

		public static Dictionary<int, string> CnlTypeDictionary { get; } = new Dictionary<int, string>
		{
			{1, "Input"},
			{2, "Input/output"},
			{3, "Calculated"},
			{4, "Calculated/output"},
			{5, "Output"}
		};

		public static Dictionary<int, string> DataTypeDictionary { get; } = new Dictionary<int, string>
		{
			{0, "Double"},
			{1, "Integer"},
			{2, "ASCII string"},
			{3, "Unicode string"}
		};

		public static Dictionary<string, ElemType> ElemTypeDictionary { get; } = new Dictionary<string, ElemType>
		{
				{"BOOL", ElemType.Bool },
				{"EBOOL", ElemType.Bool },
				{"REAL", ElemType.Double },
				{"FLOAT", ElemType.Float },
				{"INT", ElemType.Int },
				{"LONG", ElemType.Long },
				{"SHORT", ElemType.Short },
				{"DWORD", ElemType.UInt },
				{"QWORD", ElemType.ULong },
				{"UNDEFINED", ElemType.Undefined },
				{"WORD", ElemType.UShort },
		};

		public static List<Obj> prefixesAndSuffixes { get; } = new List<Obj>
		{
			new Obj { ObjNum = 0, Name = " " },
			new Obj { ObjNum = 1, Name = "DeviceName" },
			new Obj { ObjNum = 2, Name = "TagCode" },
			new Obj { ObjNum = 3, Name = "TagNumber" },
			new Obj { ObjNum = 4, Name = "Type" }
		};
	}
}
