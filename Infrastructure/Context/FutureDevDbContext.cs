using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Infrastructure.Context
{
    public class FutureDevDbContext : DbContext
    {
        public FutureDevDbContext(DbContextOptions option) : base(option)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var relationShips = modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in relationShips)
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

        }
    } }
