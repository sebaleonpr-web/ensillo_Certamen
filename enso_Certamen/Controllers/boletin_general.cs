using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;
using enso_Certamen.Data;

namespace enso_Certamen.Controllers
{
    public class boletin_generalController : Controller
    {
        private readonly BoletinLayonContext _db;

        public boletin_generalController(BoletinLayonContext db)
        {
            _db = db;
        }

        private void ValidarFecha(DateTime? fecha, string keyField = nameof(BoletinGeneral.FechaBoletin))
        {
            if (!fecha.HasValue)
            {
                ModelState.AddModelError(keyField, "La fecha es obligatoria.");
                return;
            }

            var f = fecha.Value.Date;
            if (f.Year < 1900 || f.Year > 2100)
                ModelState.AddModelError(keyField, "La fecha debe estar entre 1900 y 2100.");
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _db.BoletinGenerals
                .AsNoTracking()
                .Include(b => b.GuidNoticiaNavigation)
                .OrderByDescending(b => b.FechaBoletin)
                .ToListAsync();

            return View("~/Views/boletin_general/Index.cshtml", lista);
        }

        public IActionResult Create()
        {
            ViewBag.Noticias = new SelectList(
                _db.NoticiaGenerals
                   .AsNoTracking()
                   .OrderBy(n => n.TituloNoticia)
                   .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                "GuidNoticia", "Texto"
            );
            return View("~/Views/boletin_general/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TituloBoletin,DescripcionBoletin,FechaBoletin,GuidNoticia")] BoletinGeneral model)
        {
            if (model.GuidBoletin == Guid.Empty)
                model.GuidBoletin = Guid.NewGuid();

            if (model.FechaBoletin == default)
                model.FechaBoletin = DateTime.Today;

            ValidarFecha(model.FechaBoletin, nameof(model.FechaBoletin));

            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.NoticiaGenerals
                       .AsNoTracking()
                       .OrderBy(n => n.TituloNoticia)
                       .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                    "GuidNoticia", "Texto", model.GuidNoticia
                );
                return View("~/Views/boletin_general/Create.cshtml", model);
            }

            _db.BoletinGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.BoletinGenerals.FindAsync(id);
            if (entidad == null) return NotFound();

            ViewBag.Noticias = new SelectList(
                _db.NoticiaGenerals
                   .AsNoTracking()
                   .OrderBy(n => n.TituloNoticia)
                   .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                "GuidNoticia", "Texto", entidad.GuidNoticia
            );

            return View("~/Views/boletin_general/Edit.cshtml", entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidBoletin,TituloBoletin,DescripcionBoletin,FechaBoletin,GuidNoticia")] BoletinGeneral model)
        {
            if (id != model.GuidBoletin) return NotFound();

            ValidarFecha(model.FechaBoletin, nameof(model.FechaBoletin));

            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.NoticiaGenerals
                       .AsNoTracking()
                       .OrderBy(n => n.TituloNoticia)
                       .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
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
                var existe = await _db.BoletinGenerals.AnyAsync(b => b.GuidBoletin == model.GuidBoletin);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.BoletinGenerals
                .Include(b => b.GuidNoticiaNavigation)
                .FirstOrDefaultAsync(b => b.GuidBoletin == id);

            if (entidad == null) return NotFound();

            return View("~/Views/boletin_general/Delete.cshtml", entidad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entidad = await _db.BoletinGenerals
                .Include(b => b.GuidNoticiaNavigation)
                .FirstOrDefaultAsync(b => b.GuidBoletin == id);

            if (entidad != null)
            {
                _db.BoletinGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
