using enso_Certamen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // para Any()

namespace comentario_general.Controllers
{
    public class comentario_generalController : Controller
    {
        private readonly boletinLayonContext _context;

        public comentario_generalController(boletinLayonContext context)
        {
            _context = context;
        }

        // GET: comentario_general/Index (necesario para el RedirectToAction(nameof(Index)))
        public async Task<IActionResult> Index()
        {
            return View(await _context.ComentarioGenerals.ToListAsync());
        }

        // GET: comentario_general/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: comentario_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComentario,NombrelectorComentario,EmailLectorComentario,ContenidoComentario,FechaComentario,IdNoticia")]
        ComentarioGeneral comentario_general)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comentario_general);
                await _context.SaveChangesAsync();
                // Mantengo tu patrón anterior: volver a Create
                return RedirectToAction(nameof(Create));
                // Si prefieres listar: return RedirectToAction(nameof(Index));
            }
            return View(comentario_general);
        }

        // GET: comentario_general/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var comentario_general = await _context.ComentarioGenerals.FindAsync(id);
            if (comentario_general == null) return NotFound();

            return View(comentario_general);
        }

        // POST: comentario_general/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComentario,NombrelectorComentario,EmailLectorComentario,ContenidoComentario,FechaComentario,IdNoticia")]
        ComentarioGeneral comentario_general)
        {
            if (id != comentario_general.IdComentario) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comentario_general);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(comentario_general.IdComentario))
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
            return View(comentario_general);
        }

        // Helper usado en el catch
        private bool Exists(int id)
        {
            return _context.ComentarioGenerals.Any(e => e.IdComentario == id);
        }
    }
}
