using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Thoth.Web.Data;
using Thoth.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<ThothDbContext>(options =>
{

    options.UseMySql(connectionString, 
        ServerVersion.AutoDetect(connectionString), 
        mySqlOptions => mySqlOptions.EnableRetryOnFailure() 
    );
});


builder.Services.AddControllersWithViews(); 


builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // 2. Definir la página de Login
        // Si un usuario NO autenticado intenta entrar a una página [Authorize],
        // será redirigido aquí:
        options.LoginPath = "/Account/Login";
        
        // 3. (Opcional) Tiempo de expiración de la cookie
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
        
        // 4. (Opcional) Permitir que la cookie se refresque
        options.SlidingExpiration = true; 
    });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // <-- ¿Quién eres?
app.UseAuthorization();  // <-- ¿Tienes permiso?

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
app.Run();