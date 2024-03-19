using Auth.API.Services.Implementations;
using Auth.API.Services.Interface;
using DatabaseConfigurationLib;
using DatabaseConfigurationLib.Common;
using DatabaseConfigurationLib.Extensions;
using JwtAuthenticationManager;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

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

//Db Switch
builder.Services.ConfigureDatabase(configuration);

//Jwt Token
builder.Services.AddScoped<JwtTokenHandler>();

//SeriLog
var logger = SerilogConfigurator.CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

//CORS
builder.Services.AddCustomCorsPolicy();

// model validation
builder.Services.UseModelValidationHandler();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerHandler();//Swagger Extension  
}

app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

// Global Exception Middleware
app.UseGlobalExceptionHandler();
app.MapControllers();

app.Run();
