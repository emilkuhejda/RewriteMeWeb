﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RewriteMe.DataAccess;
using RewriteMe.DataAccess.Seed;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorLoggingMiddleware>();
        }

        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var contextFactory = serviceScope.ServiceProvider.GetService<IDbContextFactory>();
                using (var context = contextFactory.Create())
                {
                    context.Database.Migrate();

                    DbSeed.Seed(context);
                }
            }
        }
    }
}
