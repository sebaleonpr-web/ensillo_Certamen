using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class suscripcion_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public suscripcion_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        // GET: /suscripcion_general
        // Tabla de suscripciones, Index.cshtml — Carga la lista ordenada por fecha desc
        public async Task<IActionResult> Index()
        {
            var lista = await _db.suscripcionGenerals
                .AsNoTracking()
                .Include(s => s.GuidBoletinNavigation) // 🔹 por si la vista muestra el título del boletín
                .OrderByDescending(s => s.fechaSuscripcion)
                .ToListAsync();

            return View("~/Views/suscripcion_general/Index.cshtml", lista);
        }

        // GET: /suscripcion_general/Create
        // Carga el combo de Boletines
        public IActionResult Create()
        {
            ViewBag.Boletines = new SelectList(
                _db.boletinGenerals
                .AsNoTracking()
                .OrderBy(b => b.TituloBoletin)
                .Select(b => new { b.GuidBoletin, Texto = b.TituloBoletin }),
                "GuidBoletin", "Texto"
            );

            return View("~/Views/suscripcion_general/Create.cshtml");
        }

        // POST: /suscripcion_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nombreSuscripcion,emailSuscripcion,fechaSuscripcion,GuidBoletin")] suscripcionGeneral model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Boletines = new SelectList(
                    _db.boletinGenerals
                    .AsNoTracking()
                    .OrderBy(b => b.TituloBoletin)
                    .Select(b => new { b.GuidBoletin, Texto = b.TituloBoletin }),
                    "GuidBoletin", "Texto", model.GuidBoletin
                );

                return View("~/Views/suscripcion_general/Create.cshtml", model);
            }

            if (model.GuidSuscripcion == Guid.Empty)
                model.GuidSuscripcion = Guid.NewGuid();

            _db.suscripcionGenerals.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /suscripcion_general/Edit/{id}
        // Recibe el id de la suscripción a editar
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.suscripcionGenerals.FindAsync(id.Value);
            if (entidad == null) return NotFound();

            ViewBag.Boletines = new SelectList(
                _db.boletinGenerals
                .AsNoTracking()
                .OrderBy(b => b.TituloBoletin)
                .Select(b => new { b.GuidBoletin, Texto = b.TituloBoletin }),
                "GuidBoletin", "Texto", entidad.GuidBoletin
            );

            return View("~/Views/suscripcion_general/Edit.cshtml", entidad);
        }

        // POST: /suscripcion_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidSuscripcion,nombreSuscripcion,emailSuscripcion,fechaSuscripcion,GuidBoletin")] suscripcionGeneral model)
        {
            if (id != model.GuidSuscripcion) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Boletines = new SelectList(
                    _db.boletinGenerals
                    .AsNoTracking()
                    .OrderBy(b => b.TituloBoletin)
                    .Select(b => new { b.GuidBoletin, Texto = b.TituloBoletin }),
                    "GuidBoletin", "Texto", model.GuidBoletin
                );

                return View("~/Views/suscripcion_general/Edit.cshtml", model);
            }

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.suscripcionGenerals.AnyAsync(s => s.GuidSuscripcion == model.GuidSuscripcion);
                if (!existe) return NotFound();
                throw; // mismo criterio que tu boletin_generalController
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /suscripcion_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.suscripcionGenerals
                .AsNoTracking()
                .Include(s => s.GuidBoletinNavigation)
                .FirstOrDefaultAsync(s => s.GuidSuscripcion == id.Value);

            if (entidad == null) return NotFound();

            return View("~/Views/suscripcion_general/Delete.cshtml", entidad);
        }

        // POST: /suscripcion_general/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entidad = await _db.suscripcionGenerals.FindAsync(id);
            if (entidad != null)
            {
                _db.suscripcionGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
