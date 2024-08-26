using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using HerexamenEcommerce24.Models;

namespace HerexamenEcommerce24.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HerexamenEcommerce24Context(
                serviceProvider.GetRequiredService<DbContextOptions<HerexamenEcommerce24Context>>()))
            {
                // Seed Categorieën en Producten
                if (context.Categories.Any() || context.Products.Any())
                {
                    return; // Database is al ge-seed
                }

                // Voeg Categorieën toe
                context.Categories.AddRange(
                    new Category { Name = "Electronics" },
                    new Category { Name = "Clothing" },
                    new Category { Name = "Books" }
                );

                // Voeg Producten toe
                context.Products.AddRange(
                    new Product { Name = "Laptop", Price = 999.99M, Description = "High-performance laptop", IsActive = true, CategoryId = 1 },
                    new Product { Name = "T-Shirt", Price = 19.99M, Description = "Comfortable cotton t-shirt", IsActive = true, CategoryId = 2 },
                    new Product { Name = "Book", Price = 29.99M, Description = "Interesting book", IsActive = true, CategoryId = 3 }
                );

                await context.SaveChangesAsync();

                // Seed Identity Roles and Users
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Voeg rollen toe
                string[] roleNames = { "Admin", "User" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        // Voeg rol toe
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Voeg gebruikers toe
                var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        FirstName = "Admin",
                        LastName = "User"
                    };
                    await userManager.CreateAsync(adminUser, "Admin@123");
                }

                var regularUser = await userManager.FindByEmailAsync("user@example.com");
                if (regularUser == null)
                {
                    regularUser = new ApplicationUser
                    {
                        UserName = "user@example.com",
                        Email = "user@example.com",
                        FirstName = "Regular",
                        LastName = "User"
                    };
                    await userManager.CreateAsync(regularUser, "User@123");
                }

                // Ken gebruikers aan rollen toe
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                if (!await userManager.IsInRoleAsync(regularUser, "User"))
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }
        }
    }
}
