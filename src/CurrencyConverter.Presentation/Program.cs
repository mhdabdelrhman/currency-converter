var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Configure all the services
builder.Services.ConfigureServices(configuration);

var app = builder.Build();

// Configure and use all middlewares
app.UseMiddlewares(configuration);

app.Run();