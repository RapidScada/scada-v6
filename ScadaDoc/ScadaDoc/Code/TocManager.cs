// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Provides tables of contents.
    /// <para>Предоставляет оглавления.</para>
    /// </summary>
    public class TocManager
    {
        private const KnownLang DefaultLang = KnownLang.En;
        private const KnownVersion DefaultVersion = KnownVersion.V60;

        private readonly string webRootPath;           // contains the web-servable application content files
        private readonly ILogger logger;               // performs logging
        private readonly Dictionary<string, Toc> tocs; // the tables of contents accessed by short file name


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TocManager(IWebHostEnvironment webHostEnvironment, ILogger<TocManager> logger)
        {
            ArgumentNullException.ThrowIfNull(webHostEnvironment, nameof(webHostEnvironment));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            webRootPath = webHostEnvironment.WebRootPath;
            this.logger = logger;
            tocs = new Dictionary<string, Toc>();
        }


        /// <summary>
        /// Gets the short name of the table of contents file.
        /// </summary>
        private static string GetTocFileName(string pagePath)
        {
            string[] parts = (pagePath ?? "").Split('/', StringSplitOptions.RemoveEmptyEntries);
            string langStr = parts.Length > 0 ? parts[0].ToLowerInvariant() : "";
            string versionStr = parts.Length > 1 ? parts[1].ToLowerInvariant() : "";

            KnownLang lang = langStr switch
            {
                "en" => KnownLang.En,
                "ru" => KnownLang.Ru,
                _ => DefaultLang
            };

            KnownVersion version = versionStr switch
            {
                "5.8" => KnownVersion.V58,
                "6.0" => KnownVersion.V60,
                _ => DefaultVersion
            };

            return "Contents" + lang + (int)version + ".xml";
        }

        /// <summary>
        /// Gets the table of contents corresponding to the web page.
        /// </summary>
        public Toc GetToc(string pagePath)
        {
            try
            {
                Monitor.Enter(tocs);
                string shortFileName = GetTocFileName(pagePath);

                if (tocs.TryGetValue(shortFileName, out Toc toc))
                {
                    return toc;
                }
                else
                {
                    string fileName = Path.Combine(webRootPath, "contents", shortFileName);

                    if (File.Exists(fileName))
                    {
                        toc = new Toc();
                        tocs[shortFileName] = toc;
                        toc.LoadFromFile(fileName);
                        logger.LogInformation("Contents file \"{fn}\" loaded.", fileName);
                        return toc;
                    }
                    else
                    {
                        tocs[shortFileName] = null;
                        logger.LogWarning("Contents file \"{fn}\" not found.", fileName);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting table of contents.");
                return null;
            }
            finally
            {
                Monitor.Exit(tocs);
            }
        }
    }
}
