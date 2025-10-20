using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace enso_Certamen.Controllers
{
    public class suscripcion_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public suscripcion_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        private async Task CargarBoletinAsync(Guid? seleccionado = null)
        {
            var boletines = await _db.boletinGenerals
                                    .Select(b => new { b.GuidBoletin, b.TituloBoletin })
                                    .ToListAsync();

            ViewBag.Boletines = new SelectList(boletines, "GuidBoletin", "TituloBoletin", seleccionado);
        }
        // GET: /suscripcion_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.suscripcionGenerals
                                .OrderByDescending(s => s.fechaSuscripcion)
                                .ToListAsync();
            return View("~/Views/suscripcion_general/Index.cshtml", lista);
        }

        // GET: /suscripcion_general/Create
        public async Task<IActionResult> Create()
        {
            await CargarBoletinAsync();

            return View("~/Views/suscripcion_general/Create.cshtml");
        }
        // POST: /suscripcion_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nombreSuscripcion,emailSuscripcion,fechaSuscripcion,GuidBoletin")] suscripcionGeneral model)
        {
            if (!ModelState.IsValid)
            {
                await CargarBoletinAsync(model.GuidBoletin);
                return View("~/Views/suscripcion_general/Create.cshtml", model);
            }

            if (model.GuidSuscripcion == Guid.Empty)
                model.GuidSuscripcion = Guid.NewGuid();

            try
            {
                _db.suscripcionGenerals.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar la suscripción. Por favor, inténtalo de nuevo.");
                await CargarBoletinAsync(model.GuidBoletin);
                return View("~/Views/suscripcion_general/Create.cshtml", model);
            }
        }


        // GET: Editar suscripcion
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var suscripcion = await _db.suscripcionGenerals.FindAsync(id.Value);
            if (suscripcion == null) return NotFound();

            await CargarBoletinAsync(suscripcion.GuidBoletin);
            return View("~/Views/suscripcion_general/Edit.cshtml", suscripcion);
        }
        //POST /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidSuscripcion,nombreSuscriptor,emailSuscriptor,fechaSuscripcion,GuidBoletin")] suscripcionGeneral model)
        {
            if (id != model.GuidSuscripcion)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarBoletinAsync(model.GuidBoletin);
                return View("~/Views/suscripcion_general/Edit.cshtml", model);
            }

            try
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.suscripcionGenerals.AnyAsync(s => s.GuidSuscripcion == id);
                if (!existe) return NotFound();

                ModelState.AddModelError(string.Empty, "Otro proceso modificó esta suscripción. Refresca e intenta de nuevo.");
                await CargarBoletinAsync(model.GuidBoletin);
                return View("~/Views/suscripcion_general/Edit.cshtml", model);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar la suscripción. Por favor, inténtalo de nuevo.");
                await CargarBoletinAsync(model.GuidBoletin);
                return View("~/Views/suscripcion_general/Edit.cshtml", model);
            }
        }
        //GET: /suscripcion_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var suscripcion = await _db.suscripcionGenerals.FindAsync(id.Value);
            if (suscripcion == null) return NotFound();

            return View("~/Views/suscripcion_general/Delete.cshtml", suscripcion);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var suscripcion = await _db.suscripcionGenerals.FindAsync(id);
            if (suscripcion != null) return RedirectToAction(nameof(Index));

            try
            {
                _db.suscripcionGenerals.Remove(suscripcion);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar la suscripción. Por favor, inténtalo de nuevo.");
                return View("~/Views/suscripcion_general/Delete.cshtml", suscripcion);
            }
            
        }
    }
}