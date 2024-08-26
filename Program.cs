using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HerexamenEcommerce24.Data;
using HerexamenEcommerce24.Models;
using HerexamenEcommerce24.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HerexamenEcommerce24Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HerexamenEcommerce24Context")
    ?? throw new InvalidOperationException("Connection string 'HerexamenEcommerce24Context' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HerexamenEcommerce24Context>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await SeedData.Initialize(services); // Async seeding functie aanroepen
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Zorg dat Identity Razor Pages werken

app.Run();
