using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Eazy.Tours.Areas.Identity.Data;
using Eazy.Tours.Repositories;
using Stripe;
using Eazy.Tours.Repositories.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

//var connectionString = builder.Configuration.GetConnectionString("LoginDbContextConnection");;
var connection = builder.Configuration["ConnectionString:DefaultConnection"];

builder.Services.AddDbContext<LoginDbContext>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

//var connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LoginDbContext>().AddDefaultTokenProviders();
//builder.Services.AddIdentityCore<ApplicationUser>();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<LoginDbContext>();
//builder.Services.AddIdentityCore<IdentityUser>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("PaymentSettings"));

AddScoped();

//remove this if buy without login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();
#region Authorization
AddAuthorizationPolicies();
#endregion


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
app.UseSession();
app.UseRouting();
dataSeeding();
app.UseAuthentication();
StripeConfiguration.ApiKey =
    builder.Configuration.GetSection("PaymentSettings:SecretKey").Get<string>();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//{area=Customer}/ is missing infront of controller but it cant find localhost with it

app.MapRazorPages();

app.Run();

void dataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        DbInitializer.Initializer();
    }
}

void AddAuthorizationPolicies()
{

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireExcursionOwner, policy => policy.RequireRole(Constants.Roles.ExcursionOwner));
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
    });
}

void AddScoped()
{
    
    //builder.Services.AddScoped<IDbRepository, DbRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IDbInitializer, DbInitializerRepo>();

}