// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Editor.Code;

namespace PlgScheme.EditorWeb
{
    /// <summary>
    /// The Scheme Editor web application.
    /// <para>Веб-приложение Редактор схем.</para>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            var editorContext = new EditorContext();
            editorContext.Init();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services
                .AddSingleton(editorContext)
                .AddSingleton(editorContext.Log);

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapRazorPages();
            app.MapControllers();
            app.Run();
        }
    }
}
