using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectF.DataAccess.DbContexts;

namespace ProjectF.DataAccess;

public static class DataAccessRegistrations
{
    public static IServiceCollection WithDataAccessServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextFactory<SqlDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ProjectF")));
        
        services.AddTransient<UnitOfWork>();
        
        return services;
    }
    
    public static void Migrate(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<SqlDbContext>();
            context.Database.Migrate();
        }
        catch (Exception exception)
        {
            var logger = services.GetRequiredService<ILogger<SqlDbContext>>();
            logger.LogError(exception, "An error occurred while applying migrations");
        }
    }
}