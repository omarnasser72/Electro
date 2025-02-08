using Electro.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Electro.Repository.Contexts
{
    public class ElectroDbContext : IdentityDbContext<User>
    {
        public ElectroDbContext(DbContextOptions<ElectroDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>()
                        .HasKey(Login => new { Login.LoginProvider, Login.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                        .HasKey(token => new { token.UserId, token.LoginProvider, token.Name });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }


    }
}
