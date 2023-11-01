using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;

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
            _myLogger.WriteLog($"User è arrivato in index");
            return View();
        }

      
        [HttpGet]
        public IActionResult MessageView()
        {
            _myLogger.WriteLog($"User sta per mandare un messaggio");
            return View();
        }

    }
}