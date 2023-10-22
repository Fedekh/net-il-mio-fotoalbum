using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;


namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/message/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly FotoContext _myDb;


        public MessageController(FotoContext db)
        {
            _myDb = db;
        }

        [HttpPost]
        public IActionResult CreateMessage([FromBody] Message newMessage)
        {
            if (newMessage == null) return BadRequest("DATI NON VALIDI");

            _myDb.Message.Add(newMessage);
            _myDb.SaveChanges();

            return Ok(new { message = "Messaggio creato con successo" });
        }

    }
}
