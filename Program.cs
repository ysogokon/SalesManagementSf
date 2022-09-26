using Microsoft.EntityFrameworkCore;
using SalesManagementApp.Data;
using SalesManagementApp.Services;
using SalesManagementApp.Services.Contracts;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder ( args );
var connectionString = builder.Configuration.GetConnectionString ( "SalesManagementDbConnection" )
  ?? throw new InvalidOperationException ( "Connection SalesManagementDbConnection not found" );

builder.Services.AddDbContext<SalesManagementDbContext> (
    options => options.UseSqlServer ( connectionString )
  );

// Add services to the container.
builder.Services.AddRazorPages ();
builder.Services.AddServerSideBlazor ();
builder.Services.AddSingleton<WeatherForecastService> ();

builder.Services.AddSyncfusionBlazor ();

builder.Services.AddScoped<IEmployeeManagementService, EmployeeManagementService> ();

var app = builder.Build ();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense ( "NzIyMDUzQDMyMzAyZTMyMmUzMFlMZXYyWmN4VHEwR2Y1WUc4SUIyOENZQVVIV0wvSzl3M3FtSkx0bkl0TFE9" );

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment () )
{
  app.UseExceptionHandler ( "/Error" );
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts ();
}

app.UseHttpsRedirection ();

app.UseStaticFiles ();

app.UseRouting ();

app.MapBlazorHub ();
app.MapFallbackToPage ( "/_Host" );

app.Run ();
