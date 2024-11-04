using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopEasy.Ims.Domain.Models.DbModels;

namespace ShopEasy.Ims.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers");
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("AspNetRoles");
            });

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(r => r.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(r => r.RoleId)
                      .IsRequired();

                entity.HasOne(u => u.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(u => u.UserId)
                      .IsRequired();

                entity.ToTable("AspNetUserRoles");
            });
        }
    }
}
