using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // ¡Importante!

namespace Thoth.Web.Controllers.Web
{
    [Authorize] // <-- 1. ¡CLAVE! Solo usuarios logueados pueden entrar aquí.
    public class DashboardController : Controller
    {
        // Esta será tu página de inicio (después del login)
        public IActionResult Index()
        {
            // 2. Por ahora solo muestra una vista
            return View();
        }
    }
}