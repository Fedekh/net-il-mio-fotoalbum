namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Foto>? Fotos { get; set; }

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }
    }
}
