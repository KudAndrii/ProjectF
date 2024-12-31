using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProjectF.Core.Constants;

namespace ProjectF.DataAccess.DbContexts;

public class SqlDbContextFactory : IDesignTimeDbContextFactory<SqlDbContext>
{
    private const string AppSettingsBaseName = $"{AppSettingsName}.{AppSettingsExtension}";
    private const string AppSettingsName = "appsettings";
    private const string AppSettingsExtension = "json";

    public SqlDbContext CreateDbContext(string[] args)
    {
        var config = GetAppConfiguration();
        var optionsBuilder = new DbContextOptionsBuilder<SqlDbContext>();
        var connectionString = config.GetConnectionString("LinkVault");
        optionsBuilder.UseSqlServer(connectionString);

        return new SqlDbContext(optionsBuilder.Options);
    }

    private IConfiguration GetAppConfiguration()
    {
        var path = Directory.GetParent(Directory.GetCurrentDirectory())?
            .GetDirectories()
            .FirstOrDefault(f => f.GetFiles().Any(j => j.Name.Equals(AppSettingsBaseName)));

        if (path is null)
        {
            throw new ApplicationException($"Unable to get {AppSettingsName} file path");
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(path.FullName)
            .AddJsonFile(AppSettingsBaseName)
            .AddJsonFile($"{AppSettingsName}.{EnvironmentAccessor.Name}.{AppSettingsExtension}", true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}