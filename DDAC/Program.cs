using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DDAC.Data;
using DDAC.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DDACContextConnection") ?? throw new InvalidOperationException("Connection string 'DDACContextConnection' not found.");

builder.Services.AddDbContext<DDACContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<DDACUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DDACContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); //direct to login page
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
