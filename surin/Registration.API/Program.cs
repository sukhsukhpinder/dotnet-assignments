using DatabaseConfigurationLib.Common;
using DatabaseConfigurationLib;
using JwtAuthenticationManager;
using Microsoft.OpenApi.Models;
using Registration.API.Services.Implementations;
using Registration.API.Services.Interface;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using DatabaseConfigurationLib.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization Header using the bearer scheme, eg: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add versioning
builder.Services.UseAppVersioningHandler();

//Swagger multiple versions
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

//JWT Token Extension
builder.Services.AddCustomJwtAuthentication();

//DB Switch
builder.Services.ConfigureDatabase(configuration);

//Caching
builder.Services.AddMemoryCache();

//SeriLog
var logger = SerilogConfigurator.CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

//CORS
builder.Services.AddCustomCorsPolicy();

// Add model validation
builder.Services.UseModelValidationHandler();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICommonService, CommonService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerHandler();//Swagger Extension  
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();

// Global Exception Middleware
app.UseGlobalExceptionHandler();

app.UseCors("AllowOrigin");
app.MapControllers();

app.Run();

