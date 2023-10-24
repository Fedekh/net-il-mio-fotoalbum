using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Il nome della categoria è obbligatorio")]
        public string Name { get; set; }

        public List<Foto>? Fotos { get; set; }

        public string? OwnerId { get; set; } // Proprietà per l'ID dell'admin proprietario

        public string? OwnerName { get; set; }

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
