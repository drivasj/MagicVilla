using Magic_Villa_Web;
using Magic_Villa_Web.Services;
using Magic_Villa_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Add service Villa
builder.Services.AddHttpClient<IVillaService, VillaService>();
builder.Services.AddScoped<IVillaService, VillaService>();

// Add service NumeroVilla
builder.Services.AddHttpClient<INumeroVillaService, NumeroVillaService>();
builder.Services.AddScoped<INumeroVillaService, NumeroVillaService>();

// Add service Usuario
builder.Services.AddHttpClient<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Add service for sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add service session 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add service Authentication Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                   .AddCookie(options =>
                                    {
                                        options.Cookie.HttpOnly = true;
                                        options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                                        options.LoginPath="/Usuario/Login";
                                        options.AccessDeniedPath = "/Usuario/AccesoDenegado";
                                        options.SlidingExpiration = true;
                                    });

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

app.UseAuthentication(); // UseAuthentication
app.UseAuthorization();

app.UseSession(); // para que funcione la sesion en todo el proyecto

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
