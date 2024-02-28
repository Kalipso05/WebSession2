using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Settings
{
    internal class Logger
    {
        internal static void Log(string message, HttpStatusCode code = HttpStatusCode.OK, ConsoleColor consoleColor = ConsoleColor.Green)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"[{DateTime.Now}] - {message} ({code})");
        }
    }
}
