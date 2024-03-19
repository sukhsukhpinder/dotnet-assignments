using RegistarationApp.Core.Extentions;
using RegistarationApp.Server.Extensions;
using RegistrationApp.Server;
using RegistrationApp.Server.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Add logger
builder.Host.UseLoggerHandler();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add Swegger
builder.Services.UseSwaggerServiceHandler();

//Enforce lower case
builder.Services.AddRouting(options => options.LowercaseUrls = true);

//API Versioning
builder.Services.UseVersioningHandler();

//GlobalModelValidationFilter
builder.Services.UseModelValidationHandler();

//Get Configs
builder.UseConfigurationHandler();

//Configure Auto Mapper
builder.Services.AddAutoMapper(typeof(RegistrationAppAutoMapperProfile));

//Register core modules
builder.Services.UseRepositoryHandler();

//Authentication
builder.Services.UseJwtAuthenticationHandler();

var app = builder.Build();

//Used for log the default logging for serilog
app.UseSerilogRequestLogging();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

//Global Exception handler
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Add swagger UI via extension handler
    app.UseSwaggerUIHandler();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
