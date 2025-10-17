using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // para Any()

namespace usuario_general.Controllers
{
    public class usuario_generalController : Controller
    {
        private readonly boletinLayonContext _context;

        public usuario_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: usuario_general/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsuariosGenerals.ToListAsync());
        }

        // GET: usuario_general/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: usuario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,ContraUser,NombreUser,ApellidoUser,EmailUser,IdRol")]
        UsuariosGeneral usuario_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario_general);
                await _context.SaveChangesAsync();
                // Mantenemos tu patrón: volver al formulario
                return RedirectToAction(nameof(Create));
                // Si prefieres listar:
                // return RedirectToAction(nameof(Index));
            }
            return View(usuario_general);
        }

        // GET: usuario_general/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var usuario_general = await _context.UsuariosGenerals.FindAsync(id);
            if (usuario_general == null) return NotFound();

            return View(usuario_general);
        }

        // POST: usuario_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUser,ContraUser,NombreUser,ApellidoUser,EmailUser,IdRol")]
        UsuariosGeneral usuario_general)
        {
            if (id != usuario_general.IdUser) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(usuario_general.IdUser))
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
            return View(usuario_general);
        }

        // Helper usado en el catch
        private bool Exists(int id)
        {
            return _context.UsuariosGenerals.Any(e => e.IdUser == id);
        }
    }
}
