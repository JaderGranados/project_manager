using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_manager.data.Models;
using System.IO;

namespace project_manager.data.Context
{
    public class ApplicationDBContext: DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Task> Task { get; set; }

        private readonly string _connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build()
            .GetConnectionString("SqlServer");

        public ApplicationDBContext() { }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Models relationships
            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(x => x.Tasks)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<User>()
                .HasMany(x => x.Tasks)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTask>()
                .HasKey(x => new { x.UserId, x.TaskId });
        }

    }
}
