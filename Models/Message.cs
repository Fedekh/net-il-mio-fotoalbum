using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Devi inserire l'email per mandare il messaggio")]
        [EmailAddress(ErrorMessage = "Inserisci una mail corretta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Devi inserire un messaggio!")]
        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public string? OwnerName { get; set; }

        public Message()
        { DateTime = DateTime.Now; }

        public Message(string mail, string text, string? name)
        {
            Email = mail;
            Text = text;
            OwnerName = name;
            DateTime = DateTime.Now;
        }
    }
}
