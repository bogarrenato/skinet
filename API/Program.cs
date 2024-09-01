using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Service adds our container

builder.Services.AddControllers();
// Registrate db context
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// service is going to live as long as the http request
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// We dont know the exact type - typeof
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();
// For Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connString = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Cannot get redis connection string");
    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<StoreContext>();



// Middlewares
var app = builder.Build();

// Configure the HTTP request PIPELINE
app.UseMiddleware<ExceptionMiddleware>();
// Allowcredentials - for cookie
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.MapControllers();
// legyen api az url ben a vegpontjai elott
app.MapGroup("api").MapIdentityApi<AppUser>();


try
{
    // When we use services outoside of DI, we have to create a scope of the services and framework disposes
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    // MigrateAsync going to make a database and apply any pending migrations as well
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
