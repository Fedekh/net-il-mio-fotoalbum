using System.Diagnostics;

namespace la_mia_pizzeria_crud_mvc.CustomLogger
{
    public class CustomConsoleLogger : ICustomLog
    {
        public void WriteLog(string message)
        {
            Debug.WriteLine($"Log : {message}");
        }
    }

}