using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // para Any()

namespace noticia_general.Controllers
{
    public class noticia_generalController : Controller
    {
        private readonly boletinLayonContext _context;

        public noticia_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: noticia_general/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.NoticiaGenerals.ToListAsync());
        }

        // GET: noticia_general/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: noticia_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNoticia,TituloNoticia,ResumenNoticia,ContenidoNoticia,FechaNoticia,IdUser")]
        NoticiaGeneral noticia_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noticia_general);
                await _context.SaveChangesAsync();
                // Mantengo tu patrón anterior: volver a Create
                return RedirectToAction(nameof(Create));
                // Si prefieres listar: return RedirectToAction(nameof(Index));
            }
            return View(noticia_general);
        }

        // GET: noticia_general/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var noticia_general = await _context.NoticiaGenerals.FindAsync(id);
            if (noticia_general == null) return NotFound();

            return View(noticia_general);
        }

        // POST: noticia_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNoticia,TituloNoticia,ResumenNoticia,ContenidoNoticia,FechaNoticia,IdUser")]
        NoticiaGeneral noticia_general)
        {
            if (id != noticia_general.IdNoticia) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noticia_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(noticia_general.IdNoticia))
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
            return View(noticia_general);
        }

        // Helper usado en el catch
        private bool Exists(int id)
        {
            return _context.NoticiaGenerals.Any(e => e.IdNoticia == id);
        }
    }
}
