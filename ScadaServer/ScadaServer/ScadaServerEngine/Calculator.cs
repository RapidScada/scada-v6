/*
 * Copyright 2022 Rapid Software LLC
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
 * Modified : 2022
 */

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

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

        private readonly ServerDirs appDirs;  // the application directories
        private readonly ILog log;            // the application log
        private List<CalcEngine> calcEngines; // the objects that calculate channel data


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Calculator(ServerDirs appDirs, ILog log)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            calcEngines = null;
        }


        /// <summary>
        /// Generates the source code to compile.
        /// </summary>
        private string GenerateSourceCode(BaseTable<Cnl> cnlTable, BaseTable<Script> scriptTable,
            out Dictionary<int, string> cnlClassNames)
        {
            cnlClassNames = new Dictionary<int, string>();

            StringBuilder sourceCode = new StringBuilder();
            sourceCode.AppendLine("using Scada.Data.Const;");
            sourceCode.AppendLine("using Scada.Data.Models;");
            sourceCode.AppendLine("using System;");
            sourceCode.AppendLine("using System.Collections.Generic;");
            sourceCode.AppendLine("using System.IO;");
            sourceCode.AppendLine("using System.Linq;");
            sourceCode.AppendLine("using System.Text;");
            sourceCode.AppendLine("using static System.Math;");
            sourceCode.AppendLine();
            sourceCode.AppendLine("namespace Scada.Server.Engine {");

            // add scripts
            sourceCode.AppendLine("public class Scripts : CalcEngine {");

            foreach (Script script in scriptTable.EnumerateItems())
            {
                sourceCode
                    .Append("/********** ").Append(script.Name).AppendLine(" **********/")
                    .AppendLine(script.Source);
            }

            sourceCode.AppendLine("}").AppendLine();

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
                    sourceCode.Append("public class ").Append(className).AppendLine(" : Scripts {");
                }
            }

            // add formulas
            foreach (Cnl cnl in cnlTable.EnumerateItems())
            {
                if (cnl.Active && cnl.FormulaEnabled)
                {
                    bool inFormulaExists = !string.IsNullOrEmpty(cnl.InFormula);
                    bool outFormulaExists = !string.IsNullOrEmpty(cnl.OutFormula);

                    if (inFormulaExists || outFormulaExists)
                    {
                        AddNewClass();
                        cnlClassNames[cnl.CnlNum] = className;
                        cnlCnt++;

                        if (inFormulaExists)
                            AddInFormulaMethod(sourceCode, cnl.CnlNum, cnl.InFormula);

                        if (outFormulaExists)
                            AddOutFormulaMethod(sourceCode, cnl.CnlNum, cnl.OutFormula);
                    }
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
        /// Adds a class method according to the input formula.
        /// </summary>
        private void AddInFormulaMethod(StringBuilder sourceCode, int cnlNum, string formula)
        {
            sourceCode.Append("public CnlData CalcCnlData").Append(cnlNum).Append("() ");
            int semicolonIndex = formula.IndexOf(';');

            if (semicolonIndex < 0)
            {
                sourceCode
                    .Append("{ return ToCnlData(")
                    .Append(formula.Trim())
                    .AppendLine("); }");
            }
            else
            {
                string valFormula = formula.Substring(0, semicolonIndex).Trim();
                string statFormula = formula.Substring(semicolonIndex + 1).Trim();

                if (valFormula == "")
                    valFormula = "CnlVal";

                if (statFormula == "")
                    statFormula = "CnlStat";

                sourceCode
                    .Append("{ return new CnlData(ToCnlVal(")
                    .Append(valFormula)
                    .Append("), Convert.ToInt32(")
                    .Append(statFormula)
                    .AppendLine(")); }");
            }
        }

        /// <summary>
        /// Adds a class method according to the output formula.
        /// </summary>
        private void AddOutFormulaMethod(StringBuilder sourceCode, int cnlNum, string formula)
        {
            sourceCode
                .Append("public object CalcCmdData")
                .Append(cnlNum)
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
            string dotnetDir = Path.GetDirectoryName(typeof(object).Assembly.Location);

            SortedSet<string> refPaths = new SortedSet<string>
            {
                Path.Combine(dotnetDir, "netstandard.dll"),
                Path.Combine(dotnetDir, "System.Runtime.dll"),
                typeof(object).Assembly.Location,
                typeof(Console).Assembly.Location,
                typeof(Enumerable).Assembly.Location,
                typeof(File).Assembly.Location,
                typeof(Math).Assembly.Location,
                typeof(Stopwatch).Assembly.Location,
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
        private void RetrieveMethods(Assembly assembly, 
            Dictionary<int, CnlTag> cnlTags, Dictionary<int, OutCnlTag> outCnlTags, 
            Dictionary<int, string> cnlClassNames)
        {
            Dictionary<string, CalcEngine> calcEngineMap = new Dictionary<string, CalcEngine>();
            calcEngines = new List<CalcEngine>();

            CalcEngine GetCalcEngine(string className)
            {
                if (!calcEngineMap.TryGetValue(className, out CalcEngine calcEngine))
                {
                    Type calcEngineClass = assembly.GetType("Scada.Server.Engine." + className);
                    calcEngine = (CalcEngine)Activator.CreateInstance(calcEngineClass);
                    calcEngineMap.Add(className, calcEngine);
                    calcEngines.Add(calcEngine);
                }

                return calcEngine;
            }

            // channel tags for archiving
            foreach (CnlTag cnlTag in cnlTags.Values)
            {
                if (cnlClassNames.TryGetValue(cnlTag.Cnl.CnlNum, out string className) &&
                    !string.IsNullOrEmpty(cnlTag.Cnl.InFormula))
                {
                    CalcEngine calcEngine = GetCalcEngine(className);
                    cnlTag.CalcEngine = calcEngine;
                    cnlTag.CalcCnlDataFunc = (Func<CnlData>)Delegate.CreateDelegate(
                        typeof(Func<CnlData>), calcEngine, "CalcCnlData" + cnlTag.Cnl.CnlNum, false, true);
                }
            }

            // channel tags for sending commands
            foreach (OutCnlTag outCnlTag in outCnlTags.Values)
            {
                if (cnlClassNames.TryGetValue(outCnlTag.Cnl.CnlNum, out string className) &&
                    !string.IsNullOrEmpty(outCnlTag.Cnl.OutFormula))
                {
                    CalcEngine calcEngine = GetCalcEngine(className);
                    outCnlTag.CalcEngine = calcEngine;
                    outCnlTag.CalcCmdDataFunc = (Func<object>)Delegate.CreateDelegate(
                        typeof(Func<object>), calcEngine, "CalcCmdData" + outCnlTag.Cnl.CnlNum, false, true);
                }
            }
        }

        /// <summary>
        /// Compiles the scripts and formulas.
        /// </summary>
        public bool CompileScripts(BaseDataSet baseDataSet, 
            Dictionary<int, CnlTag> cnlTags, Dictionary<int, OutCnlTag> outCnlTags)
        {
            if (baseDataSet == null)
                throw new ArgumentNullException(nameof(baseDataSet));
            if (cnlTags == null)
                throw new ArgumentNullException(nameof(cnlTags));
            if (outCnlTags == null)
                throw new ArgumentNullException(nameof(outCnlTags));

            try
            {
                log.WriteAction(Locale.IsRussian ?
                    "Компиляция исходного кода скриптов и формул" :
                    "Compile the source code of scripts and formulas");

                string sourceCode = GenerateSourceCode(baseDataSet.CnlTable, baseDataSet.ScriptTable, 
                    out Dictionary<int, string> cnlClassNames);
                SaveSourceCode(sourceCode, out string sourceCodeFileName);
                Compilation compilation = PrepareCompilation(sourceCode);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    EmitResult result = compilation.Emit(memoryStream);

                    if (result.Success)
                    {
                        Assembly assembly = Assembly.Load(memoryStream.ToArray());
                        RetrieveMethods(assembly, cnlTags, outCnlTags, cnlClassNames);

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
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при компиляции исходного кода скриптов и формул" :
                    "Error compiling the source code of the scripts and formulas");
                return false;
            }
        }

        /// <summary>
        /// Initializes the scripts.
        /// </summary>
        public void InitScripts()
        {
            try
            {
                foreach (CalcEngine calcEngine in calcEngines)
                {
                    calcEngine.InitScripts();
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при инициализации скриптов" :
                    "Error initializing the scripts");
            }
        }

        /// <summary>
        /// Finalizes the scripts.
        /// </summary>
        public void FinalizeScripts()
        {
            try
            {
                foreach (CalcEngine calcEngine in calcEngines)
                {
                    calcEngine.FinalizeScripts();
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при завершении скриптов" :
                    "Error finalizing the scripts");
            }
        }

        /// <summary>
        /// Performs the necessary actions before the calculation.
        /// </summary>
        public void BeginCalculation(ICalcContext calcContext)
        {
            try
            {
                Monitor.Enter(this);

                foreach (CalcEngine calcEngine in calcEngines)
                {
                    calcEngine.BeginCalculation(calcContext);
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при выполнении действий перед расчётом" :
                    "Error performing actions before the calculation");
            }
        }

        /// <summary>
        /// Performs the necessary actions after the calculation.
        /// </summary>
        public void EndCalculation()
        {
            try
            {
                foreach (CalcEngine calcEngine in calcEngines)
                {
                    calcEngine.EndCalculation();
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при выполнении действий после расчёта" :
                    "Error performing actions after the calculation");
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        /// <summary>
        /// Calculates the channel data.
        /// </summary>
        public CnlData CalcCnlData(CnlTag cnlTag, CnlData initialCnlData)
        {
            if (cnlTag != null && cnlTag.CalcEngine != null && cnlTag.CalcCnlDataFunc != null)
            {
                try
                {
                    cnlTag.CalcEngine.BeginCalcCnlData(cnlTag.CnlNum, cnlTag.Cnl, initialCnlData);
                    return cnlTag.CalcCnlDataFunc();
                }
                catch
                {
                    return new CnlData(initialCnlData.Val, CnlStatusID.FormulaError);
                }
                finally
                {
                    cnlTag.CalcEngine.EndCalcCnlData();
                }
            }
            else
            {
                return initialCnlData;
            }
        }

        /// <summary>
        /// Calculates the command data.
        /// </summary>
        public bool CalcCmdData(OutCnlTag outCnlTag, double initialCmdVal, byte[] initialCmdData,
            out double cmdVal, out byte[] cmdData, out string errMsg)
        {
            if (outCnlTag != null && outCnlTag.CalcEngine != null && outCnlTag.CalcCmdDataFunc != null)
            {
                try
                {
                    outCnlTag.CalcEngine.BeginCalcCmdData(outCnlTag.Cnl.CnlNum, outCnlTag.Cnl, 
                        initialCmdVal, initialCmdData);
                    object result = outCnlTag.CalcCmdDataFunc();
                    cmdVal = double.NaN;
                    cmdData = null;

                    if (result is byte[] bytes)
                        cmdData = bytes;
                    else if (result is string str)
                        cmdData = Encoding.UTF8.GetBytes(str);
                    else if (result != null)
                        cmdVal = Convert.ToDouble(result);

                    if (double.IsNaN(cmdVal) && cmdData == null)
                    {
                        errMsg = Locale.IsRussian ?
                            "Команда отменена" :
                            "Command canceled";
                        return false;
                    }
                    else
                    {
                        errMsg = "";
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    cmdVal = double.NaN;
                    cmdData = null;
                    errMsg = ex.Message;
                    return false;
                }
                finally
                {
                    outCnlTag.CalcEngine.EndCalcCmdData();
                }
            }
            else
            {
                cmdVal = initialCmdVal;
                cmdData = initialCmdData;
                errMsg = "";
                return true;
            }
        }
    }
}
