using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Controllers
{
    public class boletin_generalController : Controller
    {
        private readonly boletinLayonContext _db;

        public boletin_generalController(boletinLayonContext db)
        {
            _db = db;
        }


        //GET: /boletin_general
        //Tabla de boletines, Index.cshtml        //Carga la lista de boletines ordenados por fecha descendente
        public async Task<IActionResult> Index()
        {
            var lista = await _db.boletinGenerals
                .AsNoTracking()
                .Include(b => b.GuidNoticiaNavigation) // 🔹 carga la noticia vinculada
                .OrderByDescending(b => b.FechaBoletin)
                .ToListAsync();

            return View("~/Views/boletin_general/Index.cshtml", lista);
        }


        
        //GET: /boletin_general/Create
        //Al presionar el boton de crear en la vista Index.cshtml
        //carga el combo de noticias
        public IActionResult Create()
        {
            ViewBag.Noticias = new SelectList(
                _db.noticiaGenerals
                .OrderBy(n => n.GuidNoticia)
                .Select(n => new { n.GuidNoticia, Texto = n.tituloNoticia.ToString() }),
                "GuidNoticia", "Texto"
            );
            return View("~/Views/boletin_general/Create.cshtml");
        }


        //POST: /boletin_general/Create
        //Al presionar el boton de crear en la vista Create.cshtml
        [HttpPost]
        //Evitar ataques maliciosos
        [ValidateAntiForgeryToken]
        //Definir los campos que se van a bindear
        //Campos definidos en el modelo BoletinGeneral.cs
        public async Task<IActionResult> Create([Bind("TituloBoletin,DescripcionBoletin,FechaBoletin,GuidNoticia")] boletinGeneral model)
        {
            //Validador si falla un dato
            if (!ModelState.IsValid)
            {
                //Recargar el combo de noticias si hay error
                ViewBag.Noticias = new SelectList(
                    _db.noticiaGenerals
                    .OrderBy(n => n.GuidNoticia)
                    .Select(n => new { n.GuidNoticia, Texto = n.tituloNoticia }),
                    "GuidNoticia", "tituloNoticia", model.GuidNoticia
                );
                return View("~/Views/boletin_general/Create.cshtml", model);
            }
            //Agregar un nuevo boletin
            _db.boletinGenerals.Add(model);
            //Se sincroniza con la base de datos
            //Confirma que se guarda
            await _db.SaveChangesAsync();
            //Redirigir a la vista Index.cshtml
            return RedirectToAction(nameof(Index));
        }

        //GET: /boletin_general/Edit/{id}
        //Al presionar el boton de editar en la vista Index.cshtml
        //Recibe el id del boletin a editar
        public async Task<IActionResult> Edit(Guid? id)
        {
            //Validador si el id es nulo
            //Si es nulo retorna no encontrado
            if (id == null) return NotFound();
            //Buscar el boletin por id en la database
            var entidad = await _db.boletinGenerals.FindAsync(id);
            if (entidad == null) return NotFound();
            //Cargar el combo de noticias
            ViewBag.Noticias = new SelectList(
                _db.noticiaGenerals
                .OrderBy(n => n.GuidNoticia)
                .Select(n => new { n.GuidNoticia, Texto = n.tituloNoticia.ToString() }),
                "GuidNoticia", "Texto", entidad.GuidNoticia
            );
            //Retornar la vista Edit.cshtml con la entidad encontrada
            return View("~/Views/boletin_general/Edit.cshtml", entidad);
        }

        //POST: /boletin_general/Edit/{id}
        //Al presionar el boton de editar en la vista Edit.cshtml
        //Submit del formulario
        [HttpPost]
        //Evitar ataques maliciosos
        [ValidateAntiForgeryToken]
        //Definir los campos que se van a bindear
        //Campos definidos en el modelo BoletinGeneral.cs
        //ID RETORNADO DEL URL, asp-route-id=@item.GuidBoletin
        public async Task<IActionResult> Edit(Guid id, [Bind("GuidBoletin,TituloBoletin,DescripcionBoletin,FechaBoletin,GuidNoticia")] boletinGeneral model)
        {

            if (id != model.GuidBoletin) return NotFound();
            //Validador si falla un dato
            if (!ModelState.IsValid)
            {
                //Recargar el combo de noticias si hay error
                ViewBag.Noticias = new SelectList(
                    _db.noticiaGenerals
                    .OrderBy(n => n.GuidNoticia)
                    .Select(n => new { n.GuidNoticia, Texto = n.tituloNoticia.ToString() }),
                    "GuidNoticia", "tituloNoticia", model.GuidNoticia
                );
                //Retornar la vista Edit.cshtml con el modelo
                //Datos incorrectos
                return View("~/Views/boletin_general/Edit.cshtml", model);
            }

            try
            {
                //Actualizar el boletin
                //Guardar los cambios en la base de datos
                //Este puede fallar
                //Si cambia lo guarda
                _db.Update(model);
                //Confirma que se guarda
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                //Validador si el boletin existe / AnySync verifica si existe
                //Si no existe retorna no encontrado
                //Da booleano
                //Existe o no existe
                var existe = await _db.boletinGenerals.AnyAsync(b => b.GuidBoletin == model.GuidBoletin);
                if (!existe) return NotFound();
                //Si existe lanza la excepcion
                //Nos devuelve al catch
                throw;
            }
            //Redirigir a la vista Index.cshtml
            //Si todo sale bien
            return RedirectToAction(nameof(Index));
        }

        //Delete: /boletin_general/Delete/{id}
        //Al presionar el boton de eliminar en la vista Index.cshtml
        //Recibe el id del boletin a eliminar
        //Este id es del URL
        public async Task<IActionResult> Delete(Guid? id)
        {
            //Si el id es nulo retorna no encontrado
            if (id == null) return NotFound();
            //Buscar el boletin por id en la database
            var entidad = await _db.boletinGenerals
                .Include(b => b.GuidNoticiaNavigation) // 👈 carga la relación
                .FirstOrDefaultAsync(b => b.GuidBoletin == id);

            //Si no lo encuentra retorna no encontrado
            if (entidad == null) return NotFound();
            //Retorna la vista Delete.cshtml con la entidad encontrada
            return View("~/Views/boletin_general/Delete.cshtml", entidad);
        }
        //POST: /boletin_general/Delete/{id}
        //La accion se llama DeleteConfirmed, esta responde al button delete
        //Al presionar el boton de eliminar en la vista Delete.cshtml
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //Buscar el boletin por id en la database
        var entidad = await _db.boletinGenerals
            .Include(b => b.GuidNoticiaNavigation) // 👈 carga la relación
            .FirstOrDefaultAsync(b => b.GuidBoletin == id);

            //Si lo encuentra, eliminar
            if (entidad != null)
            {
                //Eliminar el boletin
                _db.boletinGenerals.Remove(entidad);
                //Guardar los cambios en la base de datos
                await _db.SaveChangesAsync();
            }
            //Redirigir a la vista Index.cshtml
            return RedirectToAction(nameof(Index));
        }
    }
}
