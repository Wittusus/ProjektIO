using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektIO.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jSH9Ud0dnXXtXdnFSRQ==;Mgo+DSMBPh8sVXJ0S0J+XE9HflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31Td0RkWH1adHBcQGdcVA==;ORg4AjUWIQA/Gnt2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkRiXn5cdHBRT2ZUVUE=;OTU2NzU1QDMyMzAyZTM0MmUzMFFEeHIxelR5OTdzVjZSNzJvTE9BMlBkclppTldnL05ZNi9aRWsxNExLNjg9;OTU2NzU2QDMyMzAyZTM0MmUzMFNnOStQS0szd2MvMWJjc0ZmRU9uSk9JY0g1QnVBR2Z2RVdKWDhXc3VWYVk9;NRAiBiAaIQQuGjN/V0Z+WE9EaFxKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdUViWHhfcnBRQmlbVEB3;OTU2NzU4QDMyMzAyZTM0MmUzMFJDWVcxY3hrWmJ6WVg2dC9pMGdzM1U2UkxmRWlDN3k1ZjlLZUxqQUpZb009;OTU2NzU5QDMyMzAyZTM0MmUzMGM4RHVTdlBtRjF6RUlPZmF1RFJtRTNYMkRzMnJrUm9DaWxZMForUmxnSGc9;Mgo+DSMBMAY9C3t2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkRiXn5cdHBRT2heVUU=;OTU2NzYxQDMyMzAyZTM0MmUzMG5HVERIRXBPWjVSUWJiL3R0c3Z6M1lEYkRBbWFrSDQ5TkdHNS9hTVc2bnc9;OTU2NzYyQDMyMzAyZTM0MmUzMFh1Uk1zeXdJQkJ3MGZ2cXJ5NTNJbGlRcjlxZHJjZi92anlGa3cxeVdEeEU9;OTU2NzYzQDMyMzAyZTM0MmUzMFJDWVcxY3hrWmJ6WVg2dC9pMGdzM1U2UkxmRWlDN3k1ZjlLZUxqQUpZb009");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
