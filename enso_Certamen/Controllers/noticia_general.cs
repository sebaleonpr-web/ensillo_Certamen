using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class noticia_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public noticia_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        // Utilidad: carga dropdown de usuarios (usa Guid como texto si no tienes nombre/email)
        private async Task CargarUsuariosAsync(Guid? seleccionado = null)
        {
            var usuarios = await _db.usuariosGenerals
                                    .Select(u => new { u.GuidUsuario })
                                    .ToListAsync();

            ViewBag.Usuarios = new SelectList(usuarios, "GuidUsuario", "GuidUsuario", seleccionado);
        }

        // Validación simple de fecha (evita años tipo 1111)
        private void ValidarFecha(DateTime fecha, string keyField = "fechaNoticia")
        {
            if (fecha.Year < 1900 || fecha.Year > 2100)
            {
                ModelState.AddModelError(keyField, "La fecha debe estar entre 1900 y 2100.");
            }
        }

        // GET: /noticia_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.noticiaGenerals
                                .AsNoTracking()
                                .OrderByDescending(n => n.fechaNoticia)
                                .ToListAsync();

            return View("~/Views/noticia_general/Index.cshtml", lista);
        }

        // GET: /noticia_general/Create
        public async Task<IActionResult> Create()
        {
            await CargarUsuariosAsync();
            // Si quieres sugerir fecha de hoy en el form, puedes pasar un modelo con fecha hoy:
            // return View("~/Views/noticia_general/Create.cshtml", new noticiaGeneral { fechaNoticia = DateTime.Today });
            return View("~/Views/noticia_general/Create.cshtml");
        }

        // POST: /noticia_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuidNoticia,tituloNoticia,resumenNoticia,contenidoNoticia,fechaNoticia,GuidUsuario")] noticiaGeneral model)
        {
            ValidarFecha(model.fechaNoticia);

            if (!ModelState.IsValid)
            {
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Create.cshtml", model);
            }

            // Generar Guid si viene vacío (Guid default)
            if (model.GuidNoticia == Guid.Empty)
                model.GuidNoticia = Guid.NewGuid();

            try
            {
                _db.noticiaGenerals.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar la noticia. Intenta nuevamente.");
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Create.cshtml", model);
            }
        }

        // GET: /noticia_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var noticia = await _db.noticiaGenerals.FindAsync(id.Value);
            if (noticia == null) return NotFound();

            await CargarUsuariosAsync(noticia.GuidUsuario);
            return View("~/Views/noticia_general/Edit.cshtml", noticia);
        }

        // POST: /noticia_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidNoticia,tituloNoticia,resumenNoticia,contenidoNoticia,fechaNoticia,GuidUsuario")] noticiaGeneral model)
        {
            if (id != model.GuidNoticia) return NotFound();

            ValidarFecha(model.fechaNoticia);

            if (!ModelState.IsValid)
            {
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Edit.cshtml", model);
            }

            try
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.noticiaGenerals.AnyAsync(n => n.GuidNoticia == id);
                if (!existe) return NotFound();

                ModelState.AddModelError(string.Empty, "Otro proceso modificó esta noticia. Refresca e intenta de nuevo.");
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Edit.cshtml", model);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios. Intenta nuevamente.");
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Edit.cshtml", model);
            }
        }

        // GET: /noticia_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var noticia = await _db.noticiaGenerals
                                .AsNoTracking()
                                .FirstOrDefaultAsync(n => n.GuidNoticia == id.Value);
                                
            if (noticia == null) return NotFound();

            return View("~/Views/noticia_general/Delete.cshtml", noticia);
        }

        // POST: /noticia_general/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var noticia = await _db.noticiaGenerals.FindAsync(id);

            try
            {
                _db.noticiaGenerals.Remove(noticia);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Por ejemplo, si hay FK (comentarios/boletines) que bloquean el borrado
                ModelState.AddModelError(string.Empty, "No se pudo eliminar la noticia (posibles referencias).");
                return View("~/Views/noticia_general/Delete.cshtml", noticia);
            }
        }
    }
}
