using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var lista = await _db.comentarioGenerals
                                 .OrderByDescending(c => c.fechaComentario)
                                 .ToListAsync();
            return View("~/Views/comentario_general/Index.cshtml", lista);
        }

        // GET: /comentario_general/Create
        public IActionResult Create()
        {
            return View("~/Views/comentario_general/Create.cshtml");
        }

        // POST: /comentario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(comentarioGeneral model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/comentario_general/Create.cshtml", model);

            _db.comentarioGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /comentario_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.comentarioGenerals.FindAsync(id);
            if (entidad == null) return NotFound();
            return View("~/Views/comentario_general/Edit.cshtml", entidad);
        }

        // POST: /comentario_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, comentarioGeneral model)
        {
            if (id != model.GuidComentario) return NotFound();

            if (!ModelState.IsValid)
                return View("~/Views/comentario_general/Edit.cshtml", model);

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.comentarioGenerals.AnyAsync(e => e.GuidComentario == id);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /comentario_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.comentarioGenerals.FindAsync(id);
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
    }
}
