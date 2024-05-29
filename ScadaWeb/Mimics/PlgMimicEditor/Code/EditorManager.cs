// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        private readonly Dictionary<string, MimicGroup> mimicGroups;        // the mimic groups accessed by group name
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
        /// Adds the specified mimic to the editor.
        /// </summary>
        private MimicInstance AddMimic(string fileName, Mimic mimic)
        {
            if (!EditorUtils.FindProject(fileName, out string projectFileName))
                throw new ScadaException(EditorPhrases.ProjectNotFound);

            string groupName = projectFileName;
            MimicGroup mimicGroup;

            lock (mimicGroups)
            {
                if (!mimicGroups.TryGetValue(groupName, out mimicGroup))
                {
                    mimicGroup = new MimicGroup { Name = groupName };
                    mimicGroups.Add(groupName, mimicGroup);
                }
            }

            MimicInstance mimicInstance = new()
            {
                FileName = fileName,
                Mimic = mimic,
                EditorKey = ScadaUtils.GenerateUniqueID()
            };

            mimicByFileName.Add(fileName, mimicInstance);
            mimicByKey.Add(mimicInstance.EditorKey, mimicInstance);
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
                        EditorKey = openMimicInstance.EditorKey
                    };
                }

                // load mimic
                Mimic mimic = new();

                using (FileStream mimicStream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    mimic.Load(mimicStream);
                }

                // load faceplates
                string mimicDir = Path.GetDirectoryName(fileName);

                foreach (FaceplateMeta faceplateMeta in mimic.Dependencies)
                {
                    if (!string.IsNullOrEmpty(faceplateMeta.TypeName) &&
                        !mimic.Faceplates.ContainsKey(faceplateMeta.TypeName))
                    {
                        string faceplateFileName = Path.Combine(mimicDir, faceplateMeta.RelativePath);
                        using FileStream faceplateStream =
                            new(faceplateFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        Faceplate faceplate = new();
                        faceplate.Load(faceplateStream);
                        mimic.Faceplates.Add(faceplateMeta.TypeName, faceplate);
                    }
                }

                // add mimic to the editor
                MimicInstance mimicInstance = AddMimic(fileName, mimic);

                return new OpenResult
                {
                    IsSuccessful = true,
                    EditorKey = mimicInstance.EditorKey
                };
            }
            catch (Exception ex)
            {
                return new OpenResult
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.BuildErrorMessage(EditorPhrases.LoadMimicError)
                };
            }
            finally
            {
                Monitor.Exit(editorLock);
            }
        }

        /// <summary>
        /// Finds an open mimic by the editor key.
        /// </summary>
        public bool FindMimic(long editorKey, out MimicInstance mimicInstance, out string errMsg)
        {
            lock (editorLock)
            {
                if (mimicByKey.TryGetValue(editorKey, out mimicInstance))
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
