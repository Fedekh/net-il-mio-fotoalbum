
namespace net_il_mio_fotoalbum.Models
{
    public class PaginatedFotoViewModel
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public List<Foto>? Fotos { get; set; }

        public PaginatedFotoViewModel() { }

        public PaginatedFotoViewModel(int pageSize, int pageNumber, int totalPages, List<Foto>? fotos)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            Fotos = fotos;
        }
    }

}

