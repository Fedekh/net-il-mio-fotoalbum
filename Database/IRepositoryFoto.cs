using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Database
{
    public interface IRepositoryFoto
    {
        public List<Foto> GetFotos();
        public List<Foto> SearchFotos(string name);
        public Foto GetFotobyId(long id);
        public bool CreateFoto(Foto pizzaAdd);
        public bool UpdateFoto(long id, Foto pizza);
        public bool DeleteFoto(long id);
    }
}
