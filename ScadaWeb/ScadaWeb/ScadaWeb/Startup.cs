/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : Webstation Application
 * Summary  : Configures services and the app's request pipeline
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scada.Lang;
using Scada.Web.Code;
using Scada.Web.Services;
using System;
using System.IO;
using System.Reflection;

namespace Scada.Web
{
    /// <summary>
    /// Configures services and the app's request pipeline.
    /// <para>Настраивает службы и конвейер запросов приложения.</para>
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebContext webContext)
        {
            Configuration = configuration;
            WebContext = webContext;
        }

        public IConfiguration Configuration { get; }

        public IWebContext WebContext { get; }

        // Loads plugins to integrate their pages into the web application.
        private void ConfigureApplicationParts(ApplicationPartManager apm)
        {
            foreach (string fileName in
                Directory.EnumerateFiles(WebContext.AppDirs.ExeDir, "Plg*.dll", SearchOption.TopDirectoryOnly))
            {
                if (!WebContext.PluginHolder.ContainsPlugin(
                    ScadaUtils.RemoveFileNameSuffixes(Path.GetFileName(fileName))))
                {
                    continue;
                }

                try
                {
                    Assembly assembly = Assembly.LoadFrom(fileName);

                    if (fileName.EndsWith(".Views.dll"))
                        apm.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));
                    else
                        apm.ApplicationParts.Add(new AssemblyPart(assembly));
                }
                catch (Exception ex)
                {
                    WebContext.Log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка при загрузке части приложения из файла {0}" :
                        "Error loading application part from file {0}", fileName);
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRazorPages(options =>
                {
                    options.Conventions.AuthorizeFolder(WebPath.Root);
                    options.Conventions.AllowAnonymousToPage(WebPath.IndexPage);
                    options.Conventions.AllowAnonymousToPage(WebPath.LoginPage);
                    options.Conventions.AllowAnonymousToPage(WebPath.LogoutPage);
                })
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(typeof(CheckReadyPageFilter));
                    WebContext.PluginHolder.AddFilters(options.Filters);
                })
                .ConfigureApplicationPartManager(ConfigureApplicationParts);

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/AccessDenied";
                    options.LoginPath = WebPath.LoginPage;
                    options.LogoutPath = WebPath.LogoutPage;
                });

            services
                .AddHttpContextAccessor()
                .AddSingleton(WebContext)
                .AddScoped(UserContextFactory.GetUserContext)
                .AddScoped<IClientAccessor, ClientAccessor>();

            WebContext.PluginHolder.AddServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
