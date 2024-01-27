/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Searches for assemblies loaded by the framework
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.IO;
using System.Reflection;

namespace Scada
{
    /// <summary>
    /// Searches for assemblies loaded by the framework.
    /// <para>Ищет сборки, загружаемые фреймворком.</para>
    /// </summary>
    public class AssemblyResolver
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AssemblyResolver(string[] probingDirs)
        {
            ProbingDirs = probingDirs ?? throw new ArgumentNullException(nameof(probingDirs));
        }


        /// <summary>
        /// Gets the directories to search for assemblies.
        /// </summary>
        public string[] ProbingDirs { get; }


        /// <summary>
        /// Loads the assembly from the specified directory, if the assembly exists.
        /// </summary>
        private Assembly LoadAssembly(string shortFileName, string probingDir)
        {
            // in .NET Core use
            // AssemblyLoadContext.GetLoadContext(requestingAssembly).LoadFromAssemblyPath(path)
            string path = Path.Combine(probingDir, shortFileName);
            return File.Exists(path)
                ? Assembly.LoadFrom(path)
                : null;
        }

        /// <summary>
        /// Finds and loads the requested assembly into the default load context.
        /// </summary>
        public Assembly Resolve(string assemblyName, Assembly requestingAssembly, out string errMsg)
        {
            if (assemblyName == null)
                throw new ArgumentNullException(nameof(assemblyName));
            if (requestingAssembly == null)
                throw new ArgumentNullException(nameof(requestingAssembly));

            // assembly name example:
            // MyAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            int commaIdx = assemblyName.IndexOf(',');
            string simpleName = commaIdx > 0
                ? assemblyName.Substring(0, commaIdx)
                : assemblyName;
            string shortFileName = simpleName + ".dll";
            errMsg = "";

            // search in the directory of the requesting assembly
            string assemblyDir = Path.GetDirectoryName(requestingAssembly.Location);
            Assembly assembly = 
                LoadAssembly(shortFileName, assemblyDir) ??
                LoadAssembly(shortFileName, Path.Combine(assemblyDir, requestingAssembly.GetName().Name));

            if (assembly != null)
                return assembly;

            // search in the probing directories
            foreach (string probingDir in ProbingDirs)
            {
                assembly = LoadAssembly(shortFileName, probingDir);

                if (assembly != null)
                    return assembly;
            }

            if (!simpleName.EndsWith(".resources", StringComparison.Ordinal))
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Резолвер не смог найти сборку '{0}'{1}    запрошенную '{2}'" :
                    "Resolver could not find assembly '{0}'{1}    requested by '{2}'",
                    assemblyName, Environment.NewLine, requestingAssembly.FullName);
            }

            return null;
        }
    }
}
