// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// Provides integrity check of the configuration database.
    /// <para>Обеспечивает проверку целостности базы конфигурации.</para>
    /// </summary>
    internal class IntegrityCheck
    {
        /// <summary>
        /// The output file name.
        /// </summary>
        public const string OutputFileName = "ScadaAdmin_IntegrityCheck.txt";

        private readonly ILog log;              // the application log
        private readonly ConfigBase configBase; // the configuration database

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public IntegrityCheck(ILog log, ConfigBase configBase)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configBase = configBase ?? throw new ArgumentNullException(nameof(configBase));
        }

        /// <summary>
        /// Executes integrity check.
        /// </summary>
        public void Execute(string outputFileName)
        {
            try
            {
                using (StreamWriter writer = new(outputFileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(ExtensionPhrases.IntegrityCheckTitle);
                    writer.WriteLine(new string('-', ExtensionPhrases.IntegrityCheckTitle.Length));
                    bool hasErrors = false;

                    foreach (IBaseTable baseTable in configBase.AllTables)
                    {
                        writer.Write(baseTable.Title);
                        writer.Write("...");

                        SortedSet<int> requiredKeys = new();
                        List<int> lostKeys = new();

                        foreach (TableRelation relation in baseTable.Dependent)
                        {
                            if (relation.ChildTable.TryGetIndex(relation.ChildColumn, out ITableIndex index))
                            {
                                foreach (int indexKey in index.EnumerateIndexKeys())
                                {
                                    // if index.AllowNull then 0 means NULL, otherwise 0 is 0
                                    if (indexKey != 0 || !index.AllowNull)
                                        requiredKeys.Add(indexKey);
                                }
                            }
                            else
                            {
                                throw new ScadaException(CommonPhrases.IndexNotFound);
                            }
                        }

                        foreach (int key in requiredKeys)
                        {
                            if (!baseTable.PkExists(key))
                                lostKeys.Add(key);
                        }

                        if (lostKeys.Count > 0)
                        {
                            hasErrors = true;
                            writer.WriteLine(ExtensionPhrases.TableHasErrors);
                            writer.WriteLine(ExtensionPhrases.LostPrimaryKeys + string.Join(", ", lostKeys));
                            writer.WriteLine();
                        }
                        else
                        {
                            writer.WriteLine(ExtensionPhrases.TableCorrect);
                        }
                    }

                    writer.WriteLine(hasErrors ? ExtensionPhrases.BaseHasErrors : ExtensionPhrases.BaseCorrect);
                }

                ScadaUiUtils.StartProcess(outputFileName);
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.IntegrityCheckError);
            }
        }
    }
}
