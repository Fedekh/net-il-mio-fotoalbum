using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles ="Admin,Superadmin")]
    public class FotoController : Controller
    {
        private FotoContext _db;
        private ICustomLog _myLogger;
        private UserManager<IdentityUser> _userManager;

        public FotoController(FotoContext db, ICustomLog log, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _myLogger = log;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            //guid dell admin
            string admin = _userManager.GetUserId(User);
            IdentityUser? user = _userManager.GetUserAsync(User).Result;

            _myLogger.WriteLog($"L'admin {_userManager.GetUserAsync(User)} è arrivato in index");

            var roles = _userManager.GetRolesAsync(user).Result;

            if (roles.Contains("Superadmin"))
            {
                return RedirectToAction("Index", "Superadmin");
            }

            int pageSize = 4; //risultati per pagina
            int pageNumber = page ?? 1;  //pagina corrente

            List<Foto> totalFotos = _db.Foto.Where(p => p.OwnerID == admin)
                                       .Include(p => p.Categories).ToList();

            List<Foto> album = _db.Foto.Where(p => p.OwnerID == admin)
                                       .Include(p => p.Categories)
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
        public IActionResult SearchFotos(string? search, int? page)
        {
            string admin = _userManager.GetUserId(User);
            int pageSize = 4;
            int pageNumber = page ?? 1;

            if (string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", new { page });
            }

            var query = _db.Foto.Where(p => p.OwnerID == admin && p.Name
                                .Contains(search))
                                .Include(p => p.Categories);

            int totalItems = query.Count();

            List<Foto> album = query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            if (album.Count != 0)
            {
                // Se sono presenti risultati, visualizza i risultati.
                PaginatedFotoViewModel model = new PaginatedFotoViewModel
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    Fotos = album,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
                };
                return View("Index", model);
            }
            else
            {
                // visualizza tutti gli album
                List<Foto> allAlbum = _db.Foto.Where(p => p.OwnerID == admin)
                                       .Include(p => p.Categories)
                                       .ToList();

                PaginatedFotoViewModel model = new PaginatedFotoViewModel()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    Fotos = allAlbum,
                    TotalPages = (int)Math.Ceiling((double)allAlbum.Count / pageSize)
                };
                return View("Index", model);
            }
        }



        [HttpGet]
        public IActionResult Details(long id)
        {
            string admin = _userManager.GetUserId(User);
            Foto? foto = _db.Foto.Where(p => p.OwnerID == admin)
                                .Where(p => p.Id == id).Include(p => p.Categories)
                                .FirstOrDefault();

            if (foto == null)
                return View("../NotFound");

            return View(foto);

        }


        [HttpGet]
        public IActionResult Create()
        {
            string admin = _userManager.GetUserId(User);

            List<SelectListItem> allCategoriesList = new List<SelectListItem>();
            List<Category> dbCategories = _db.Category.Where(p => p.OwnerId == admin || p.OwnerId == null)
                                                        .ToList();

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

            model.Foto.OwnerID = admin;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FotoFormModel model)
        {
            string admin = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesList = new List<SelectListItem>();
                List<Category> categories = _db.Category.Where(p => p.OwnerId == admin || p.OwnerId == null)
                                                        .ToList();

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

                        Category? categoryDb = _db.Category.Where(p => p.OwnerId == admin || p.OwnerId == null)
                                                            .Where(p => p.Id == categoriesIds)
                                                            .FirstOrDefault();

                        if (categoryDb != null) model.Foto.Categories.Add(categoryDb);
                    }
                }
            }
            
            model.Foto.OwnerID = admin;

            _db.Foto.Add(model.Foto);
            _db.SaveChanges();
            TempData["Message"] = $"La foto {model.Foto.Name} è stata creata correttamente";

            return RedirectToAction("Index");
        }
    
        



        [HttpGet]
        public IActionResult Edit(long id)
        {
            string admin = _userManager.GetUserId(User);
            Foto? fotoEdit = _db.Foto.Where(p=>p.OwnerID == admin)
                                        .Where(p => p.Id == id)
                                        .Include(p => p.Categories)
                                        .FirstOrDefault();

            if (fotoEdit == null) return View("../NotFound");

            List<Category> dbCategories = _db.Category.Where(p=>p.OwnerId == admin || p.OwnerId == null).ToList();
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
            string admin = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                List<Category> categories = _db.Category.Where(p => p.OwnerId == admin || p.OwnerId == null).ToList();
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
                Foto? fotoEdit = _db.Foto.Where(p=>p.OwnerID == admin).Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

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

                        Category? categoryInDb = _db.Category.Where(p => p.OwnerId == admin || p.OwnerId == null).Where(category => category.Id == categorySelectedId).FirstOrDefault();

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
            string admin = _userManager.GetUserId(User);

            Foto? fotoDelete = _db.Foto.Where(p=>p.OwnerID == admin).Where(p=>p.Id == id).Include(p=>p.Categories).FirstOrDefault();

            if (fotoDelete == null) return View("../NotFound");

            _db.Foto.Remove(fotoDelete);
            _db.SaveChanges();

            TempData["Message"] = $"La foto {fotoDelete.Name} è stata eliminata correttamente";

            return RedirectToAction("Index");

        }

     
    }
}



    
     
