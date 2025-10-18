using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class usuario_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public usuario_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        // GET: /usuario_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.usuariosGenerals
                                 .OrderBy(u => u.GuidUsuario)
                                 .ToListAsync();
            return View("~/Views/usuario_general/Index.cshtml", lista);
        }

        // GET: /usuario_general/Create
        public IActionResult Create()
        {
            return View("~/Views/usuario_general/Create.cshtml");
        }

        // POST: /usuario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(usuariosGeneral model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/usuario_general/Create.cshtml", model);

            _db.usuariosGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /usuario_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.usuariosGenerals.FindAsync(id);
            if (entidad == null) return NotFound();
            return View("~/Views/usuario_general/Edit.cshtml", entidad);
        }

        // POST: /usuario_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, usuariosGeneral model)
        {
            if (id != model.GuidUsuario) return NotFound();

            if (!ModelState.IsValid)
                return View("~/Views/usuario_general/Edit.cshtml", model);

            try
            {
                _db.Update(model);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.usuariosGenerals.AnyAsync(e => e.GuidUsuario == id);
                if (!existe) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /usuario_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var entidad = await _db.usuariosGenerals.FindAsync(id);
            if (entidad == null) return NotFound();
            return View("~/Views/usuario_general/Delete.cshtml", entidad);
        }

        // POST: /usuario_general/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entidad = await _db.usuariosGenerals.FindAsync(id);
            if (entidad != null)
            {
                _db.usuariosGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
