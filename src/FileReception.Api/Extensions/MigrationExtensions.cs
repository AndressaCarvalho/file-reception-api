using Microsoft.EntityFrameworkCore;

namespace FileReception.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<TContext>();

            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("MigrationExtensions");

            logger.LogError(ex, "Erro ao aplicar as migrações existentes.");

            throw;
        }
    }
}
