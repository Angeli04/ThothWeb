using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Thoth.Web.Repositories;
using Thoth.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; 
using System.IO;                    
using System;
using Thoth.Web.Models.ViewModels;

namespace Thoth.Web.Controllers.Web
{
    [Authorize(Roles = "Administrador")]
    public class CapacitacionesController : Controller
    {
        private readonly ICapacitacionRepository _repo;
        private readonly IWebHostEnvironment _webHostEnvironment; 


        public CapacitacionesController(ICapacitacionRepository repo, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var capacitaciones = await _repo.GetAllAsync();
            return View(capacitaciones);
        }

        public IActionResult Create()
    {
        return View();
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CapacitacionViewModel model)
        {
            var extensionesPermitidas = new[] {".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx"};
            
            if (ModelState.IsValid)
            {
                string rutaArchivo = "";

                
                if (model.ArchivoMaterial != null)
                {
                    
                    var extension = Path.GetExtension(model.ArchivoMaterial.FileName).ToLowerInvariant();

                    if (!extensionesPermitidas.Contains(extension))
                    {
                        ModelState.AddModelError("ArchivoMaterial", "La extensión del archivo no es válida.");
                        return View(model);
                    }
                    

                    string carpetaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    
                    
                    if (!Directory.Exists(carpetaUploads))
                    {
                        Directory.CreateDirectory(carpetaUploads);
                    }

                    
                    string nombreUnico = Guid.NewGuid().ToString() + "_" + model.ArchivoMaterial.FileName;         
                    
                    string rutaCompleta = Path.Combine(carpetaUploads, nombreUnico);
             
                    using (var fileStream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await model.ArchivoMaterial.CopyToAsync(fileStream);
                    }
          
                    rutaArchivo = "/uploads/" + nombreUnico;
                }


                var capacitacion = new Capacitacion
                {
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    ContenidoUrl = rutaArchivo,
                    FechaCreacion = DateTime.Now
                };

                await _repo.AddAsync(capacitacion);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

 
        public async Task<IActionResult> Edit(int id)
        {
            var capacitacion = await _repo.GetByIdAsync(id);
            if (capacitacion == null) return NotFound();


            var model = new CapacitacionEditViewModel
            {
                Id = capacitacion.Id,
                Titulo = capacitacion.Titulo,
                Descripcion = capacitacion.Descripcion,
                ContenidoUrlActual = capacitacion.ContenidoUrl 
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(CapacitacionEditViewModel model)
        {
            var extensionesPermitidas = new[] {".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx"};
            if (ModelState.IsValid)
            {
                
                var extension = Path.GetExtension(model.ArchivoMaterial.FileName).ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    ModelState.AddModelError("ArchivoMaterial", "La extensión del archivo no es válida.");
                    return View(model);
                }


                var capacitacionDb = await _repo.GetByIdAsync(model.Id);
                if (capacitacionDb == null) return NotFound();

                
                capacitacionDb.Titulo = model.Titulo;
                capacitacionDb.Descripcion = model.Descripcion;

               
                if (model.ArchivoMaterial != null)
                {

                    string carpetaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string nombreUnico = Guid.NewGuid().ToString() + "_" + model.ArchivoMaterial.FileName;
                    string rutaCompleta = Path.Combine(carpetaUploads, nombreUnico);

                    using (var fileStream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await model.ArchivoMaterial.CopyToAsync(fileStream);
                    }
                    capacitacionDb.ContenidoUrl = "/uploads/" + nombreUnico;
                }

                await _repo.UpdateAsync(capacitacionDb);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    
        public async Task<IActionResult> Details(int id)
        {
            var capacitacion = await _repo.GetByIdAsync(id);
            
            if (capacitacion == null)
            {
                return NotFound();
            }

            return View(capacitacion);
        }







    }
}