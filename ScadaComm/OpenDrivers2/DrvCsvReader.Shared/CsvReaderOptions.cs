using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvCsvReader
{
    /// <summary>
    /// Represents CSV reader options.
    /// <para>Представляет параметры считывателя CSV.</para>
    /// </summary>
    internal class CsvReaderOptions
    {
        [Description("The name of the CSV file containing data to read.")]
        public string FileName { get; set; }

        [Description("The string used as the decimal separator in numeric values.")]
        public string DecimalSeparator { get; set; } = ".";

        [Description("The delimiter used to separate fields.")]
        public string FieldDelimiter { get; set; } = ",";
    }
}
