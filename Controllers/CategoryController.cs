using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {
        private ICustomLog _myLogger;
        private FotoContext _db;


        public CategoryController(ICustomLog log, FotoContext db)
        {
            _myLogger = log;
            _db = db;

        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Category> categories = _db.Category.ToList();

            if (categories.Count == 0) throw new Exception("Non ci sono categorie");

            return View(categories);
        }


        [HttpGet]
        public IActionResult Create() 
        {
            _myLogger.WriteLog($"L'utente vuole creare una foto");
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {

            if (!ModelState.IsValid) return View(category);

            _db.Category.Add(category);
            _db.SaveChanges();

            TempData["Message"] = $"Hai creato correttamente {category.Name}";

            return View("Index");
        }   


        [HttpGet]
        public IActionResult Edit(long id)
        {

            Category? categoryEdit = _db.Category.Where(P => P.Id == id).FirstOrDefault();

            if (categoryEdit != null)
            {
                 _myLogger.WriteLog($"L'utente vuole modificare una foto");
                return View("Index");

            }
            return View("../NotFound");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(long id, Category category)
        {

            if (!ModelState.IsValid) return View(category);

            Category? categoryEdit = _db.Category.Where(p=>p.Id == id).FirstOrDefault();
            if (categoryEdit == null)   return View("../NotFound");

            categoryEdit.Name = category.Name;
            _db.SaveChanges();

            TempData["Message"] = $"La foto {categoryEdit.Name} è stata modificata correttamente";
            _myLogger.WriteLog($"L'utente ha modificato {categoryEdit.Name}");

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id) 
        {
            Category? categoryDelete = _db.Category.Where(p => p.Id == id).FirstOrDefault();
            if (categoryDelete == null) return View("../NotFound");
            else
            {

            _db.Category.Remove(categoryDelete);
            _db.SaveChanges();
            TempData["Message"] = $"La foto {categoryDelete.Name} è stata eliminata correttamente";
            _myLogger.WriteLog($"L'utente ha eliminato {categoryDelete.Name}");

            return RedirectToAction("Index");
            }

        }



    }
}