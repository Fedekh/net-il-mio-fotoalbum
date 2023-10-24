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

        // user ID from AspNetUser table.
        public string? OwnerId { get; set; }

        public string? OwnerName { get; set; }

        public Message()
        { }

        public Message(string mail, string text)
        {
            Email = mail;
            Text = text;
        }
    }
}
