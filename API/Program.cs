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

// Middlewares
var app = builder.Build();

// Configure the HTTP request PIPELINE
app.MapControllers();

app.Run();
