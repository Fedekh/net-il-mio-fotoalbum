using Microsoft.AspNetCore.Http;
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

        public FotoController(IRepositoryFoto repoFoto)
        {
            _repo = repoFoto;
        }

        [HttpGet]
        public IActionResult Get(string? search)
        {
            if (search == null)
            {
                List<Foto> album = _repo.GetFotos();
                return Ok(album);
            }
            else
            {
                List<Foto> album = _repo.SearchFotos(search);
                return Ok(album);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetFoto(long id)
        {
            Foto? foto = _repo.GetFotobyId(id);
            if (foto == null) return NotFound("Foto non trovata");

            return Ok(foto);
        }
    }
}
