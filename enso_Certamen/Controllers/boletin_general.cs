using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class boletin_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public boletin_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        // GET: /boletin_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.BoletinGenerals
                                .OrderByDescending(b => b.FechaBoletin)
                                .ToListAsync();

            return View("~/Views/boletin_general/Index.cshtml", lista);
        }

        // GET: /boletin_general/Create
        public IActionResult Create()
        {
            ViewBag.Noticias = new SelectList(
                _db.NoticiaGenerals
                .OrderBy(n => n.TituloNoticia)
                .Select(n => new { n.IdNoticia, n.TituloNoticia }),
                "IdNoticia", "TituloNoticia"
            );

            return View("~/Views/boletin_general/Create.cshtml");
        }

        // POST: /boletin_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TituloBoletin,DescripcionBoletin,FechaBoletin,IdNoticia")] BoletinGeneral model)
        {
            var min = new DateTime(1753, 1, 1);
            var max = new DateTime(9999, 12, 31);
            if (model.FechaBoletin < min || model.FechaBoletin > max)
            {
                ModelState.AddModelError("FechaBoletin", $"La fecha debe estar entre {min.ToShortDateString()} y {max.ToShortDateString()}.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.NoticiaGenerals
                    .OrderBy(n => n.TituloNoticia)
                    .Select(n => new { n.IdNoticia, n.TituloNoticia }),
                    "IdNoticia", "TituloNoticia", model.IdNoticia
                );
                return View("~/Views/boletin_general/Create.cshtml", model);
            }

            // El ID ahora se genera automáticamente por la BD (IDENTITY)
            _db.BoletinGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /boletin_general/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.BoletinGenerals.FindAsync(id);
            if (entidad == null) return NotFound();

            ViewBag.Noticias = new SelectList(
                _db.NoticiaGenerals
                .OrderBy(n => n.TituloNoticia)
                .Select(n => new { n.IdNoticia, n.TituloNoticia }),
                "IdNoticia", "TituloNoticia", entidad.IdNoticia
            );

            return View("~/Views/boletin_general/Edit.cshtml", entidad);
        }

        // POST: /boletin_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBoletin,TituloBoletin,DescripcionBoletin,FechaBoletin,IdNoticia")] BoletinGeneral model)
        {

            var min = new DateTime(1753, 1, 1);
            var max = new DateTime(9999, 12, 31);
            if (model.FechaBoletin < min || model.FechaBoletin > max)
            {
                ModelState.AddModelError("FechaBoletin", $"La fecha debe estar entre {min.ToShortDateString()} y {max.ToShortDateString()}.");
            }

            if (id != model.IdBoletin) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.NoticiaGenerals
                    .OrderBy(n => n.TituloNoticia)
                    .Select(n => new { n.IdNoticia, n.TituloNoticia }),
                    "IdNoticia", "TituloNoticia", model.IdNoticia
                );
                return View("~/Views/boletin_general/Edit.cshtml", model);
            }

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.BoletinGenerals.AnyAsync(b => b.IdBoletin == model.IdBoletin);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /boletin_general/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.BoletinGenerals.FindAsync(id);
            if (entidad == null) return NotFound();

            return View("~/Views/boletin_general/Delete.cshtml", entidad);
        }

        // POST: /boletin_general/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _db.BoletinGenerals.FindAsync(id);
            if (entidad != null)
            {
                _db.BoletinGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
