using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Extensions;
using Web.Services.Implementation;
using Web.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/UserAuth/Login";           // Ruta para login
        options.LogoutPath = "/Cliente/Index";           // Ruta para logout
        options.AccessDeniedPath = "/AccesoDenegado/Error"; // Ruta acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddApiHttpClient<IAppointment, AppointmentService>(builder.Configuration);
builder.Services.AddApiHttpClient<IAuthorization, AuthorizationService>(builder.Configuration);
builder.Services.AddApiHttpClient<IService, ServiceService>(builder.Configuration);
builder.Services.AddApiHttpClient<IUser, UserService>(builder.Configuration);
builder.Services.AddApiHttpClient<ISpecialty, SpecialtyService>(builder.Configuration);
builder.Services.AddApiHttpClient<IAdmin, AdminService>(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserAuth}/{action=Login}/{id?}");

app.Run();
