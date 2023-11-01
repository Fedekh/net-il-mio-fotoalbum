using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

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
        public IActionResult Get(string? search, int pageNumber = 1)
        {
            int itemsPerPage = 3;
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

            int totalItems = album.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            // Effettua la paginazione
            var paginatedAlbum = album.Skip((pageNumber - 1) * itemsPerPage)
                                      .Take(itemsPerPage).ToList();

            var viewModel = new PaginatedFotoViewModel
            {
                PageNumber = pageNumber,
                PageSize = itemsPerPage,
                TotalPages = totalPages,
                Fotos = paginatedAlbum
            };

            return Ok(viewModel);
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
