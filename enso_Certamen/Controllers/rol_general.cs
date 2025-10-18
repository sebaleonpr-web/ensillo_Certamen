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
        private readonly boletinLayonContext _context;

        public rol_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: rol_general/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.rolGenerals.ToListAsync());
        }

        // GET: rol_general/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: rol_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuidRol,NombreRol,DescripRol")] rolGeneral rol_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rol_general);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(rol_general);
        }

        // GET: rol_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var rol_general = await _context.rolGenerals.FindAsync(id);
            if (rol_general == null) return NotFound();

            return View(rol_general);
        }

        // POST: rol_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidRol,NombreRol,DescripRol")] rolGeneral rol_general)
        {
            if (id != rol_general.GuidRol) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rol_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(rol_general.GuidRol)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rol_general);
        }

        // GET: rol_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var rol_general = await _context.rolGenerals.FindAsync(id);
            if (rol_general == null) return NotFound();

            return View(rol_general);
        }

        // POST: rol_general/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rol_general = await _context.rolGenerals.FindAsync(id);
            if (rol_general != null)
            {
                _context.rolGenerals.Remove(rol_general);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool Exists(Guid id)
        {
            return _context.rolGenerals.Any(e => e.GuidRol == id);
        }
    }
}
