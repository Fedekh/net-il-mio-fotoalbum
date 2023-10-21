using la_mia_pizzeria_crud_mvc.CustomLogger;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Database
{
    public class RepositoryFoto : IRepositoryFoto
    {
       
        private FotoContext _db;

        public RepositoryFoto(FotoContext db)
        {           
            _db = db;
        }

        public List<Foto> GetFotos()
        {
            List<Foto> album = _db.Foto.Include(p=>p.Categories).ToList();
            return album;

        }


        public List<Foto> SearchFotos(string search)
        {
            if (search == null) return GetFotos();

            List<Foto> album = _db.Foto.
                                        Where(p=>p.Name.ToLower()
                                        .Contains(search.ToLower()))
                                        .Include(p=>p.Categories)
                                        .ToList();

            return album;
        }


        public Foto GetFotobyId(long id)
        {
            Foto? foto = _db.Foto.Where(p=>p.Id == id).Include(p=>p.Categories).FirstOrDefault();

            if (foto == null) throw new Exception("Foto non trovata in archivio");

            return foto;

        }

        public bool CreateFoto(Foto pizzaAdd)
        {
            _db.Foto.Add(pizzaAdd);
            _db.SaveChanges();
            return true;
        }


        public bool UpdateFoto(long id, Foto pizza)
        {
            Foto? fotoEdit = _db.Foto.Where(p=>p.Id ==id).FirstOrDefault();

            if (fotoEdit == null) throw new Exception("Foto non trovata in archivio");


            fotoEdit.Name = pizza.Name;
            fotoEdit.Description = pizza.Description;
            fotoEdit.ImageFile = pizza.ImageFile;
            fotoEdit.Categories = pizza.Categories;

            _db.SaveChanges();
            return true;
        }


        public bool DeleteFoto(long id)
        {
            Foto? fotoDelete = _db.Foto.Where(p => p.Id == id).Include(P=>P.Categories).FirstOrDefault();

            if (fotoDelete == null) throw new Exception("Foto non trovata in archivio");

            _db.Remove(fotoDelete);
            _db.SaveChanges();
            return true;
        }
    }
}
