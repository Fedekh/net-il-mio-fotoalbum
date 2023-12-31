﻿using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Drawing.Printing;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "Superadmin")]
    public class SuperadminController : Controller
    {
        private FotoContext _db;
        private ICustomLog _myLogger;
        private UserManager<IdentityUser> _userManager;


        public SuperadminController(FotoContext db, ICustomLog log, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _myLogger = log;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            int pageSize = 4; //risultati per pagina
            int pageNumber = page ?? 1;  //pagina corrente
            List<Foto> totalFotos = _db.Foto.Include(p => p.Categories).ToList();

            List<Foto> album = _db.Foto.Include(p => p.Categories)
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToList();
            var model = new PaginatedFotoViewModel()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Fotos = album,
                TotalPages =(int)Math.Ceiling((double)totalFotos.Count()/ pageSize)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            Foto? foto = _db.Foto
                                .Where(p => p.Id == id).Include(p => p.Categories)
                                .FirstOrDefault();
            if (foto == null)
                return View("../NotFound");
            return View(foto);

        }



        [HttpGet]
        public IActionResult Edit(long id)
        {
            Foto? foto = _db.Foto.Include(p=>p.Categories).FirstOrDefault(f => f.Id == id);
            if (foto == null)
            {
                return View("NotFound");
            }
            return View(foto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, bool isVisible)
        {
            Foto foto = _db.Foto.FirstOrDefault(f => f.Id == id);
            if (foto == null)
            {
                return View("NotFound");
            }

            // Modifica solo la proprietà "IsVisible"
            foto.IsVisible = isVisible;
            _db.SaveChanges();

            TempData["Message"] = "La visibilità della foto è stata modificata con successo.";

            return RedirectToAction("Index");
        }
    }
}
