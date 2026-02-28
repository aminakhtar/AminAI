using AminAI.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=localhost;Database=aminai;Username=postgres;Password=postgres";
builder.Services.AddDbContext<WeatherDbContext>(options =>
 options.UseNpgsql(connectionString));

var app = builder.Build();

// Ensure DB is created
using (var scope = app.Services.CreateScope())
{
 var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
 var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
 logger.LogInformation("Using connection: {Connection}", db.Database.GetDbConnection().ConnectionString);

 try
 {
        // Apply EF Core migrations (creates or updates schema as needed)
        db.Database.Migrate();
        logger.LogInformation("Database migrations applied");
 }
 catch (Exception ex)
 {
 logger.LogError(ex, "Error applying database migrations");
 }
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
 app.UseSwagger();
 app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
