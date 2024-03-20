
using EntityFrameworkAPI.Extensions;
using EntityFrameworkAPI.Middlewares;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCustomVersioning();

builder.Services.AddControllers();

builder.Services.AddProviders();

builder.Services.AddServiceType();

builder.Services.AddSeriLogger(builder);

builder.Services.AddMapster();

builder.Services.AddModelValidation();

builder.Services.AddJwtAuthentication();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
app.Run();
