// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace PlgScheme.EditorWeb
{
    /// <summary>
    /// The Scheme Editor web application.
    /// <para>Веб-приложение Редактор схем.</para>
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapRazorPages();
            app.MapControllers();
            app.Run();
        }
    }
}
