using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // necesario para Any()

namespace suscripcion_general.Controllers
{
    public class suscripcion_generalController : Controller
    {
        private readonly boletinLayonContext _context;

        public suscripcion_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: suscripcion_general/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.SuscripcionGenerals.ToListAsync());
        }

        // GET: suscripcion_general/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: suscripcion_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSuscripcion,NombreSuscripcion,EmailSuscripcion,FechaSuscripcion,IdBoletin")]
        SuscripcionGeneral suscripcion_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suscripcion_general);
                await _context.SaveChangesAsync();
                // Mantengo tu patrón: volver al formulario después de crear
                return RedirectToAction(nameof(Create));
                // Si prefieres listar: return RedirectToAction(nameof(Index));
            }
            return View(suscripcion_general);
        }

        // GET: suscripcion_general/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var suscripcion_general = await _context.SuscripcionGenerals.FindAsync(id);
            if (suscripcion_general == null) return NotFound();

            return View(suscripcion_general);
        }

        // POST: suscripcion_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSuscripcion,NombreSuscripcion,EmailSuscripcion,FechaSuscripcion,IdBoletin")]
        SuscripcionGeneral suscripcion_general)
        {
            if (id != suscripcion_general.IdSuscripcion) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(suscripcion_general.IdSuscripcion))
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
            return View(suscripcion_general);
        }

        // Helper usado en el catch
        private bool Exists(int id)
        {
            return _context.SuscripcionGenerals.Any(e => e.IdSuscripcion == id);
        }
    }
}
