using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class rol_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public rol_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        // GET: /rol_general
        public async Task<IActionResult> Index()
        {
            var roles = await _db.rolGenerals
                                .OrderBy(r => r.nombreRol)
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
        public async Task<IActionResult> Create([Bind("nombreRol,descripRol")] rolGeneral? input)
        {
            if (input is null)
                return BadRequest();

            input.nombreRol = (input.nombreRol ?? string.Empty).Trim();
            input.descripRol = (input.descripRol ?? string.Empty).Trim();

            if (!ModelState.IsValid)
                return View("~/Views/rol_general/Create.cshtml", input);

            if (input.GuidRol == Guid.Empty)
                input.GuidRol = Guid.NewGuid();

            _db.rolGenerals.Add(input);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /rol_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var rol = await _db.rolGenerals
                            .FirstOrDefaultAsync(r => r.GuidRol == id.Value);
            if (rol == null) return NotFound();

            return View("~/Views/rol_general/Edit.cshtml", rol);
        }

        // POST: /rol_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidRol,nombreRol,descripRol")] rolGeneral? input)
        {
            if (input is null)
                return BadRequest();

            if (id != input.GuidRol)
                return NotFound();

            if (!ModelState.IsValid)
                return View("~/Views/rol_general/Edit.cshtml", input);

            input.nombreRol = (input.nombreRol ?? string.Empty).Trim();
            input.descripRol = (input.descripRol ?? string.Empty).Trim();

            try
            {
                _db.Entry(input).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.rolGenerals.Any(e => e.GuidRol == input.GuidRol))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /rol_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var rol = await _db.rolGenerals
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.GuidRol == id.Value);

            if (rol == null) return NotFound();

            return View("~/Views/rol_general/Delete.cshtml", rol);
        }

        // POST: /rol_general/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid GuidRol)
        {
            var rol = await _db.rolGenerals
                            .FirstOrDefaultAsync(r => r.GuidRol == GuidRol);
            if (rol == null) return NotFound();

            _db.rolGenerals.Remove(rol);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(Guid id)
        {
            return _db.rolGenerals.Any(e => e.GuidRol == id);
        }
    }
}
