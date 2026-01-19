using Microsoft.AspNetCore.Cors.Infrastructure;
using ecommerce_csharp.Data;
using ecommerce_csharp.Services;  //Using the system of Cart Services 
using Microsoft.AspNetCore.Identity;  
using Microsoft.EntityFrameworkCore;
using System.Globalization;   //Price
using Microsoft.AspNetCore.Localization;  //price 
using Microsoft.AspNetCore.StaticFiles;
//using static System.Formats.Asn1.AsnWriter;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages(); //This was added for the pages in terms of Admin

builder.Services.AddSession();   //For cart
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartService>();  // For cart

var app = builder.Build();

//This code is created for the price

var supportedCultures = new[] { new CultureInfo("en-US") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

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
//app.UseStaticFiles();

//Aavif pictures
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".avif"] = "image/avif";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});




app.UseRouting();

app.UseSession();  // Very important for cart and used when you want to tell the app how long will it store the cart information

app.UseAuthorization();  //This works more when you want to tell the app what access users have in what pages


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
