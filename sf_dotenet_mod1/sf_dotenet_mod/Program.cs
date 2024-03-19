using Serilog;
using sf_dotenet_mod.Configuration;

var builder = WebApplication.CreateBuilder(args);

#region serilog configuration

var logger = SerilogConfiguration.ConfigureSerilog();
builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(logger);

#endregion

#region register services to the container
builder.Services.AddControllers();
builder.Services.AddControllersWithViews(options =>
{
    // Register the custom action filter globally
    //options.Filters.Add(typeof(ValidateModelStateAttribute));
});

#region configure dependency injection & Identity services
DependencyInjectionConfiguration.ConfigureDependencies(builder.Services);
IdentityConfiguration.ConfigureIdentity(builder.Services);
#endregion

builder.Services.ConfigureApplicationCookie(
    options =>
    {
        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
        options.LoginPath = new PathString("/Account/Login");
    });

#endregion

var app = builder.Build();

#region configure middlewares
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=account}/{action=login}");
#endregion

app.Run();
