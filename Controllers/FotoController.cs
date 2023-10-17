using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize]
    public class FotoController : Controller
    {
        private IRepositoryFoto _repositoryFoto;
        private ICustomLog _myLogger;

        public FotoController(ICustomLog log, IRepositoryFoto repositoryFoto)
        {
            _myLogger = log;
            _repositoryFoto = repositoryFoto;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _myLogger.WriteLog($"L'admin è arrivato in index");

            List<Foto> album = _repositoryFoto.GetFotos();
            if (album.Count > 0) return View(album);

            return View(TempData["Message"] = "Ci spiace non hai inserito nessuna foto ancora");
        }

        [HttpGet]
        public IActionResult SearchFotos(string search)
        {
            List<Foto> album = _repositoryFoto.SearchFotos(search);

            return View(album);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            Foto? foto = _repositoryFoto.GetFotobyId(id);

            return View(foto);
        }

        //[HttpGet]
        //public IActionResult Create()
    }
}
