using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HerexamenEcommerce24.Data;
using HerexamenEcommerce24.Models;

namespace HerexamenEcommerce24
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Voeg DbContext toe aan de container
            builder.Services.AddDbContext<HerexamenEcommerce24Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HerexamenEcommerce24Context") ?? throw new InvalidOperationException("Connection string 'HerexamenEcommerce24Context' not found.")));

            // Voeg Identity toe aan de services
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<HerexamenEcommerce24Context>();

            // Voeg controllers met views toe
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed de database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services); // Roept de seeding functie aan
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Configureer de HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Voeg authenticatie middleware toe
            app.UseAuthorization();  // Voeg autorisatie middleware toe

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); // Zorg dat Identity Razor Pages werken

            app.Run();
        }
    }
}
