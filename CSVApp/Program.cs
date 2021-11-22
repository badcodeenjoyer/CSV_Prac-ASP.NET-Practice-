using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CSVApp.Data;
using CSVApp.Services;
using CSVApp.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CSVAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CSVAppContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRead, Read_CSVService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CSVModels}/{action=Index}");


app.Run();
