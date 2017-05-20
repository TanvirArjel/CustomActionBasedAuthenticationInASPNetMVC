using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CustomAuthenticationInASPNetMVC.Models;

namespace CustomAuthenticationInASPNetMVC.DataAccessLayer
{
    public class UserDbContext : DbContext
    {
        public UserDbContext() : base("UserDbConnection")
        {
            
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasMany(u => u.Roles)
                        .WithMany(r => r.Users)
                        .Map(ur => 
                            {
                                ur.MapLeftKey("UserId");
                                ur.MapRightKey("RoleId");
                                ur.ToTable("UserRole");
                            });
        }

        public System.Data.Entity.DbSet<CustomAuthenticationInASPNetMVC.Models.Role> Roles { get; set; }
    }
}