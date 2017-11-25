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
        public DbSet<Role> Roles { get; set; }
        public DbSet<ActionCategory> ActionCategories { get; set; }
        public DbSet<ControllerAction> ControllerActions { get; set; }
        public DbSet<UserPasswordReset> UserPasswordResets { get; set; }

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

            modelBuilder.Entity<User>()
                .HasMany(u => u.ActionCategories)
                .WithMany(a => a.Users)
                .Map(ua =>
                {
                    ua.MapLeftKey("UserId");
                    ua.MapRightKey("ActionCategoryId");
                    ua.ToTable("UserActionCategory");
                });

            modelBuilder.Entity<User>()
                        .HasMany(u => u.ControllerActions)
                        .WithMany(a => a.Users)
                        .Map(ua =>
                        {
                            ua.MapLeftKey("UserId");
                            ua.MapRightKey("ActionId");
                            ua.ToTable("UserAction");
                        });

            modelBuilder.Entity<Role>()
                .HasMany(r => r.ActionCategories)
                .WithMany(a => a.Roles)
                .Map(ar =>
                {
                    ar.MapLeftKey("RoleId");
                    ar.MapRightKey("ActionCategoryId");
                    ar.ToTable("RoleActionCategory");
                });

            modelBuilder.Entity<Role>()
                .HasMany(r => r.ControllerActions)
                .WithMany(a => a.Roles)
                .Map(ar =>
                {
                    ar.MapLeftKey("RoleId");
                    ar.MapRightKey("ActionId");
                    ar.ToTable("RoleAction");
                });
        }


    }
}