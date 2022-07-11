using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Context;
using ProjectAPI.Data;
using ProjectAPI.Data.Models;
using ProjectAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services
    .AddDbContextPool<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.MapControllers();

await app.Seed();

app.MapHub<SignalHub>("/signalhub");

app.Run();
