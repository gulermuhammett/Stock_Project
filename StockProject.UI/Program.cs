using Microsoft.AspNetCore.Authentication.Cookies;
using StockProject.UI.Areas.Admin.Controllers;

namespace StockProject.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.AccessDeniedPath = "/Home/ErisimEngellendi";//Yetkisiz ki�iler girmek istedi�inde bu actiona y�nlendirilir.
                option.ExpireTimeSpan = TimeSpan.FromDays(30); //Cookinin ne zama sona erece�i
                option.SlidingExpiration = true; //�telemeli zaman a��m� aktif
                option.LoginPath = "/Home/Login";

                //option.ReturnUrlParameter = "returnUrl";
                option.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//Eklendi
            app.UseAuthorization();
            app.UseSession();//Eklendi
            app.MapControllerRoute(
                name: "areadefault",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}