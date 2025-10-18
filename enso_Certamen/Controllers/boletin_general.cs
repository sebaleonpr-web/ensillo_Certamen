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

        public async Task<IActionResult> Index()
        {
            var lista = await _db.boletinGenerals
                                 .OrderByDescending(b => b.FechaBoletin)
                                 .ToListAsync();
            return View("~/Views/boletin_general/Index.cshtml", lista);
        }

        public IActionResult Create()
        {
            ViewBag.Noticias = new SelectList(
                _db.noticiaGenerals
                   .OrderBy(n => n.GuidNoticia)
                   .Select(n => new { n.GuidNoticia, Texto = n.GuidNoticia.ToString() }),
                "GuidNoticia", "Texto"
            );
            return View("~/Views/boletin_general/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TituloBoletin,DescripcionBoletin,FechaBoletin,GuidNoticia")] boletinGeneral model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.noticiaGenerals
                       .OrderBy(n => n.GuidNoticia)
                       .Select(n => new { n.GuidNoticia, Texto = n.GuidNoticia.ToString() }),
                    "GuidNoticia", "Texto", model.GuidNoticia
                );
                return View("~/Views/boletin_general/Create.cshtml", model);
            }

            _db.boletinGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.boletinGenerals.FindAsync(id);
            if (entidad == null) return NotFound();

            ViewBag.Noticias = new SelectList(
                _db.noticiaGenerals
                   .OrderBy(n => n.GuidNoticia)
                   .Select(n => new { n.GuidNoticia, Texto = n.GuidNoticia.ToString() }),
                "GuidNoticia", "Texto", entidad.GuidNoticia
            );

            return View("~/Views/boletin_general/Edit.cshtml", entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidBoletin,TituloBoletin,DescripcionBoletin,FechaBoletin,GuidNoticia")] boletinGeneral model)
        {
            if (id != model.GuidBoletin) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.noticiaGenerals
                       .OrderBy(n => n.GuidNoticia)
                       .Select(n => new { n.GuidNoticia, Texto = n.GuidNoticia.ToString() }),
                    "GuidNoticia", "Texto", model.GuidNoticia
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
                var existe = await _db.boletinGenerals.AnyAsync(b => b.GuidBoletin == model.GuidBoletin);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.boletinGenerals.FindAsync(id);
            if (entidad == null) return NotFound();
            return View("~/Views/boletin_general/Delete.cshtml", entidad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entidad = await _db.boletinGenerals.FindAsync(id);
            if (entidad != null)
            {
                _db.boletinGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
