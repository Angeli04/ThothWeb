// --- Imports necesarios para la base de datos ---
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Thoth.Web.Data; // Asegúrate que este sea el namespace de tu DbContext
// --- Fin de Imports ---

var builder = WebApplication.CreateBuilder(args);

// --- INICIO: Configuración del DbContext ---

// 1. Obtener la cadena de conexión de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Configurar el DbContext para usar MySQL
builder.Services.AddDbContext<ThothDbContext>(options =>
{
    // 3. Usar el proveedor de Pomelo para MySQL
    options.UseMySql(connectionString, 
        ServerVersion.AutoDetect(connectionString), // Detecta la versión de tu servidor MySQL
        mySqlOptions => mySqlOptions.EnableRetryOnFailure() // Opcional: bueno para reintentar conexiones fallidas
    );
});

// --- FIN: Configuración del DbContext ---


// Add services to the container.
builder.Services.AddControllersWithViews(); // Esto ya debería estar

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();