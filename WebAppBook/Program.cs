using Domain.Entity;
using infrastructure.Data;
using infrastructure.ViewModel;
using Infrastructure.IRepository;
using Infrastructure.IRepository.ServicesRepository;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using WebAppBook.Permission;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<FreeBookDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("BookConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<FreeBookDbContext>();
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit= false;
    option.Password.RequireLowercase= false;
    option.Password.RequireUppercase= false;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequiredLength=5;
    option.Password.RequireNonAlphanumeric= false;
});
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Admin";
    option.AccessDeniedPath = "/Admin/Home/Denied";
});

// To Show All edits without stop and run the program
builder.Services.Configure<SecurityStampValidatorOptions>(option =>
option.ValidationInterval = TimeSpan.Zero
);
//


builder.Services.AddScoped<IServicesRepository<Category>,ServicesCategory>();
builder.Services.AddScoped<IServicesRepositoryLog<LogCategory>, ServicesLogCategory>();


builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();


builder.Services.AddSession();



// add Seed Method to add defaul users and roles in application

var services = builder.Services.BuildServiceProvider();
try
{
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DefaultRole.SeedAsync(roleManager);
    await DefaultUser.SeedSuperAdminAsync(roleManager, userManager);
    await DefaultUser.SeedBasicAsync(roleManager, userManager);
}
catch (Exception)
{

    throw;
}


    


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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

    app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Accounts}/{action=Login}/{id?}"
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
