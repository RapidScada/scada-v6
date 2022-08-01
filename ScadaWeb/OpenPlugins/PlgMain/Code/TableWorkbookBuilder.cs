// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Lang;
using Scada.Report.Xml2003.Excel;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Builds an Excel workbook that contains a table view.
    /// <para>Создает книгу Excel, которая содержит табличное представление.</para>
    /// </summary>
    internal class TableWorkbookBuilder
    {
        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "TableViewTemplate.xml";

        private readonly ConfigDatabase configDatabase;
        private readonly ScadaClient scadaClient;
        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;
        private readonly dynamic dict;

        private TableWorkbookArgs workbookArgs;
        
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableWorkbookBuilder(ConfigDatabase configDatabase, ScadaClient scadaClient, string templateDir)
        {
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            templateFilePath = Path.Combine(templateDir, TemplateFileName);
            renderer = new WorkbookRenderer();
            //renderer.BeforeProcessing += Renderer_BeforeProcessing;
            //renderer.AfterProcessing += Renderer_AfterProcessing;
            //renderer.DirectiveFound += Renderer_DirectiveFound;
            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Code.TableWorkbookBuilder");

            workbookArgs = null;

            GenerateTime = DateTime.MinValue;
        }


        /// <summary>
        /// Gets the time when a workbook was last built, UTC.
        /// </summary>
        public DateTime GenerateTime { get; private set; }


        /// <summary>
        /// Generates a workbook to the output stream.
        /// </summary>
        public void Generate(TableWorkbookArgs args, Stream outStream)
        {
            workbookArgs = args ?? throw new ArgumentNullException(nameof(args));
            workbookArgs.Validate();
            ArgumentNullException.ThrowIfNull(outStream, nameof(outStream));

            renderer.Render(templateFilePath, outStream);
        }
    }
}
