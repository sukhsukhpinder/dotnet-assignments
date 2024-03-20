
using AuthenticationManager;
using EntityFramework.EntityFrameworkAPI.Infrastructure.Data;
using EntityFrameworkAPI.Core.Repository;
using EntityFrameworkAPI.Core.UnitOfWork;
using EntityFrameworkAPI.Infrastructure.Repository;
using EntityFrameworkAPI.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors();

builder.Services.AddCustomJwtAuthentication();

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes("Bearer")
        .RequireClaim("Role")
        .Build();
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<EFDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors();

var connString = builder.Configuration.GetConnectionString("JWTConnection");

builder.Services.AddDbContext<EFDbContext>(o => o.UseSqlServer(connString));
builder.Services.AddScoped<IUnit, Unit>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//builder.Services.AddDbContext<AppDBContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();

var _logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(_logger);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

app.UseOcelot().Wait();

app.UseStaticFiles();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
