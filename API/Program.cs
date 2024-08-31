using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
// Middlewares
var app = builder.Build();

// Configure the HTTP request PIPELINE
app.MapControllers();


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
