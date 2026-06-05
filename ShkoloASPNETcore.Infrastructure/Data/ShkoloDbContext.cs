using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Infrastructure.Models.ShkoloClone.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Infrastructure.Data
{
    public class ShkoloDbContext : IdentityDbContext<ApplicationUser>
    {
        public ShkoloDbContext(DbContextOptions<ShkoloDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Grade>()
                .HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seeding roles and admin user
            string adminRoleId = "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d";
            string teacherRoleId = "b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e";
            string studentRoleId = "c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Administrator", NormalizedName = "ADMINISTRATOR", ConcurrencyStamp = "55122ea8-6932-474d-96eb-f6e80b4dbdf4" },
                new IdentityRole { Id = teacherRoleId, Name = "Teacher", NormalizedName = "TEACHER", ConcurrencyStamp = "03df52f8-5813-40fa-80e2-6cf7d8d21b7a" },
                new IdentityRole { Id = studentRoleId, Name = "Student", NormalizedName = "STUDENT", ConcurrencyStamp = "94bf82fa-2613-41fa-90e2-6cf7d8d21b7b" }
            );

            string adminUserId = "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d";
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@shkolo.bg",
                NormalizedUserName = "ADMIN@SHKOLO.BG",
                Email = "admin@shkolo.bg",
                NormalizedEmail = "ADMIN@SHKOLO.BG",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = "AQAAAAIAAYagAAAAEG3g66dfj9Kklm88AAnvR6DqPrXmEUtYvPq7lzOpwXmRtzv6Q==",
                
                // Guid hashes to prevent stupid ef core errors
                SecurityStamp = "bca6230f-b472-466d-a60d-4ef852923508",
                ConcurrencyStamp = "83ef07a6-8054-4638-b75d-3571d87e0dc0"
            };

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign admin role to admin user
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }
    }
}
