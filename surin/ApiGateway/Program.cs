using JwtAuthenticationManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();



var app = builder.Build();

app.UseCors("AllowSpecificOrigin");
await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
