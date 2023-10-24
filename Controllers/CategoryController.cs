using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{   
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private ICustomLog _myLogger;
        private FotoContext _db;
        private UserManager<IdentityUser> _userManager;


        public CategoryController(FotoContext db, ICustomLog log, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _myLogger = log;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string admin = _userManager.GetUserId(User);

            List<Category>? categories = _db.Category.Where(p=>p.OwnerId == admin || p.OwnerId == null).ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            string admin = _userManager.GetUserId(User);

            Category? category = _db.Category.Where(p => p.OwnerId == admin || p.OwnerId == null).Where(p=>p.Id == id).FirstOrDefault();
            if (category == null) return View("../NotFound");
            return View(category);
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
            string admin = _userManager.GetUserId(User);
            category.OwnerId = admin;

            if (!ModelState.IsValid) return View(category);

            _db.Category.Add(category);
            _db.SaveChanges();

            TempData["Message"] = $"Hai creato correttamente la categoria {category.Name}";

            return RedirectToAction("Index");
        }   


        [HttpGet]
        public IActionResult Edit(long id)
        {
            string admin = _userManager.GetUserId(User);

            Category? categoryEdit = _db.Category.Where(p=>p.OwnerId == admin).Where(P => P.Id == id).FirstOrDefault();

            if (categoryEdit != null)
            {
                 _myLogger.WriteLog($"L'utente vuole modificare una foto");
                return View(categoryEdit);

            }
            return View("../NotFound");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(long id, Category category)
        {
            string admin = _userManager.GetUserId(User);

            if (!ModelState.IsValid) return View(category);

            Category? categoryEdit = _db.Category.Where(p => p.OwnerId == admin).Where(p=>p.Id == id).FirstOrDefault();
            if (categoryEdit == null)   return View("../NotFound");

            categoryEdit.Name = category.Name;
            _db.SaveChanges();

            TempData["Message"] = $"La categoria {categoryEdit.Name} è stata modificata correttamente";
            _myLogger.WriteLog($"L'utente ha modificato {categoryEdit.Name}");

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id) 
        {
            string admin = _userManager.GetUserId(User);

            Category? categoryDelete = _db.Category.Where(p => p.OwnerId == admin).Where(p => p.Id == id).FirstOrDefault();
            if (categoryDelete == null) return View("../NotFound");
            else
            {
                _db.Category.Remove(categoryDelete);
                _db.SaveChanges();
                TempData["Message"] = $"La categoria {categoryDelete.Name} è stata eliminata correttamente";
                _myLogger.WriteLog($"L'utente ha eliminato {categoryDelete.Name}");

            return RedirectToAction("Index");
            }

        }
    }
}