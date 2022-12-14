using Microsoft.AspNetCore.Identity;
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

builder.Services.AddDefaultIdentity<IdentityUser> ( options => options.SignIn.RequireConfirmedAccount = false )
    .AddRoles<IdentityRole> ()
    .AddEntityFrameworkStores<SalesManagementDbContext> ();

// Add services to the container.
builder.Services.AddRazorPages ();
builder.Services.AddServerSideBlazor ();
builder.Services.AddSingleton<WeatherForecastService> ();

builder.Services.AddSyncfusionBlazor ();

builder.Services.AddScoped<IEmployeeManagementService, EmployeeManagementService> ();
builder.Services.AddScoped<IProductService, ProductService> ();
builder.Services.AddScoped<IClientService, ClientService> ();
builder.Services.AddScoped<IOrderService, OrderService> ();
builder.Services.AddScoped<ISalesOrderReportService, SalesOrderReportService> ();
builder.Services.AddScoped<IOrganizationService, OrganizationService> ();
builder.Services.AddScoped<IAppointmentService, AppointmentService> ();

builder.Services.AddScoped<TokenProvider> ();

var app = builder.Build ();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense ( "NzMwMjY2QDMyMzAyZTMzMmUzME0vMTkxei9ZSGhDUHI1VWFud3FZWUx1QU5rQzh5MUhLVkpScFVuTmRBMUE9" );

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

app.UseAuthentication ();
app.UseAuthorization ();

app.MapBlazorHub ();
app.MapFallbackToPage ( "/_Host" );

app.Run ();
