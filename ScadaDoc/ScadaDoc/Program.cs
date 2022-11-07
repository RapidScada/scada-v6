using Scada.Doc.Code;

namespace Scada.Doc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
                .AddSingleton(typeof(TocManager))
                .AddRazorPages();

            var app = builder.Build();
            app.UseForwardedHeaders();
            app.UsePathBase(builder.Configuration["pathBase"]);

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapRazorPages();
            app.Run();
        }
    }
}
