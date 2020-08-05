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
using Scada.Data.Models;
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
    public class Calculator
    {
        private readonly ILog log; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Calculator(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
        }


        /// <summary>
        /// Compiles the scripts and formulas.
        /// </summary>
        public bool CompileScripts(BaseDataSet baseDataSet)
        {
            StringBuilder sourceCode = new StringBuilder();
            sourceCode.AppendLine("public class MyClass {");

            for (int i = 0; i < 10000; i++)
            {
                sourceCode.Append("public int MyMethod").Append(i).AppendLine("() { return 1; }");
            }

            sourceCode.AppendLine("}");

            // compile source code
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode.ToString());

            string[] refPaths = new string[] {
                typeof(object).Assembly.Location
            };

            foreach (var r in refPaths)
            {
                log.WriteLine(r);
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                "Calculator",
                syntaxTrees: new[] { syntaxTree },
                references: refPaths.Select(path => MetadataReference.CreateFromFile(path)).ToArray(),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(memoryStream);

                if (result.Success)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Исходный код скриптов и формул откомпилирован успешно" :
                        "The source code of scripts and formulas has been compiled successfully");

                    Assembly assembly = Assembly.Load(memoryStream.ToArray());
                    var type = assembly.GetType("MyClass");
                    var instance = assembly.CreateInstance("MyClass");
                    var meth = type.GetMember("MyMethod1")[0] as MethodInfo;
                    log.WriteLine(meth.Invoke(instance, null).ToString());
                    return true;
                }
                else
                {
                    log.WriteError(Locale.IsRussian ?
                        "Ошибка при компилировании исходного кода скриптов и формул:" :
                        "Error compiling the source code of the scripts and formulas:");

                    IEnumerable<Diagnostic> errors = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic error in errors)
                    {
                        log.WriteLine(string.Format("{0}: {1}", error.Id, error.GetMessage()));
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// Calculates the input channel data.
        /// </summary>
        public CnlData CalcCnlData(Func<object> calcCnlDataFunc, int cnlNum, CnlData initialCnlData)
        {
            return initialCnlData;
        }
    }
}
