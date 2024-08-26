using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using HerexamenEcommerce24.Models;

namespace HerexamenEcommerce24.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HerexamenEcommerce24Context(
                serviceProvider.GetRequiredService<DbContextOptions<HerexamenEcommerce24Context>>()))
            {
                // Kijk of er al data is
                if (context.Categories.Any())
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

                context.SaveChanges();
            }
        }
    }
}
