// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Data.Tables;
using Scada.Report.Xml2003.Excel;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Builds an Excel workbook that contains events.
    /// <para>Создает книгу Excel, которая содержит события.</para>
    /// </summary>
    internal class EventWorkbookBuilder
    {
        /// <summary>
        /// The workbook template file name.
        /// </summary>
        private const string TemplateFileName = "EventTemplate.xml";

        private readonly ConfigDatabase configDatabase;
        private readonly ScadaClient scadaClient;
        private readonly string templateFilePath;
        private readonly WorkbookRenderer renderer;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventWorkbookBuilder(ConfigDatabase configDatabase, ScadaClient scadaClient, string templateDir)
        {
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            this.scadaClient = scadaClient ?? throw new ArgumentNullException(nameof(scadaClient));
            templateFilePath = Path.Combine(templateDir, TemplateFileName);
            renderer = new WorkbookRenderer();

            EventFilter = null;
        }


        /// <summary>
        /// Gets or sets the event filter.
        /// </summary>
        public DataFilter EventFilter { get; set; }


        /// <summary>
        /// Builds a workbook to the output stream.
        /// </summary>
        public void Build(Stream outStream)
        {
            renderer.Render(templateFilePath, outStream);
        }
    }
}
