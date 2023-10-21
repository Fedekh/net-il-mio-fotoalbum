using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Foto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "L'immagine deve per forza avere un titolo")]
        [StringLength(30, ErrorMessage = "Il titolo non può avere piu di 30 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "L'immagine deve avere una descrizione")]
        public string Description { get; set; }

        public bool IsVisible { get; set; } = true;

        public string? ImageUrl { get; set; }

        public byte[]? ImageFile { get; set; }

        public string ImageSrc => ImageFile is null ? (ImageUrl is null ? "" : ImageUrl) : $"data:image/jpeg;base64, {Convert.ToBase64String(ImageFile)}";

        public List<Category>? Categories { get; set; }

        public Foto() { }

        public Foto(string name, string description, byte[]? imageFile, List<Category>? categories)
        {
            Name = name;
            Description = description;
            ImageFile = imageFile;
            Categories = categories;
        }
    }
}
