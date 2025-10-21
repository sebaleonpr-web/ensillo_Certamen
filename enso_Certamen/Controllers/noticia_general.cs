using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class noticia_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public noticia_generalController(boletinLayonContext db)
        {
            _db = db;
        }

        private void ValidarFecha(DateTime? fecha, string keyField = "fechaComentario")
        {
            if (!fecha.HasValue)
            {
                ModelState.AddModelError(keyField, "La fecha es obligatoria.");
                return;
            }

            var f = fecha.Value.Date;
            if (f.Year < 1900 || f.Year > 2100)
                ModelState.AddModelError(keyField, "La fecha debe estar entre 1900 y 2100.");
        }
        // ---------- Helpers ----------
        private async Task CargarUsuariosAsync(Guid? seleccionado = null)
        {
            var items = await _db.usuariosGenerals
                                .AsNoTracking()
                                .OrderBy(u => u.GuidUsuario)
                                .Select(u => new SelectListItem
                                {
                                    Value = u.GuidUsuario.ToString(),
                                    Text = u.nombreUser,
                                    Selected = seleccionado.HasValue && u.GuidUsuario == seleccionado.Value
                                })
                                .ToListAsync();

            ViewBag.Usuarios = items ?? new List<SelectListItem>(); // nunca null
        }

        // ---------- Index ----------
        // GET: /noticia_general
        public async Task<IActionResult> Index()
        {
            var lista = await _db.noticiaGenerals
                                .AsNoTracking()
                                .Include(n => n.GuidUsuarioNavigation)
                                .OrderByDescending(n => n.fechaNoticia)
                                .ToListAsync();

            return View("~/Views/noticia_general/Index.cshtml", lista);
        }

        // ---------- Create ----------
        // GET: /noticia_general/Create
        public async Task<IActionResult> Create()
        {
            await CargarUsuariosAsync();
            return View("~/Views/noticia_general/Create.cshtml");
        }

        // POST: /noticia_general/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuidNoticia,tituloNoticia,resumenNoticia,contenidoNoticia,fechaNoticia,GuidUsuario")] noticiaGeneral model)
        {
            // Normalizaciones
            if (model.GuidUsuario == Guid.Empty) model.GuidUsuario = null; // si es opcional en tu modelo/DB
            if (model.GuidNoticia == Guid.Empty) model.GuidNoticia = Guid.NewGuid();
            if (model.fechaNoticia == default)   model.fechaNoticia = DateTime.Today;

            ValidarFecha(model.fechaNoticia, nameof(model.fechaNoticia));

            if (!ModelState.IsValid)
            {
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Create.cshtml", model);
            }

            try
            {
                _db.noticiaGenerals.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar la noticia. Intenta nuevamente.");
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Create.cshtml", model);
            }
        }

        // ---------- Edit ----------
        // GET: /noticia_general/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var noticia = await _db.noticiaGenerals.FindAsync(id.Value);
            if (noticia == null) return NotFound();

            await CargarUsuariosAsync(noticia.GuidUsuario);
            return View("~/Views/noticia_general/Edit.cshtml", noticia);
        }

        // POST: /noticia_general/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidNoticia,tituloNoticia,resumenNoticia,contenidoNoticia,fechaNoticia,GuidUsuario")] noticiaGeneral model)
        {
            if (id != model.GuidNoticia) return NotFound();

            if (model.GuidUsuario == Guid.Empty) model.GuidUsuario = null; // opcional
            if (model.fechaNoticia == default)   model.fechaNoticia = DateTime.Today;

            ValidarFecha(model.fechaNoticia, nameof(model.fechaNoticia));

            if (!ModelState.IsValid)
            {
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Edit.cshtml", model);
            }

            try
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                var existe = await _db.noticiaGenerals.AnyAsync(n => n.GuidNoticia == id);
                if (!existe) return NotFound();

                ModelState.AddModelError(string.Empty, "Otro proceso modificó esta noticia. Refresca e intenta de nuevo.");
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Edit.cshtml", model);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios. Intenta nuevamente.");
                await CargarUsuariosAsync(model.GuidUsuario);
                return View("~/Views/noticia_general/Edit.cshtml", model);
            }
        }

        // ---------- Delete ----------
        // GET: /noticia_general/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var noticia = await _db.noticiaGenerals
                                .Include(n => n.GuidUsuarioNavigation)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(n => n.GuidNoticia == id.Value);
            if (noticia == null) return NotFound();

            return View("~/Views/noticia_general/Delete.cshtml", noticia);
        }

        // POST: /noticia_general/Delete/{id}
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(Guid id)
{
    var noticia = await _db.noticiaGenerals
                        .Include(n => n.boletinGenerals)
                        .Include(n => n.comentarioGenerals)
                        .FirstOrDefaultAsync(n => n.GuidNoticia == id);

    if (noticia == null) return NotFound();

    using var tx = await _db.Database.BeginTransactionAsync();

    try
    {
        // 1) Borrar comentarios asociados
        if (noticia.comentarioGenerals != null && noticia.comentarioGenerals.Count > 0)
        {
            _db.comentarioGenerals.RemoveRange(noticia.comentarioGenerals);
            await _db.SaveChangesAsync();
        }

        // 2) Desasociar o eliminar boletines asociados (evita error FK)
        if (noticia.boletinGenerals != null && noticia.boletinGenerals.Count > 0)
        {
            foreach (var boletin in noticia.boletinGenerals)
            {
                boletin.GuidNoticia = null; // solo si tu FK es nullable
            }
            await _db.SaveChangesAsync();
        }

        // 3) Eliminar la noticia
        _db.noticiaGenerals.Remove(noticia);
        await _db.SaveChangesAsync();

        await tx.CommitAsync();
        return RedirectToAction(nameof(Index));
    }
    catch (Exception)
    {
        await tx.RollbackAsync();
        ModelState.AddModelError(string.Empty, "No se pudo eliminar la noticia (tiene referencias activas).");
        return View("~/Views/noticia_general/Delete.cshtml", noticia);
    }
}


    }
}
