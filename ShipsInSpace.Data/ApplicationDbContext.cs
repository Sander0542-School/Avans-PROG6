using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ShipsInSpace.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedData(builder);
        }

        public void SeedData(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new()
                {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new()
                {
                    Name = "Pirate",
                    NormalizedName = "PIRATE"
                },
            });

            var adminUserId = Guid.NewGuid().ToString();

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = adminUserId,
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Admin123!")
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = "1"
                });
        }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../ShipsInSpace.Web/appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));

            return new ApplicationDbContext(builder.Options);
        }
    }
}