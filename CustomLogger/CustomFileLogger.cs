using System.Diagnostics;

namespace la_mia_pizzeria_crud_mvc.CustomLogger
{
    public class CustomFileLogger : ICustomLog
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("./my_log.txt", $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}] LOG {message}{Environment.NewLine}"); // appende ogni volta

        }
    }

}