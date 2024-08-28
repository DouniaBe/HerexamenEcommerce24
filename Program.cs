using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HerexamenEcommerce24.Data;
using HerexamenEcommerce24.Models;
using HerexamenEcommerce24.Services;
using HerexamenEcommerce24.Middleware;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using HerexamenEcommerce2024.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Voeg DbContext toe aan de container
builder.Services.AddDbContext<HerexamenEcommerce24Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HerexamenEcommerce24Context")
    ?? throw new InvalidOperationException("Connection string 'HerexamenEcommerce24Context' not found.")));

// Voeg Identity toe aan de services
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HerexamenEcommerce24Context>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();

// Voeg lokalisatie services toe
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "nl-NL", "fr-FR" };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
});

var app = builder.Build();

// Seed de database
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

// Configureer de HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "nl-NL", "fr-FR" };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
});

app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseLanguageMiddleware(); // Voeg eigen middleware toe
app.UseRequestLocalization(); // Voeg lokalisatie middleware toe
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseMiddleware<RequestLoggingMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Zorg dat Identity Razor Pages werken

app.Run();
