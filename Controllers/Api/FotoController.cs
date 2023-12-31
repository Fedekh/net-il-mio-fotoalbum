﻿using Microsoft.AspNetCore.Identity;
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
        public IActionResult GetCategories()
        {
            List<Category> categories = _repo.GetCategories();
            return Ok(categories);
        }


        [HttpGet]
        public IActionResult Get([FromQuery] string? search, [FromQuery] string? catInput, [FromQuery] int pageNumber = 1)
        {
            int itemsPerPage = 6;
            List<Foto> album;
            List<string>? catNames = catInput?.Split(", ").ToList();

            if (search == null)
            {
                album = _repo.GetFotos();
            }
            else
            {
                album = _repo.SearchFotos(search);
            }

            if (catNames != null && catNames.Count > 0)
            {
                album = album
                    .Where(foto => catNames.Any(cat => foto.Categories.Any(categoria => cat == categoria.Name)))
                    .ToList();
            }

            foreach (Foto foto in album)
            {
                foto.OwnerName = GetOwnerName(foto.OwnerID);
            }

            int totalItems = album.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var paginatedAlbum = album.Skip((pageNumber - 1) * itemsPerPage)
                                      .Take(itemsPerPage)
                                      .ToList();

            var viewModel = new PaginatedFotoViewModel
            {
                PageNumber = pageNumber,
                PageSize = itemsPerPage,
                TotalPages = totalPages,
                Fotos = paginatedAlbum,
                TotalCount = totalItems,
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
