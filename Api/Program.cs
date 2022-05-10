using Api.Models;
using Dotmim.Sync.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// [Required]: Handling multiple sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<VehiclesContext>(opt => opt.UseSqlServer(connectionString));

var tables = new string[] { "Vehicle" };

builder.Services.AddSyncServer<SqlSyncProvider>(connectionString, tables);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
