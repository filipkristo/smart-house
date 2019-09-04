using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;

namespace SmartHouseDataStore
{
    public static class DbMigration
    {
        public static void Migrate(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<SmartHouseContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}