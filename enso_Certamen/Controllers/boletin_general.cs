using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // <-- necesario para Any()

namespace boletin_general.Controllers
{
    public class boletin_generalController : Controller
    {
        private readonly boletinLayonContext _context;

        public boletin_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: boletin_general
        public IActionResult Create()
        {
            return View();
        }

        // Agregado: Index para que el RedirectToAction(nameof(Index)) funcione
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoletinGenerals.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBoletin ,TituloBoletin ,DescripcionBoletin ,FechaBoletin ,IdNoticia")]
        BoletinGeneral boletin_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boletin_general);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(boletin_general);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var boletin_general = await _context.BoletinGenerals.FindAsync(id);
            if (boletin_general == null) return NotFound();

            return View(boletin_general);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBoletin ,TituloBoletin ,DescripcionBoletin ,FechaBoletin ,IdNoticia")]
        BoletinGeneral boletin_general)
        {
            if (id != boletin_general.IdBoletin) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boletin_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(boletin_general.IdBoletin))
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
            return View(boletin_general); // <-- corregido (antes decía cliente)
        }

        // Agregado: helper Exists que usas en el catch
        private bool Exists(int id)
        {
            return _context.BoletinGenerals.Any(e => e.IdBoletin == id);
        }
    }
}
