using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // necesario para Any()

namespace rol_general.Controllers
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
            return View(await _context.RolGenerals.ToListAsync());
        }

        // GET: rol_general/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: rol_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,NombreRol,DescripRol")]
        RolGeneral rol_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rol_general);
                await _context.SaveChangesAsync();
                // Mantengo tu estilo: redirigir a Create
                return RedirectToAction(nameof(Create));
                // Si prefieres listar: return RedirectToAction(nameof(Index));
            }
            return View(rol_general);
        }

        // GET: rol_general/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var rol_general = await _context.RolGenerals.FindAsync(id);
            if (rol_general == null) return NotFound();

            return View(rol_general);
        }

        // POST: rol_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,NombreRol,DescripRol")]
        RolGeneral rol_general)
        {
            if (id != rol_general.IdRol) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rol_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(rol_general.IdRol))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rol_general);
        }

        // Helper usado en el catch
        private bool Exists(int id)
        {
            return _context.RolGenerals.Any(e => e.IdRol == id);
        }
    }
}
