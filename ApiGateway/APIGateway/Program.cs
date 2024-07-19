using AuthManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add jsonfile root
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelet.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomeAuthentication(); // Add Authentication

var app = builder.Build();
await app.UseOcelot();

app.UseAuthorization();
app.UseAuthentication();


app.Run();
