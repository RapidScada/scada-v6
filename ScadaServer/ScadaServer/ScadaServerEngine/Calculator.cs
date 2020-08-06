/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaServerEngine
 * Summary  : Provides compilation and execution of scripts and formulas
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Provides compilation and execution of scripts and formulas.
    /// <para>Обеспечивает компиляцию и  выполнение скриптов и формул.</para>
    /// </summary>
    internal class Calculator
    {
        /// <summary>
        /// The string indent.
        /// </summary>
        private const string Indent = "    ";
        /// <summary>
        /// The maximum number of channels in one class.
        /// </summary>
        private const int MaxCnlCntInClass = 10000;
        /// <summary>
        /// The file name to store the source code.
        /// </summary>
        private const string SourceCodeFileName = "CalcEngine.cs";

        private readonly ServerDirs appDirs; // the application directories
        private readonly ILog log;           // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Calculator(ServerDirs appDirs, ILog log)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.log = log ?? throw new ArgumentNullException("log");
        }


        /// <summary>
        /// Generates the source code to compile.
        /// </summary>
        private string GenerateSourceCode(
            BaseTable<InCnl> inCnlTable, BaseTable<OutCnl> outCnlTable, BaseTable<Script> scriptTable,
            out Dictionary<int, string> inCnlClassNames, out Dictionary<int, string> outCnlClassNames)
        {
            inCnlClassNames = new Dictionary<int, string>();
            outCnlClassNames = new Dictionary<int, string>();

            StringBuilder sourceCode = new StringBuilder();
            sourceCode.AppendLine("namespace Scada.Server.Engine {");

            // add scripts
            sourceCode.AppendLine("public class Scripts : CalcEngine {");

            foreach (Script script in scriptTable.EnumerateItems())
            {
                sourceCode.AppendLine(script.Source);
            }

            sourceCode.AppendLine("}");

            // definitions for creating classes that implement formulas
            int cnlCnt = 0;
            int classCnt = 0;
            string className = "";

            void AddNewClass()
            {
                if (classCnt == 0 || cnlCnt == MaxCnlCntInClass)
                {
                    if (classCnt > 0)
                        sourceCode.AppendLine("}");

                    classCnt++;
                    cnlCnt = 0;
                    className = "Formulas" + classCnt;
                    sourceCode.AppendFormat("public class {0} : Scripts {", className).AppendLine();
                }
            }

            // add formulas of input channels
            foreach (InCnl inCnl in inCnlTable.EnumerateItems())
            {
                if (inCnl.FormulaEnabled && !string.IsNullOrEmpty(inCnl.Formula))
                {
                    AddNewClass();
                    inCnlClassNames[inCnl.CnlNum] = className;
                    AddInCnlMethod(sourceCode, inCnl.CnlNum, inCnl.Formula);
                    cnlCnt++;
                }
            }

            // add formulas of output channels
            foreach (OutCnl outCnl in outCnlTable.EnumerateItems())
            {
                if (outCnl.FormulaEnabled && !string.IsNullOrEmpty(outCnl.Formula))
                {
                    AddNewClass();
                    outCnlClassNames[outCnl.OutCnlNum] = className;
                    AddOutCnlMethod(sourceCode, outCnl.OutCnlNum, outCnl.Formula);
                    cnlCnt++;
                }
            }

            // close the last class
            if (classCnt > 0)
                sourceCode.AppendLine("}"); 

            // close namespace
            sourceCode.AppendLine("}");
            return sourceCode.ToString();
        }

        /// <summary>
        /// Adds a class method according to the input channel formula.
        /// </summary>
        private void AddInCnlMethod(StringBuilder sourceCode, int cnlNum, string formula)
        {
            int semicolonIndex = formula.IndexOf(';');

            if (semicolonIndex < 0)
            {
                sourceCode
                    .Append("public object CalcCnlData")
                    .Append(cnlNum)
                    .Append("() { return ")
                    .Append(formula.Trim())
                    .AppendLine("; }");
            }
            else
            {
                string valFormula = formula.Substring(0, semicolonIndex).Trim();
                string statFormula = formula.Substring(semicolonIndex).Trim();

                if (valFormula == "")
                    valFormula = "CnlVal";

                if (statFormula == "")
                    statFormula = "CnlStat";

                sourceCode
                    .Append("public object CalcCnlData")
                    .Append(cnlNum)
                    .Append("() { return new CnlData(")
                    .Append(valFormula)
                    .Append(", ")
                    .Append(statFormula)
                    .AppendLine("); }");
            }
        }

        /// <summary>
        /// Adds a class method according to the output channel formula.
        /// </summary>
        private void AddOutCnlMethod(StringBuilder sourceCode, int outCnlNum, string formula)
        {
            sourceCode
                .Append("public object CalcCmdData")
                .Append(outCnlNum)
                .Append("() { return ")
                .Append(formula.Trim())
                .AppendLine("; }");
        }

        /// <summary>
        /// Saves the source code to a file.
        /// </summary>
        private void SaveSourceCode(string sourceCode, out string fileName)
        {
            fileName = Path.Combine(appDirs.LogDir, SourceCodeFileName);
            File.WriteAllText(fileName, sourceCode, Encoding.UTF8);
        }

        /// <summary>
        /// Prepares for compilation the source code.
        /// </summary>
        private Compilation PrepareCompilation(string sourceCode)
        {
            SortedSet<string> refPaths = new SortedSet<string>
            {
                typeof(object).Assembly.Location,
                typeof(File).Assembly.Location,
                typeof(Math).Assembly.Location,
                typeof(CalcEngine).Assembly.Location,
                typeof(ScadaUtils).Assembly.Location
            };

            log.WriteAction(Locale.IsRussian ?
                "Добавлены следующие зависимости:" :
                "Added the following dependencies:");

            foreach (string path in refPaths)
            {
                log.WriteLine(Indent + path);
            }

            Compilation compilation = CSharpCompilation.Create(
                "CalcEngine",
                new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                refPaths.Select(path => MetadataReference.CreateFromFile(path)).ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, 
                    optimizationLevel: OptimizationLevel.Release));

            return compilation;
        }

        /// <summary>
        /// Retrieve methods for getting channel data from the assembly.
        /// </summary>
        private void RetrieveMethods(Assembly assembly, Dictionary<int, CnlTag> cnlTags,
            Dictionary<int, string> inCnlClassNames, Dictionary<int, string> outCnlClassNames)
        {
            Dictionary<string, CalcEngine> calcEngineMap = new Dictionary<string, CalcEngine>();

            foreach (CnlTag cnlTag in cnlTags.Values)
            {
                if (inCnlClassNames.TryGetValue(cnlTag.InCnl.CnlNum, out string className))
                {
                    if (!calcEngineMap.TryGetValue(className, out CalcEngine calcEngine))
                    {
                        Type calcEngineClass = assembly.GetType("Scada.Server.Engine." + className);
                        calcEngine = (CalcEngine)Activator.CreateInstance(calcEngineClass);
                        calcEngineMap.Add(className, calcEngine);
                    }

                    cnlTag.CalcEngine = calcEngine;
                    cnlTag.CalcCnlDataFunc = (Func<object>)Delegate.CreateDelegate(
                        typeof(Func<object>), calcEngine, "CalcCnlData" + cnlTag.InCnl.CnlNum, false, true);
                }
            }
        }

        /// <summary>
        /// Compiles the scripts and formulas.
        /// </summary>
        public bool CompileScripts(BaseDataSet baseDataSet, Dictionary<int, CnlTag> cnlTags)
        {
            try
            {
                log.WriteAction(Locale.IsRussian ?
                    "Компиляция исходного кода скриптов и формул" :
                    "Compile the source code of scripts and formulas");

                string sourceCode = GenerateSourceCode(
                    baseDataSet.InCnlTable, baseDataSet.OutCnlTable, baseDataSet.ScriptTable,
                    out Dictionary<int, string> inCnlClassNames, out Dictionary<int, string> outCnlClassNames);
                SaveSourceCode(sourceCode, out string sourceCodeFileName);
                Compilation compilation = PrepareCompilation(sourceCode);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    EmitResult result = compilation.Emit(memoryStream);

                    if (result.Success)
                    {
                        Assembly assembly = Assembly.Load(memoryStream.ToArray());
                        RetrieveMethods(assembly, cnlTags, inCnlClassNames, outCnlClassNames);

                        log.WriteAction(Locale.IsRussian ?
                            "Исходный код скриптов и формул скомпилирован успешно" :
                            "The source code of scripts and formulas has been compiled successfully");
                        return true;
                    }
                    else
                    {
                        // write errors to log
                        log.WriteError(Locale.IsRussian ?
                            "Ошибка при компиляции исходного кода скриптов и формул:" :
                            "Error compiling the source code of the scripts and formulas:");

                        foreach (Diagnostic diagnostic in result.Diagnostics)
                        {
                            if (diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
                                log.WriteLine(Indent + diagnostic);
                        }

                        log.WriteLine(Indent + string.Format(Locale.IsRussian ?
                            "Проверьте исходный код в файле {0}" :
                            "Check the source code in {0}", sourceCodeFileName));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при компиляции исходного кода скриптов и формул" :
                    "Error compiling the source code of the scripts and formulas");
                return false;
            }
        }

        /// <summary>
        /// Calculates the input channel data.
        /// </summary>
        public CnlData CalcCnlData(CalcEngine calcEngine, Func<object> calcCnlDataFunc, 
            int cnlNum, CnlData initialCnlData)
        {
            if (calcEngine != null && calcCnlDataFunc != null)
            {
                object cnlDataObj = calcCnlDataFunc();
                CnlData newCnlData = CnlData.Empty;

                if (cnlDataObj is double cnlVal)
                {
                    newCnlData = new CnlData(cnlVal, initialCnlData.Stat);
                }
                else if (cnlDataObj is CnlData cnlData)
                {
                    newCnlData = cnlData;
                }

                return newCnlData;
            }
            else
            {
                return initialCnlData;
            }
        }
    }
}
