using ProjectF.Api.Configurations;
using ProjectF.Api.Middlewares;
using ProjectF.Cache;
using ProjectF.OmdbClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .WithApiVersioning()
    .WithExceptionHandlers()
    .WithCaching(builder.Configuration)
    .WithOmdbClientServices();

var app = builder.Build();

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

app.Run();