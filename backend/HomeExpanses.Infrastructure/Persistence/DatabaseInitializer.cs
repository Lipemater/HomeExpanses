using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeExpanses.Infrastructure.Persistence
{
    public static class DatabaseInitializer
    {
        public static async Task ApplyMigrationsAsync(this IServiceProvider services)
        {
            await using var scope = services.CreateAsyncScope();

            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();
        }
    }
}