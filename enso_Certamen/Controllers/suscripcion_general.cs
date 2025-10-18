using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var lista = await _db.suscripcionGenerals
                                 .OrderByDescending(s => s.fechaSuscripcion)
                                 .ToListAsync();
            return View("~/Views/suscripcion_general/Index.cshtml", lista);
        }

        // GET: /suscripcion_general/Create
        public IActionResult Create()
        {
            return View("~/Views/suscripcion_general/Create.cshtml");
        }

        // POST: /suscripcion_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(suscripcionGeneral model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/suscripcion_general/Create.cshtml", model);

            _db.suscripcionGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /suscripcion_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.suscripcionGenerals.FindAsync(id);
            if (entidad == null) return NotFound();
            return View("~/Views/suscripcion_general/Edit.cshtml", entidad);
        }

        // POST: /suscripcion_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, suscripcionGeneral model)
        {
            if (id != model.GuidSuscripcion) return NotFound();

            if (!ModelState.IsValid)
                return View("~/Views/suscripcion_general/Edit.cshtml", model);

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.suscripcionGenerals.AnyAsync(e => e.GuidSuscripcion == id);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /suscripcion_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.suscripcionGenerals.FindAsync(id);
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
