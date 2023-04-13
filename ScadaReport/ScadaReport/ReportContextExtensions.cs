// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;

namespace Scada.Report
{
    /// <summary>
    /// Provides extensions to the IReportContext interface.
    /// <para>Предоставляет расширения интерфейса IReportContext.</para>
    /// </summary>
    public static class ReportContextExtensions
    {
        /// <summary>
        /// Finds an archive entity by the first non-empty archive code.
        /// Raises an exception if not found.
        /// </summary>
        public static Archive FindArchive(this IReportContext reportContext, params string[] archiveCodes)
        {
            string archiveCode = ScadaUtils.FirstNonEmpty(archiveCodes);
            return reportContext.ConfigDatabase.ArchiveTable.SelectFirst(new TableFilter("Code", archiveCode)) ??
                throw new ScadaException(Locale.IsRussian ?
                    "Архив не найдён в базе конфигурации." :
                    "Archive not found in the configuration database.");
        }

        /// <summary>
        /// Finds channel entities by the specified channel numbers.
        /// If some channel does not exists, adds an empty channel.
        /// </summary>
        public static List<Cnl> FindChannels(this IReportContext reportContext, IList<int> cnlNums)
        {
            if (cnlNums == null)
            {
                return new List<Cnl>();
            }
            else
            {
                List<Cnl> cnls = new(cnlNums.Count);

                foreach (int cnlNum in cnlNums)
                {
                    cnls.Add(
                        reportContext.ConfigDatabase.CnlTable.GetItem(cnlNum) ?? 
                        new Cnl { CnlNum = cnlNum, Name = "" });
                }

                return cnls;
            }
        }
    }
}
