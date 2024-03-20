using SchoolWebAppAPI.Extensions;
using SchoolWebAppAPI.Helpers;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCorsPolicy();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Add SwaggerGen Config
builder.Services.UseSwaggerGenAuthButton();

//Add App versioning
builder.Services.UseAppVersioningHandler(builder);

//Add log4net
builder.Services.AddLog4net();
 
// Register dependencies
var configuration = builder.Configuration;
bool useSQL = Convert.ToBoolean(configuration["useSQL"]);
builder.Services.RegisterDependencies(useSQL);
builder.Services.JWTHandler(configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerHandler();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

// add global exception middleware
app.ConfigureCustomExceptionMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); 

app.Run();
