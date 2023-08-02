// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Log;
using Scada.Web.Plugins.PlgScheme.Model;
using Scada.Web.Plugins.PlgScheme.Model.PropertyGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// Manages scheme components.
    /// <para>Менеджер, управляющий компонентами схемы.</para>
    /// </summary>
    public sealed class CompManager
    {
        /// <summary>
        /// Маска для поиска файлов библиотек компонентов.
        /// </summary>
        private const string CompLibMask = "PlgSch*Comp.dll";
        /// <summary>
        /// Стандартные типы компонентов, ключ - полное имя типа.
        /// </summary>
        private static readonly Dictionary<string, Type> StandardCompTypes;

        private readonly ILog log;                                      // журнал приложения
        private readonly object syncLock;                               // объект для синхронизации доступа
        private readonly List<CompLibSpec> allSpecs;                    // все спецификации библиотек
        private readonly Dictionary<string, CompFactory> factsByPrefix; // фабрики компонентов, ключ - XML-префикс
        private readonly Dictionary<string, CompLibSpec> specsByType;   // спецификации библиотек, ключ - имя типа компонента


        /// <summary>
        /// Initializes the class.
        /// </summary>
        static CompManager()
        {
            StandardCompTypes = new Dictionary<string, Type>()
            {
                { typeof(StaticText).FullName, typeof(StaticText) },
                { typeof(DynamicText).FullName, typeof(DynamicText) },
                { typeof(StaticPicture).FullName, typeof(StaticPicture) },
                { typeof(DynamicPicture).FullName, typeof(DynamicPicture) },
              
            };
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CompManager(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            syncLock = new object();
            allSpecs = new List<CompLibSpec>();
            factsByPrefix = new Dictionary<string, CompFactory>();
            specsByType = new Dictionary<string, CompLibSpec>();
            LoadErrors = new List<string>();
        }


        /// <summary>
        /// Получить ошибки при загрузке библиотек.
        /// </summary>
        public List<string> LoadErrors { get; private set; }


        /// <summary>
        /// Проверить, что тип компонента относится к стандартным типам.
        /// </summary>
        private static bool TypeIsStrandard(Type compType)
        {
            return compType == typeof(StaticText) || compType == typeof(DynamicText) ||
                compType == typeof(StaticPicture) || compType == typeof(DynamicPicture);
        }

        /// <summary>
        /// Очистить компоненты.
        /// </summary>
        public void ClearComponents()
        {
            allSpecs.Clear();
            factsByPrefix.Clear();
            specsByType.Clear();
            LoadErrors.Clear();
        }

        /// <summary>
        /// Добавить компоненты в словари.
        /// </summary>
        public bool AddComponents(ISchemeComp schemeComp)
        {
            if (schemeComp.CompLibSpec is CompLibSpec compLibSpec)
            {
                if (compLibSpec.Validate(out string errMsg) && compLibSpec.CompFactory != null)
                {
                    allSpecs.Add(compLibSpec);
                    factsByPrefix[compLibSpec.XmlPrefix] = compLibSpec.CompFactory;

                    foreach (CompItem compItem in compLibSpec.CompItems)
                    {
                        if (compItem != null && compItem.CompType != null)
                        {
                            specsByType[compItem.CompType.FullName] = compLibSpec;
                            AttrTranslator.TranslateAttrs(compItem.CompType);
                        }
                    }

                    return true;
                }
                else if (!string.IsNullOrEmpty(errMsg))
                {
                    LoadErrors.Add(errMsg);
                }
            }

            return false;
        }

        /// <summary>
        /// Загрузить компоненты из файлов.
        /// </summary>
        public void LoadCompFromFiles(AppDirs appDirs)
        {
            ArgumentNullException.ThrowIfNull(appDirs, nameof(appDirs));

            try
            {
                lock (syncLock)
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Загрузка компонентов из файлов" :
                        "Load components from files");

                    ClearComponents();

                    foreach (FileInfo fileInfo in new DirectoryInfo(appDirs.ExeDir)
                        .EnumerateFiles(CompLibMask, SearchOption.TopDirectoryOnly))
                    {
                        string fileName = fileInfo.FullName;
                        //PluginSpec pluginSpec = PluginSpec.CreateFromDll(fileName, out string errMsg);
                        object pluginSpec = null;
                        string errMsg = "Not implemented.";

                        if (pluginSpec == null)
                        {
                            log.WriteError(errMsg);
                        }
                        else if (pluginSpec is ISchemeComp schemeComp)
                        {
                            //pluginSpec.AppDirs = appDirs;
                            //pluginSpec.Log = log;
                            //pluginSpec.Init();

                            if (AddComponents(schemeComp))
                            {
                                log.WriteAction(Locale.IsRussian ?
                                    "Загружены компоненты из файла {0}" :
                                    "Components are loaded from the file {0}", fileName);
                            }
                            else
                            {
                                log.WriteAction(Locale.IsRussian ?
                                    "Не удалось загрузить компоненты из файла {0}" :
                                    "Unable to load components from the file {0}", fileName);
                            }
                        }
                        else
                        {
                            log.WriteError(Locale.IsRussian ?
                                "Библиотека {0} не предоставляет компоненты схем" :
                                "The assembly {0} does not provide scheme components", fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian?
                    "Ошибка при загрузке компонентов из файлов" :
                    "Error loading components from files");
            }
        }

        /// <summary>
        /// Создать компонент на основе XML-узла.
        /// </summary>
        public ComponentBase CreateComponent(XmlNode compNode, out string errMsg)
        {
            if (compNode == null)
                throw new ArgumentNullException(nameof(compNode));

            errMsg = "";
            string nodeName = compNode.Name;

            try
            {
                lock (syncLock)
                {
                    string xmlPrefix = compNode.Prefix;
                    string localName = compNode.LocalName.ToLowerInvariant();

                    if (string.IsNullOrEmpty(xmlPrefix))
                    {
                        if (localName == "statictext")
                            return new StaticText();
                        else if (localName == "dynamictext")
                            return new DynamicText();
                        else if (localName == "staticpicture")
                            return new StaticPicture();
                        else if (localName == "dynamicpicture")
                            return new DynamicPicture();
						else
                            errMsg = string.Format(SchemePhrases.UnknownComponent, nodeName);
                    }
                    else if (factsByPrefix.TryGetValue(xmlPrefix, out CompFactory compFactory))
                    {
                        ComponentBase comp = compFactory.CreateComponent(localName, true);
                        if (comp == null)
                            errMsg = string.Format(SchemePhrases.UnableCreateComponent, nodeName);
                        return comp;
                    }
                    else
                    {
                        errMsg = string.Format(SchemePhrases.CompLibraryNotFound, nodeName);
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format(SchemePhrases.ErrorCreatingComponent, nodeName);
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при создании компонента на основе XML-узла \"{0}\"" :
                    "Error creating component based on the XML node \"{0}\"", compNode.Name);
            }

            return null;
        }

        /// <summary>
        /// Создать компонент заданного типа.
        /// </summary>
        public ComponentBase CreateComponent(string compTypeName)
        {
            try
            {
                lock (syncLock)
                {
                    if (StandardCompTypes.TryGetValue(compTypeName, out Type compType))
                    {
                        return (ComponentBase)Activator.CreateInstance(compType);
                    }
                    else if (specsByType.TryGetValue(compTypeName, out CompLibSpec compLibSpec))
                    {
                        ComponentBase comp = compLibSpec.CompFactory.CreateComponent(compTypeName, false);

                        if (comp == null)
                        {
                            throw new ScadaException(Locale.IsRussian ?
                                "Фабрика компонентов вернула пустой результат." :
                                "Component factory returned an empty result.");
                        }

                        return comp;
                    }
                    else
                    {
                        throw new ScadaException(Locale.IsRussian ?
                            "Неизвестный тип компонента." :
                            "Unknown component type.");
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка при создании компонента типа \"{0}\"" :
                    "Error creating component of the type \"{0}\"", compTypeName);

                if (ex is ScadaException)
                    log.WriteError(errMsg + ": " + ex.Message);
                else 
                    log.WriteError(ex, errMsg);

                return null;
            }
        }

        /// <summary>
        /// Получить спецификацию библиотеки по типу компонента.
        /// </summary>
        public CompLibSpec GetSpecByType(Type compType)
        {
            if (compType == null)
                throw new ArgumentNullException(nameof(compType));

            if (TypeIsStrandard(compType))
            {
                return null;
            }
            else
            {
                lock (syncLock)
                {
                    specsByType.TryGetValue(compType.FullName, out CompLibSpec compLibSpec);
                    return compLibSpec;
                }
            }
        }

        /// <summary>
        /// Получить спецификации библиотек, отсортированные по заголовкам групп.
        /// </summary>
        public CompLibSpec[] GetSortedSpecs()
        {
            lock (syncLock)
            {
                int specCnt = allSpecs.Count;
                string[] headers = new string[specCnt];
                CompLibSpec[] specs = new CompLibSpec[specCnt];

                for (int i = 0; i < specCnt; i++)
                {
                    CompLibSpec spec = allSpecs[i];
                    headers[i] = spec.GroupHeader;
                    specs[i] = spec;
                }

                Array.Sort(headers, specs);
                return specs;
            }
        }

        /// <summary>
        /// Получить объединённый список стилей компонентов.
        /// </summary>
        public List<string> GetAllStyles(string rootPath)
        {
            lock (syncLock)
            {
                List<string> allStyles = new();

                foreach (CompLibSpec spec in allSpecs)
                {
                    spec.Styles.ForEach(s => allStyles.Add(rootPath + s));
                }

                return allStyles;
            }
        }

        /// <summary>
        /// Получить объединённый список скриптов компонентов.
        /// </summary>
        public List<string> GetAllScripts(string rootPath)
        {
            lock (syncLock)
            {
                List<string> allScripts = new();

                foreach (CompLibSpec spec in allSpecs)
                {
                    spec.Scripts.ForEach(s => allScripts.Add(rootPath + s));
                }

                return allScripts;
            }
        }
    }
}
