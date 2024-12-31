using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ProjectF.Api.Configurations;
using ProjectF.Api.Middlewares;
using ProjectF.Cache;
using ProjectF.DataAccess;
using ProjectF.OmdbClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .WithDataAccessServices(builder.Configuration)
    .WithApiVersioning()
    .WithExceptionHandlers()
    .WithCaching(builder.Configuration)
    .WithOmdbClientServices()
    .WithHealthChecks(builder.Configuration);

var app = builder.Build();

// Apply migrations on startup
if (app.Configuration.GetValue<bool>("Migrate"))
{
    app.Services.Migrate();
}

if (app.Environment.IsDevelopment())
{
    // https://domain:port/openapi/v1.json
    app.MapOpenApi();
}

var apiVersionSet = app.UseVersioning();
app.UseMiddleware<ApiContextMetadataMiddleware>();
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers().WithApiVersionSet(apiVersionSet);

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();