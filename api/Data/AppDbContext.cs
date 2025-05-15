using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<Portfolio>()
               .HasKey(p => new { p.AppUserId, p.StockId });

           modelBuilder.Entity<Portfolio>()
               .HasOne(p => p.AppUser)
               .WithMany(u => u.Portfolios)
               .HasForeignKey(p => p.AppUserId);

           modelBuilder.Entity<Portfolio>()
               .HasOne(p => p.Stock)
               .WithMany(s => s.Portfolios)
               .HasForeignKey(p => p.StockId);

           List<IdentityRole> roles = new List<IdentityRole>
           {
               new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
               new IdentityRole { Name = "User", NormalizedName = "USER" }
           };
              modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
        
    }
}