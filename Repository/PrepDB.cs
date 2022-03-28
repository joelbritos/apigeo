using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace APIGEO.Repository
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var service = app.ApplicationServices.CreateScope())
            {
                SeedData(service.ServiceProvider.GetService<APIGEOContext>());
            }
        }

        public static void SeedData(APIGEOContext context)
        {
            System.Console.WriteLine("Appling Migrations...");
            context.Database.Migrate();
        }
    }
}