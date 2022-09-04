using Microsoft.EntityFrameworkCore;
using NoteTakingAPI.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                      });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<NoteDataContext>(connection =>
    connection.UseNpgsql(builder.Configuration.GetConnectionString("NoteTakingDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
