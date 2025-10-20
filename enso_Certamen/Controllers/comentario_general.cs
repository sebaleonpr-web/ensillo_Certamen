using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class comentario_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public comentario_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        // GET: /comentario_general
        // Tabla de comentarios — ordenados por fecha desc
        public async Task<IActionResult> Index()
        {
            var lista = await _db.comentarioGenerals
                .AsNoTracking()
                .Include(c => c.GuidNoticiaNavigation) // 🔹 por si la vista muestra el título de la noticia
                .OrderByDescending(c => c.fechaComentario)
                .ToListAsync();

            return View("~/Views/comentario_general/Index.cshtml", lista);
        }

        // GET: /comentario_general/Create
        public IActionResult Create()
        {
            CargarSelectNoticias();
            return View("~/Views/comentario_general/Create.cshtml");
        }

        // POST: /comentario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nombrelectorComentario,emailLectorComentario,contenidoComentario,fechaComentario,GuidNoticia")] comentarioGeneral model)
        {
            // Validación simple de rango de año (opcional)
            if (model.fechaComentario.Year < 1900 || model.fechaComentario.Year > 2100)
            {
                ModelState.AddModelError(nameof(model.fechaComentario), "La fecha es inválida. Usa un año razonable (1900–2100).");
            }

            if (!ModelState.IsValid)
            {
                CargarSelectNoticias(model.GuidNoticia);
                return View("~/Views/comentario_general/Create.cshtml", model);
            }

            if (model.GuidComentario == Guid.Empty)
                model.GuidComentario = Guid.NewGuid();

            _db.comentarioGenerals.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /comentario_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.comentarioGenerals.FindAsync(id.Value);
            if (entidad == null) return NotFound();

            CargarSelectNoticias(entidad.GuidNoticia);
            return View("~/Views/comentario_general/Edit.cshtml", entidad);
        }

        // POST: /comentario_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidComentario,nombrelectorComentario,emailLectorComentario,contenidoComentario,fechaComentario,GuidNoticia")] comentarioGeneral model)
        {
            if (id != model.GuidComentario) return NotFound();

            if (model.fechaComentario.Year < 1900 || model.fechaComentario.Year > 2100)
            {
                ModelState.AddModelError(nameof(model.fechaComentario), "La fecha es inválida. Usa un año razonable (1900–2100).");
            }

            if (!ModelState.IsValid)
            {
                CargarSelectNoticias(model.GuidNoticia);
                return View("~/Views/comentario_general/Edit.cshtml", model);
            }

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.comentarioGenerals.AnyAsync(c => c.GuidComentario == model.GuidComentario);
                if (!existe) return NotFound();
                throw; // mismo criterio que usas en boletin_general
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /comentario_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.comentarioGenerals
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
            var entidad = await _db.comentarioGenerals.FindAsync(id);
            if (entidad != null)
            {
                _db.comentarioGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ======================
        // Helpers
        // ======================
        private void CargarSelectNoticias(Guid? seleccion = null)
        {
            var noticias = _db.noticiaGenerals
                .AsNoTracking()
                .OrderBy(n => n.tituloNoticia)
                .Select(n => new
                {
                    n.GuidNoticia,
                    Texto = n.tituloNoticia
                })
                .ToList();

            ViewBag.Noticias = new SelectList(noticias, "GuidNoticia", "Texto", seleccion);
        }
    }
}
