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
    public class comentario_generalController : Controller
    {
        private readonly BoletinLayonContext _db;

        public comentario_generalController(BoletinLayonContext db)
        {
            _db = db;
        }

       // Helper
        private void ValidarFecha(DateTime? fecha, string keyField = "fechaComentario")
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

        
        // GET: /comentario_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.ComentarioGenerals
                .AsNoTracking()
                .Include(c => c.GuidNoticiaNavigation)
                .OrderByDescending(c => c.FechaComentario)
                .ToListAsync();

            return View("~/Views/comentario_general/Index.cshtml", lista);
        }

        // GET: /comentario_general/Create
        public IActionResult Create()
        {
            ViewBag.Noticias = new SelectList(
                _db.NoticiaGenerals
                .AsNoTracking()
                .OrderBy(n => n.TituloNoticia)
                .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                "GuidNoticia", "Texto"
            );

            return View("~/Views/comentario_general/Create.cshtml");
        }

        // POST: /comentario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nombrelectorComentario,emailLectorComentario,contenidoComentario,fechaComentario,GuidNoticia")] ComentarioGeneral model)
        {
            // 👇 Normaliza el combo: Guid.Empty => null
            if (!model.GuidNoticia.HasValue || model.GuidNoticia.Value == Guid.Empty)
                model.GuidNoticia = null;

            ValidarFecha(model.FechaComentario, nameof(model.FechaComentario));
            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.NoticiaGenerals.AsNoTracking()
                        .OrderBy(n => n.TituloNoticia)
                        .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                    "GuidNoticia", "Texto", model.GuidNoticia
                );
                return View("~/Views/comentario_general/Create.cshtml", model);
            }

            if (model.GuidComentario == Guid.Empty)
                model.GuidComentario = Guid.NewGuid();

            _db.ComentarioGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: /comentario_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.ComentarioGenerals.FindAsync(id.Value);
            if (entidad == null) return NotFound();

            ViewBag.Noticias = new SelectList(
                _db.NoticiaGenerals
                .AsNoTracking()
                .OrderBy(n => n.TituloNoticia)
                .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                "GuidNoticia", "Texto", entidad.GuidNoticia
            );

            return View("~/Views/comentario_general/Edit.cshtml", entidad);
        }

        // POST: /comentario_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidComentario,nombrelectorComentario,emailLectorComentario,contenidoComentario,fechaComentario,GuidNoticia")] ComentarioGeneral model)
        {
            ValidarFecha(model.FechaComentario, nameof(model.FechaComentario));

            if (id != model.GuidComentario) return NotFound();
            ValidarFecha(model.FechaComentario);
            // 👇 Igual que en Create
            if (!model.GuidNoticia.HasValue || model.GuidNoticia.Value == Guid.Empty)
                model.GuidNoticia = null;
            if (!ModelState.IsValid)
            {
                ViewBag.Noticias = new SelectList(
                    _db.NoticiaGenerals.AsNoTracking()
                        .OrderBy(n => n.TituloNoticia)
                        .Select(n => new { n.GuidNoticia, Texto = n.TituloNoticia }),
                    "GuidNoticia", "Texto", model.GuidNoticia
                );
                return View("~/Views/comentario_general/Edit.cshtml", model);
            }

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.ComentarioGenerals.AnyAsync(c => c.GuidComentario == model.GuidComentario);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: /comentario_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.ComentarioGenerals
                .AsNoTracking()
                .Include(c => c.GuidNoticiaNavigation)
                .FirstOrDefaultAsync(c => c.GuidComentario == id.Value);

            if (entidad == null) return NotFound();

            return View("~/Views/comentario_general/Delete.cshtml", entidad);
        }

        // POST: /comentario_general/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entidad = await _db.ComentarioGenerals.FindAsync(id);
            if (entidad != null)
            {
                _db.ComentarioGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
