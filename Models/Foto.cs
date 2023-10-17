namespace net_il_mio_fotoalbum.Models
{
    public class Foto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[]? ImageFile { get; set; }

        public string ImageSrc => ImageFile is null ? "" : $"data:image/pgn;base64, {Convert.ToBase64String(ImageFile)}";

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
