
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Thoth.Web.Models;
using Thoth.Web.Models.ViewModels;
using Thoth.Web.Repositories;

namespace Thoth.Web.Controllers.Web
{
    
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IRolRepository _rolRepo;


        public UsuarioController(IUsuarioRepository usuarioRepo, IRolRepository rolRepo)
        {
            _usuarioRepo = usuarioRepo;
            _rolRepo = rolRepo;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioRepo.GetAllUsersAsync();
            return View(usuarios);
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _usuarioRepo.SoftDeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            await _usuarioRepo.ActivateUserAsync(id);
            return RedirectToAction(nameof(Index));
        }


    [HttpGet]
    public async Task<IActionResult> Create()
    {

        var roles = await _rolRepo.GetAllRolesAsync();
            

        ViewData["Roles"] = roles.Select(r => new SelectListItem
        {
            Text = r.Nombre,
            Value = r.Id.ToString()
        });


        return View(new UsuarioCrearViewModel());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioCrearViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Rut = model.Rut,
                Email = model.Email,
                RolId = model.RolId,
                Avatar = "default.png"
            };

            try
            {
                await _usuarioRepo.CreateUserAsync(usuario, model.Password);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        var roles = await _rolRepo.GetAllRolesAsync();
        ViewData["Roles"] = roles.Select(r => new SelectListItem
        {
            Text = r.Nombre,
            Value = r.Id.ToString()
        });

        return View(model);
    }


   
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        
        var usuario = await _usuarioRepo.GetUserByIdAsync(id);
        if (usuario == null) return NotFound();

        
        var roles = await _rolRepo.GetAllRolesAsync();
        ViewData["Roles"] = roles.Select(r => new SelectListItem
        {
            Text = r.Nombre,
            Value = r.Id.ToString()
        });

        
        var model = new UsuarioEditarViewModel
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Rut = usuario.Rut,
            Email = usuario.Email,
            RolId = usuario.RolId
        };

        return View(model);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UsuarioEditarViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuarioDb = await _usuarioRepo.GetUserByIdAsync(model.Id);
            
            if (usuarioDb == null) return NotFound();

            usuarioDb.Nombre = model.Nombre;
            usuarioDb.Apellido = model.Apellido;
            usuarioDb.Rut = model.Rut;
            usuarioDb.Email = model.Email;
            usuarioDb.RolId = model.RolId;
            
            try
            {
                await _usuarioRepo.UpdateUserAsync(usuarioDb);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        var roles = await _rolRepo.GetAllRolesAsync();
        ViewData["Roles"] = roles.Select(r => new SelectListItem
        {
            Text = r.Nombre,
            Value = r.Id.ToString()
        });

        return View(model);
    }

}
}