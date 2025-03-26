﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Log;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Config;
using Scada.Web.Plugins.PlgMimic.MimicModel;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Manages mimic diagrams being edited.
    /// <para>Управляет редактируемыми мнемосхемами.</para>
    /// </summary>
    public class EditorManager
    {
        private readonly IWebContext webContext; // the web application context
        private readonly object editorLock;      // synchronizes access to the open mimics
        private readonly Dictionary<string, MimicGroup> mimicGroups;        // the mimic groups by project file name
        private readonly Dictionary<string, MimicInstance> mimicByFileName; // the mimics accessed by file name
        private readonly Dictionary<long, MimicInstance> mimicByKey;        // the mimics accessed by editor key


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EditorManager(IWebContext webContext)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            editorLock = new object();
            mimicGroups = [];
            mimicByFileName = [];
            mimicByKey = [];

            EditorConfig = new EditorConfig();
            MimicPluginConfig = new MimicPluginConfig();
            PluginLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(webContext.AppDirs.LogDir, EditorUtils.LogFileName),
                CapacityMB = webContext.AppConfig.GeneralOptions.MaxLogSize
            };
            ComponentList = new ComponentList();
            Translation = new PropertyTranslation();
        }


        /// <summary>
        /// Gets the configuration of the editor plugin.
        /// </summary>
        public EditorConfig EditorConfig { get; }

        /// <summary>
        /// Gets the configuration of the mimic plugin.
        /// </summary>
        public MimicPluginConfig MimicPluginConfig { get; }

        /// <summary>
        /// Gets the plugin log.
        /// </summary>
        public ILog PluginLog { get; }

        /// <summary>
        /// Gets the list of available components.
        /// </summary>
        public ComponentList ComponentList { get; }

        /// <summary>
        /// Gets the translation of mimic and component properties.
        /// </summary>
        public PropertyTranslation Translation { get; }

        
        /// <summary>
        /// Adds the specified mimic to the editor.
        /// </summary>
        private MimicInstance AddMimic(string projectFileName, string mimicFileName, Mimic mimic)
        {
            MimicGroup mimicGroup;

            lock (mimicGroups)
            {
                if (!mimicGroups.TryGetValue(projectFileName, out mimicGroup))
                {
                    mimicGroup = new MimicGroup(projectFileName);
                    mimicGroups.Add(projectFileName, mimicGroup);
                }
            }

            MimicInstance mimicInstance = new()
            {
                FileName = mimicFileName,
                Mimic = mimic,
                MimicKey = ScadaUtils.GenerateUniqueID(),
                ParentGroup = mimicGroup
            };

            mimicByFileName.Add(mimicFileName, mimicInstance);
            mimicByKey.Add(mimicInstance.MimicKey, mimicInstance);
            mimicGroup.AddMimic(mimicInstance);
            return mimicInstance;
        }

        /// <summary>
        /// Loads the configuration of the editor and mimic plugins.
        /// </summary>
        public void LoadConfig()
        {
            if (!EditorConfig.Load(webContext.Storage, EditorConfig.DefaultFileName, out string errMsg))
            {
                PluginLog.WriteError(errMsg);
                webContext.Log.WriteError(WebPhrases.PluginMessage, EditorPluginInfo.PluginCode, errMsg);
            }

            if (!MimicPluginConfig.Load(webContext.Storage, MimicPluginConfig.DefaultFileName, out errMsg))
            {
                PluginLog.WriteError(errMsg);
                webContext.Log.WriteError(WebPhrases.PluginMessage, EditorPluginInfo.PluginCode, errMsg);
            }
        }

        /// <summary>
        /// Obtains components from the active plugins.
        /// </summary>
        public void ObtainComponents()
        {
            ComponentList.Groups.Add(new StandardComponentGroup());
            Translation.Init(ComponentList);
        }

        /// <summary>
        /// Opens a mimic from the specified file.
        /// </summary>
        public OpenResult OpenMimic(string fileName)
        {
            try
            {
                Monitor.Enter(editorLock);

                // find already open mimic
                if (mimicByFileName.TryGetValue(fileName, out MimicInstance openMimicInstance))
                {
                    return new OpenResult
                    {
                        IsSuccessful = true,
                        MimicKey = openMimicInstance.MimicKey
                    };
                }

                // find project
                if (!EditorUtils.FindProject(fileName, out string projectFileName))
                    throw new ScadaException(EditorPhrases.ProjectNotFound);

                // load mimic
                Mimic mimic = new();

                using (FileStream mimicStream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    mimic.Load(mimicStream);
                }

                // load faceplates
                string viewDir = EditorUtils.GetViewDir(projectFileName);

                foreach (FaceplateMeta faceplateMeta in mimic.Dependencies)
                {
                    if (!string.IsNullOrEmpty(faceplateMeta.TypeName) &&
                        !mimic.Faceplates.ContainsKey(faceplateMeta.TypeName))
                    {
                        string faceplateFileName = Path.Combine(viewDir, faceplateMeta.Path);
                        using FileStream faceplateStream =
                            new(faceplateFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        Faceplate faceplate = new();
                        faceplate.Load(faceplateStream);
                        mimic.Faceplates.Add(faceplateMeta.TypeName, faceplate);
                    }
                }

                // add mimic to the editor
                MimicInstance mimicInstance = AddMimic(projectFileName, fileName, mimic);
                PluginLog.WriteAction(Locale.IsRussian ?
                    "Загружена мнемосхема {0}" :
                    "{0} mimic loaded", fileName);

                return new OpenResult
                {
                    IsSuccessful = true,
                    MimicKey = mimicInstance.MimicKey
                };
            }
            catch (Exception ex)
            {
                string errMsg = ex.BuildErrorMessage(EditorPhrases.LoadMimicError);
                PluginLog.WriteError(errMsg);

                return new OpenResult
                {
                    IsSuccessful = false,
                    ErrorMessage = errMsg
                };
            }
            finally
            {
                Monitor.Exit(editorLock);
            }
        }

        /// <summary>
        /// Finds an open mimic by the key.
        /// </summary>
        public bool FindMimic(long mimicKey, out MimicInstance mimicInstance, out string errMsg)
        {
            lock (editorLock)
            {
                if (mimicByKey.TryGetValue(mimicKey, out mimicInstance))
                {
                    errMsg = "";
                    return true;
                }
                else
                {
                    mimicInstance = null;
                    errMsg = EditorPhrases.MimicNotFound;
                    return false;
                }
            }
        }

        /// <summary>
        /// Saves the mimic specified by the key.
        /// </summary>
        public bool SaveMimic(long mimicKey, out string errMsg)
        {
            if (!FindMimic(mimicKey, out MimicInstance mimicInstance, out errMsg))
                return false;

            try
            {
                lock (mimicInstance.Mimic.SyncRoot)
                {
                    using FileStream mimicStream =
                        new(mimicInstance.FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                    mimicInstance.Mimic.Save(mimicStream);
                }

                PluginLog.WriteAction(Locale.IsRussian ?
                    "Сохранена мнемосхема {0}" :
                    "{0} mimic loaded", mimicInstance.FileName);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(EditorPhrases.SaveMimicError);
                PluginLog.WriteError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Closes the mimic specified by the key.
        /// </summary>
        public void CloseMimic(long mimicKey)
        {
            lock (editorLock)
            {
                if (mimicByKey.TryGetValue(mimicKey, out MimicInstance mimicInstance))
                {
                    mimicByFileName.Remove(mimicInstance.FileName);
                    mimicByKey.Remove(mimicKey);

                    lock (mimicGroups)
                    {
                        MimicGroup mimicGroup = mimicInstance.ParentGroup;
                        mimicGroup.RemoveMimic(mimicInstance);

                        if (mimicGroup.IsEmpty)
                            mimicGroups.Remove(mimicGroup.Name);
                    }

                    PluginLog.WriteAction(Locale.IsRussian ?
                        "Закрыта мнемосхема {0}" :
                        "{0} mimic closed", mimicInstance.FileName);
                }
            }
        }

        /// <summary>
        /// Gets a snapshot containing the mimic groups.
        /// </summary>
        public MimicGroup[] GetMimicGroups()
        {
            lock (mimicGroups)
            {
                return [.. mimicGroups.Values.OrderBy(g => g.Name)];
            }
        }
    }
}
