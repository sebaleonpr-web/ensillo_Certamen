using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // -------------------------- LISTA --------------------------
        // GET: /usuario_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.usuariosGenerals
                                .AsNoTracking()
                                .Include(u => u.GuidRolNavigation) // si quieres mostrar el nombre del rol
                                .OrderBy(u => u.GuidUsuario)
                                .ToListAsync();

            return View("~/Views/usuario_general/Index.cshtml", lista);
        }

        // ----------------------- HELPER ROLES ----------------------
        // Carga ViewBag.Roles como IEnumerable<SelectListItem> (nunca null)
        private async Task CargarRolesAsync(Guid? seleccionado = null)
        {
            var lista = await _db.rolGenerals
                                .AsNoTracking()
                                .OrderBy(r => r.nombreRol)
                                .Select(r => new SelectListItem
                                {
                                    Value    = r.GuidRol.ToString(),
                                    Text     = r.nombreRol,
                                    Selected = (seleccionado.HasValue && r.GuidRol == seleccionado.Value)
                                })
                                .ToListAsync();

            ViewBag.Roles = lista ?? new List<SelectListItem>();
        }

        // -------------------------- CREATE -------------------------
        // GET: /usuario_general/Create
        public async Task<IActionResult> Create()
        {
            await CargarRolesAsync(); // necesario para el <select>
            return View("~/Views/usuario_general/Create.cshtml");
        }

        // POST: /usuario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(usuariosGeneral model)
        {
            if (model.GuidRol == Guid.Empty) model.GuidRol = null;
            if (model.GuidUsuario == Guid.Empty) model.GuidUsuario = Guid.NewGuid();

            // === NUEVO: exigir contraseña en Create ===
            if (string.IsNullOrWhiteSpace(model.contraUser))
            {
                ModelState.AddModelError(nameof(model.contraUser), "La contraseña es obligatoria.");
            }

            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(model.GuidRol);
                return View("~/Views/usuario_general/Create.cshtml", model);
            }

            _db.usuariosGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // --------------------------- EDIT --------------------------
        // GET: /usuario_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var entidad = await _db.usuariosGenerals.FindAsync(id);
            if (entidad == null) return NotFound();

            await CargarRolesAsync(entidad.GuidRol);
            return View("~/Views/usuario_general/Edit.cshtml", entidad);
        }

        // POST: /usuario_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, usuariosGeneral model)
        {
            if (id != model.GuidUsuario) return NotFound();

            // Cargar la entidad existente
            var entidad = await _db.usuariosGenerals
                                .FirstOrDefaultAsync(u => u.GuidUsuario == id);
            if (entidad == null) return NotFound();

            // Normaliza GuidRol
            var guidRol = (model.GuidRol == Guid.Empty) ? (Guid?)null : model.GuidRol;

            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(guidRol);
                return View("~/Views/usuario_general/Edit.cshtml", model);
            }

            // Copiar campos editables (sin tocar contraseña si viene vacía)
            entidad.nombreUser   = model.nombreUser;
            entidad.apellidoUser = model.apellidoUser;
            entidad.emailUser    = model.emailUser;
            entidad.GuidRol      = guidRol;

            if (!string.IsNullOrWhiteSpace(model.contraUser))
            {
                entidad.contraUser = model.contraUser;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // -------------------------- DELETE -------------------------
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
                // === NUEVO: desasociar noticias que referencian al usuario (evita conflicto FK) ===
                var noticias = await _db.noticiaGenerals
                                        .Where(n => n.GuidUsuario == id)
                                        .ToListAsync();
                if (noticias.Count > 0)
                {
                    foreach (var n in noticias)
                        n.GuidUsuario = null;

                    await _db.SaveChangesAsync(); // guardar primero la desasociación
                }

                _db.usuariosGenerals.Remove(entidad);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
