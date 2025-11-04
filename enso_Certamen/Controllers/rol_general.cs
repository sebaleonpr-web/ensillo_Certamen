using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;
using enso_Certamen.Data;

namespace enso_Certamen.Controllers
{
    public class rol_generalController : Controller
    {
        private readonly BoletinLayonContext _db;

        public rol_generalController(BoletinLayonContext db)
        {
            _db = db;
        }

        // GET: /rol_general
        public async Task<IActionResult> Index()
        {
            var roles = await _db.RolGenerals
                .AsNoTracking()
                .OrderBy(r => r.NombreRol)
                .ToListAsync();

            return View("~/Views/rol_general/Index.cshtml", roles);
        }

        // GET: /rol_general/Create
        public IActionResult Create()
        {
            return View("~/Views/rol_general/Create.cshtml");
        }

        // POST: /rol_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nombreRol,descripRol")] RolGeneral? input)
        {
            if (input is null)
                return BadRequest();

            input.NombreRol = (input.NombreRol ?? string.Empty).Trim();
            input.DescripRol = (input.DescripRol ?? string.Empty).Trim();

            // Generar Guid antes de validar/guardar
            if (input.GuidRol == Guid.Empty)
                input.GuidRol = Guid.NewGuid();

            if (!ModelState.IsValid)
                return View("~/Views/rol_general/Create.cshtml", input);

            _db.RolGenerals.Add(input);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /rol_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var rol = await _db.RolGenerals
                .FirstOrDefaultAsync(r => r.GuidRol == id.Value);
            if (rol == null) return NotFound();

            return View("~/Views/rol_general/Edit.cshtml", rol);
        }

        // POST: /rol_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidRol,nombreRol,descripRol")] RolGeneral? input)
        {
            if (input is null)
                return BadRequest();

            if (id != input.GuidRol)
                return NotFound();

            input.NombreRol = (input.NombreRol ?? string.Empty).Trim();
            input.DescripRol = (input.DescripRol ?? string.Empty).Trim();

            if (!ModelState.IsValid)
                return View("~/Views/rol_general/Edit.cshtml", input);

            try
            {
                _db.Entry(input).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.RolGenerals.Any(e => e.GuidRol == input.GuidRol))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /rol_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var rol = await _db.RolGenerals
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.GuidRol == id.Value);

            if (rol == null) return NotFound();

            return View("~/Views/rol_general/Delete.cshtml", rol);
        }

        // POST: /rol_general/Delete  (reasigna usuarios; si no hay otro rol, crea uno temporal)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid GuidRol)
        {
            var rol = await _db.RolGenerals
                .FirstOrDefaultAsync(r => r.GuidRol == GuidRol);
            if (rol == null) return NotFound();

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1) Obtener usuarios dependientes
                var dependientes = await _db.UsuariosGenerals
                    .Where(u => u.GuidRol == GuidRol)
                    .ToListAsync();

                if (dependientes.Count > 0)
                {
                    // 2) Buscar un rol alternativo distinto
                    var rolAlternativo = await _db.RolGenerals
                        .AsNoTracking()
                        .Where(r => r.GuidRol != GuidRol)
                        .OrderBy(r => r.NombreRol)
                        .FirstOrDefaultAsync();

                    // 3) Si no existe, crear uno temporal
                    if (rolAlternativo == null)
                    {
                        var rolTemporal = new RolGeneral
                        {
                            GuidRol = Guid.NewGuid(),
                            NombreRol = "Rol Temporal",
                            DescripRol = "Generado automáticamente para reasignación"
                        };

                        _db.RolGenerals.Add(rolTemporal);
                        await _db.SaveChangesAsync(); // Persistir para obtener Guid válido
                        rolAlternativo = rolTemporal;
                    }

                    // 4) Reasignar todos los usuarios al rol alternativo
                    foreach (var u in dependientes)
                        u.GuidRol = rolAlternativo.GuidRol;

                    await _db.SaveChangesAsync();
                }

                // 5) Eliminar el rol original
                _db.RolGenerals.Remove(rol);
                await _db.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                ModelState.AddModelError(string.Empty, "No se pudo eliminar el rol.");
                return View("~/Views/rol_general/Delete.cshtml", rol);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(Guid id)
        {
            return _db.RolGenerals.Any(e => e.GuidRol == id);
        }
    }
}
