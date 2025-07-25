// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Represents a mimic diagram in runtime mode.
    /// <para>Представляет мнемосхему в режиме выполнения.</para>
    /// </summary>
    public class MimicView : ViewBase
    {
        private readonly MimicViewArgs viewArgs; // the view arguments


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicView(View viewEntity)
            : base(viewEntity)
        {
            viewArgs = new MimicViewArgs(Args);
            Mimic = new Mimic();
        }


        /// <summary>
        /// Gets the mimic diagram.
        /// </summary>
        public Mimic Mimic { get; }


        /// <summary>
        /// Adds the faceplate to the view resources.
        /// </summary>
        private void AddToResources(FaceplateMeta faceplateMeta)
        {
            Resources.Add(new ViewResource
            {
                TypeCode = faceplateMeta.TypeName,
                Path = faceplateMeta.Path
            });
        }

        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public override void LoadView(Stream stream)
        {
            Mimic.Load(stream, new LoadContext { EditMode = false });
            Mimic.Dependencies.ForEach(AddToResources);
        }

        /// <summary>
        /// Loads the view resource from the specified stream.
        /// </summary>
        public override void LoadResource(ViewResource resource, Stream stream)
        {
            if (!string.IsNullOrEmpty(resource.TypeCode) &&
                !Mimic.FaceplateMap.ContainsKey(resource.TypeCode))
            {
                Faceplate faceplate = new();
                faceplate.Load(stream, new LoadContext { EditMode = false });
                Mimic.FaceplateMap.Add(resource.TypeCode, faceplate);

                foreach (FaceplateMeta dependency in faceplate.Dependencies)
                {
                    FaceplateMeta dependencyCopy = dependency.Transit();
                    Mimic.Dependencies.Add(dependencyCopy);
                    AddToResources(dependencyCopy);
                }
            }
        }

        /// <summary>
        /// Builds the view after loading the view itself and all required resources.
        /// </summary>
        public override void Build()
        {
            // set title in template mode
            if (viewArgs.TitleCompID > 0 &&
                Mimic.ComponentMap.TryGetValue(viewArgs.TitleCompID, out Component component) &&
                component.TypeName == "Text")
            {
                component.Properties.SetValue("Text", Title);
            }
        }

        /// <summary>
        /// Binds the view to the configuration database.
        /// </summary>
        public override void Bind(ConfigDataset configDataset)
        {
            foreach (Component component in Mimic.EnumerateComponents())
            {
                if (component.Bindings != null)
                {
                    component.Bindings.BindChannels(configDataset);
                    component.Bindings.OffsetCnlNums(viewArgs.CnlOffset);
                    component.Bindings.GetAllCnlNums().ForEach(cnlNum =>
                        AddCnl(configDataset.CnlTable.GetItem(cnlNum)));
                }
            }
        }
    }
}
