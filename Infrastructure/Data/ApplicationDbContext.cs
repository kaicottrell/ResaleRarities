using Infrastructure.Utility;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<RareImage> RareImage { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<ListingStatus> ListingStatus { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<TokenInfo> TokenInfo { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is where you can enter seed data
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Listing>()
                .Property(l => l.Compensation)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Inventory>()
                .Property(inv => inv.ResalePrice)
                .HasColumnType("decimal(18, 2)");


            //Seed Template Groups
            //modelBuilder.Entity<TemplateGroup>().HasData(
            //    new TemplateGroup
            //    {
            //        Id=1,
            //        Name = "Gym"
            //    },
            //     new TemplateGroup
            //     {
            //         Id = 2,
            //         Name = "Intelligence"
            //     }
            //);
        }


    }
}



