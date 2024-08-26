using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HerexamenEcommerce24.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HerexamenEcommerce24.Data
{
    public class HerexamenEcommerce24Context : IdentityDbContext<ApplicationUser>
    {
        public HerexamenEcommerce24Context(DbContextOptions<HerexamenEcommerce24Context> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
        }
     
    }
}
