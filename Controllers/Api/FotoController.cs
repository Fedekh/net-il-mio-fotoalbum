using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Linq;
using System.Collections.Generic;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/foto/[action]")]
    [ApiController]
    public class FotoController : ControllerBase
    {
        private IRepositoryFoto _repo;
        private UserManager<IdentityUser> _userManager;

        public FotoController(IRepositoryFoto repoFoto, UserManager<IdentityUser> userManager)
        {
            _repo = repoFoto;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(string? search)
        {
            List<Foto> album;

            if (search == null)
            {
                album = _repo.GetFotos();
            }
            else
            {
                album = _repo.SearchFotos(search);
            }

            foreach (Foto foto in album)
            {
                foto.OwnerName = GetOwnerName(foto.OwnerID);
            }

            return Ok(album);
        }

        [HttpGet("{id}")]
        public IActionResult GetFoto(long id)
        {
            Foto foto = _repo.GetFotobyId(id);
            if (foto == null)
            {
                return NotFound("Foto non trovata");
            }

            foto.OwnerName = GetOwnerName(foto.OwnerID);

            return Ok(foto);
        }

        // Metodo per ottenere il nome dell'utente
        private string GetOwnerName(string ownerId)
        {
            IdentityUser? user = _userManager.FindByIdAsync(ownerId).Result;
            if (user != null)
            {
                string ownerName = user.UserName;
                Console.WriteLine($"OwnerName for {ownerId}: {ownerName}");
                return ownerName;
            }
            return "Sconosciuto";
        }

    }
}
