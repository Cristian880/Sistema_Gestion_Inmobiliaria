using Sis_Inmobiliaria.Core.Application.ViewModels.User;
using Sis_Inmobiliaria.Infrastructure.Identity;
using Sis_Inmobiliaria.Infrastructure.Persistence;
using Sis_Inmobiliaria.Infrastructure.Shared;

namespace Sis_Inmobilaria.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(60);
                opt.Cookie.HttpOnly = true;
            });

            builder.Services.AddPersistenceLayerIoc(builder.Configuration);
            builder.Services.AddApplicationLayerIoc();
            builder.Services.AddIdentityLayerIocForWebApp(builder.Configuration);
            builder.Services.AddSharedLayerIoc(builder.Configuration);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=Index}/{id?}")
            .WithStaticAssets();

            app.Run();
        }
    }
}
