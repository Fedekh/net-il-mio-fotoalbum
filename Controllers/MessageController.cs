using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MessageController : Controller
    {
        private FotoContext _db;
        private ICustomLog _myLogger;
        private UserManager<IdentityUser> _userManager;

        public MessageController(FotoContext db, ICustomLog log, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _myLogger = log;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string admin = _userManager.GetUserName(User);
            _myLogger.WriteLog($"L'admin {_userManager.GetUserAsync(User)} sta controllando i messaggi");
            List<Message>? messages = _db.Message.Where(p => p.OwnerName == admin).OrderByDescending(p => p.DateTime).ToList();
            return View(messages);
        }      


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            Message? message = _db.Message.Where(p=>p.Id == id).FirstOrDefault();
            if (message == null) return View("../NotFound");
            _db.Message.Remove(message);
            _db.SaveChanges();
            TempData["Message"] = $"Il messaggio dall'utente {message.Email} è stato cancellato correttament";

            return RedirectToAction("Index");
        }

    }
}