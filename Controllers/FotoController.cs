using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles ="Admin")]
    public class FotoController : Controller
    {
        private FotoContext _db;
        private ICustomLog _myLogger;
        private string administratorId;

        public FotoController(FotoContext db, ICustomLog log)
        {
            _db = db;
            _myLogger = log;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _myLogger.WriteLog($"L'admin è arrivato in index");

            List<Foto>? album = _db.Foto.Include(p => p.Categories).ToList();

            return View(album);

        }

        [HttpGet]
        public IActionResult SearchFotos(string search)
        {

            if (string.IsNullOrEmpty(search)) return Index();

            List<Foto> album = _db.Foto.Where(image => image.Name.Contains(search)).Include(p => p.Categories).ToList();
            if (album.Count != 0)  return View("Index",album);
            else
            {
                List<Foto>? album2 = _db.Foto.Include(p => p.Categories).ToList();

                return View("Index",album2);
            }
        }


        [HttpGet]
        public IActionResult Details(long id)
        {
            Foto? foto = _db.Foto.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

            if (foto == null)
                return View("../NotFound");

            return View(foto);

        }


        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> allCategoriesList = new List<SelectListItem>();
            List<Category> dbCategories = _db.Category.ToList();

            foreach (Category category in dbCategories)
            {
                allCategoriesList.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }

            FotoFormModel model = new FotoFormModel()
            {
                Foto = new Foto(),
                Categories = allCategoriesList
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FotoFormModel model)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesList = new List<SelectListItem>();
                List<Category> categories = _db.Category.ToList();

                foreach (Category category in categories)
                {
                    allCategoriesList.Add(new SelectListItem()
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });

                }

                model.Categories = allCategoriesList;

                return View(model);

            }
            else
            {

                using (MemoryStream stream = new MemoryStream())
                {
                    model.ImageFormFile?.CopyToAsync(stream);

                    // Converte il contenuto del file in un array di byte
                    byte[] fileBytes = stream.ToArray();

                    model.Foto.ImageFile = fileBytes;
                }


                model.Foto.Categories = new List<Category>();

                if (model.SelectedCategoriesId != null)
                {
                    foreach (string selectedId in model.SelectedCategoriesId)
                    {
                        long categoriesIds = long.Parse(selectedId);

                        Category? categoryDb = _db.Category.Where(p => p.Id == categoriesIds).FirstOrDefault();

                        if (categoryDb != null) model.Foto.Categories.Add(categoryDb);
                    }
                }
            }

            _db.Foto.Add(model.Foto);
            _db.SaveChanges();
            TempData["Message"] = $"La foto {model.Foto.Name} è stata creata correttamente";

            return RedirectToAction("Index");
        }
    
        



        [HttpGet]
        public IActionResult Edit(long id)
        {
            Foto? fotoEdit = _db.Foto.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

            if (fotoEdit == null) return View("../NotFound");

            List<Category> dbCategories = _db.Category.ToList();
            List<SelectListItem> categoriesList = new List<SelectListItem>();

            foreach (Category cat in dbCategories)
            {
                categoriesList.Add(new SelectListItem()
                {
                    Text = cat.Name,
                    Value = cat.Id.ToString(),
                    Selected =  fotoEdit.Categories.Any(p => p.Id == cat.Id),
                });
            }

            FotoFormModel model = new FotoFormModel()
            {
                Foto = fotoEdit,
                Categories = categoriesList
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, FotoFormModel model)
        {
            if (!ModelState.IsValid)
            {
                List<Category> categories = _db.Category.ToList();
                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach (Category cat in categories)
                {
                    selectListItems.Add(new SelectListItem() { Text = cat.Name, Value = cat.Id.ToString(), });
                }

                model.Categories = selectListItems;

                return View(model);
            }

            else
            {
                Foto? fotoEdit = _db.Foto.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

                if (fotoEdit == null) return View("../NotFound");


                fotoEdit.Categories.Clear();

                fotoEdit.ImageUrl = model.Foto.ImageUrl;
                fotoEdit.Name = model.Foto.Name;
                fotoEdit.Description = model.Foto.Description;
                fotoEdit.IsVisible = model.Foto.IsVisible;

                if (model.SelectedCategoriesId != null)
                {
                    foreach (string category in model.SelectedCategoriesId)
                    {
                        long categorySelectedId = long.Parse(category);

                        Category? categoryInDb = _db.Category.Where(category => category.Id == categorySelectedId).FirstOrDefault();

                        if (categoryInDb != null) fotoEdit.Categories.Add(categoryInDb);
                    }
                }


                // Gestisci l'aggiornamento dell'immagine
                if (model.ImageFormFile != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        model.ImageFormFile?.CopyTo(stream);

                        byte[] fileBytes = stream.ToArray();

                        fotoEdit.ImageFile = fileBytes;
                    }
                }

                _db.SaveChanges();
                TempData["Message"] = $"La foto {fotoEdit.Name} è stata modificata correttamente";


                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            Foto? fotoDelete = _db.Foto.Where(p=>p.Id == id).Include(p=>p.Categories).FirstOrDefault();

            if (fotoDelete == null) return View("../NotFound");

            _db.Foto.Remove(fotoDelete);
            _db.SaveChanges();

            TempData["Message"] = $"La foto {fotoDelete.Name} è stata eliminata correttamente";

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult GetMessage()
        {
            List<Message> messages = _db.Message.ToList();
            return View(messages);
        }
    }
}



    
     
