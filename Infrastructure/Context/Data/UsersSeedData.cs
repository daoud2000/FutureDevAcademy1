using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Infrastructure.Context.Data
{
    public static class UserSeedData
    {
        public static async Task InitializeAsync(FutureDevDbContext context)
        {
            if (!context.Roles.Any(r => r.Code == RoleEnum.Admin))
            {
                context.Roles.Add(new Role
                {
                    Name = FutureDevConst.ADMIN_ROLE,
                    Code = RoleEnum.Admin
                });
            }

            if (!context.Roles.Any(r => r.Code == RoleEnum.Student))
            {
                context.Roles.Add(new Role
                {
                    Name = FutureDevConst.STUDENT_ROLE,
                    Code = RoleEnum.Student
                });
            }

            await context.SaveChangesAsync();

            if (!context.Users.Any())
            {
                var adminRole = context.Roles
                    .First(r => r.Code == RoleEnum.Admin);

                var passwordHasher = new PasswordHasher<User>();

                var admin = new User
                {

                    Password = "Admin@123",
                    FullName = "System Admin",
                    Email = "admin@tms.com",
                    PhoneNumber = "123",
                    RoleId = adminRole.Id,
                    




                };

                admin.Password =
                    passwordHasher.HashPassword(admin, "Admin@123");

                

                context.Users.Add(admin);

                await context.SaveChangesAsync();
            }
        }
    }
}