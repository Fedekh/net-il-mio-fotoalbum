using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class HomeController : Controller
    {
        private FotoContext _db;
        private ICustomLog _myLogger;

        public HomeController(FotoContext db, ICustomLog log)
        {
            _db = db;
            _myLogger = log;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

      
        [HttpGet]
        public IActionResult MessageView()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult SearchFotos(string search)
        //{

        //    if (string.IsNullOrEmpty(search)) return Index();

        //    List<Foto> album = _db.Foto.Where(image => image.Name.Contains(search)).Include(p => p.Categories).ToList();
        //    if (album.Count != 0) return View("Index", album);
        //    else
        //    {
        //        List<Foto>? album2 = _db.Foto.Include(p => p.Categories).ToList();

        //        return View("Index", album2);
        //    }
        //}




    }
}