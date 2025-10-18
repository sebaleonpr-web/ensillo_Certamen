using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var lista = await _db.noticiaGenerals.ToListAsync();
            return View("~/Views/noticia_general/Index.cshtml", lista);
        }

        public IActionResult Create()
        {
            return View("~/Views/noticia_general/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuidNoticia,TituloNoticia,DescripcionNoticia,FechaNoticia")] noticiaGeneral model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/noticia_general/Create.cshtml", model);

            _db.noticiaGenerals.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
