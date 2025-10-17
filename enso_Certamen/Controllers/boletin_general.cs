using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace boletin_general.Controllers
{
    public class boletin_generalController : Controller
    {
        private readonly boletinLayonContext _context;

        public boletin_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: boletin_general/Index
        public async Task<IActionResult> Index()
        {
            // Si quieres ver el nombre de la noticia en la tabla, puedes incluir la navegación:
            // var lista = await _context.BoletinGenerals.Include(b => b.IdNoticiaNavigation).ToListAsync();
            // return View(lista);
            return View(await _context.BoletinGenerals.ToListAsync());
        }

        // GET: boletin_general/Create
        public IActionResult Create()
        {
            ViewBag.IdNoticia = new SelectList(_context.NoticiaGenerals, "IdNoticia", "TituloNoticia");
            return View();
        }

        // POST: boletin_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBoletin,TituloBoletin,DescripcionBoletin,FechaBoletin,IdNoticia")] BoletinGeneral boletin_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boletin_general);
                await _context.SaveChangesAsync();

                TempData["Ok"] = "Boletín creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdNoticia = new SelectList(_context.NoticiaGenerals, "IdNoticia", "TituloNoticia", boletin_general.IdNoticia);
            return View(boletin_general);
        }

        // GET: boletin_general/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var boletin_general = await _context.BoletinGenerals.FindAsync(id);
            if (boletin_general == null) return NotFound();

            ViewBag.IdNoticia = new SelectList(_context.NoticiaGenerals, "IdNoticia", "TituloNoticia", boletin_general.IdNoticia);
            return View(boletin_general);
        }

        // POST: boletin_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBoletin,TituloBoletin,DescripcionBoletin,FechaBoletin,IdNoticia")] BoletinGeneral boletin_general)
        {
            if (id != boletin_general.IdBoletin) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boletin_general);
                    await _context.SaveChangesAsync();
                    TempData["Ok"] = "Boletín actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(boletin_general.IdBoletin)) return NotFound();
                    throw;
                }
            }

            ViewBag.IdNoticia = new SelectList(_context.NoticiaGenerals, "IdNoticia", "TituloNoticia", boletin_general.IdNoticia);
            return View(boletin_general);
        }

        // GET: boletin_general/Delete/5  -> muestra confirmación
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var boletin = await _context.BoletinGenerals
                .Include(b => b.IdNoticiaNavigation)
                .FirstOrDefaultAsync(m => m.IdBoletin == id);

            if (boletin == null) return NotFound();

            return View(boletin);
        }

        // POST: boletin_general/Delete/5  -> elimina
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boletin = await _context.BoletinGenerals.FindAsync(id);
            if (boletin != null)
            {
                _context.BoletinGenerals.Remove(boletin);
                await _context.SaveChangesAsync();
                TempData["Ok"] = "Boletín eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper
        private bool Exists(int id)
        {
            return _context.BoletinGenerals.Any(e => e.IdBoletin == id);
        }
    }
}
