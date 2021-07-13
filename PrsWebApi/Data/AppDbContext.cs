using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrsWebApi.Models;

namespace PrsWebApi.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {
        }
        public DbSet<PrsWebApi.Models.User> Users { get; set; }
        public DbSet<PrsWebApi.Models.Vendor> Vendors { get; set; }
        public DbSet<PrsWebApi.Models.Product> Products { get; set; }
        public DbSet<PrsWebApi.Models.Request> Requests { get; set; }
        public DbSet<PrsWebApi.Models.Lineitem> Lineitems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>(e => {
                e.HasIndex(p => p.Username).IsUnique();
            });
            builder.Entity<Vendor>(e => {
                e.HasIndex(p => p.Code).IsUnique();
            });
            builder.Entity<Product>(e => {
                e.HasIndex(p => p.Partnumber).IsUnique();
            });
        }


    }
}
