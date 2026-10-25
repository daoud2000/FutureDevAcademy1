using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Context.Data
{
    public static class CategoriesSeedData
    {
        public static async Task InitializeAsync(FutureDevDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new Category
                {

                    Name = "Sales",
                    Courses = new List<Course>(),
                },

                    new Category { Name = "IT", Courses = new List<Course>() },
                    new Category { Name = "Hr", Courses = new List<Course>() },
                    new Category { Name = "Marketing", Courses = new List<Course>() 


                });



        }




        await context.SaveChangesAsync();
    }
}
    }
