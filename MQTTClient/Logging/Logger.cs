using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Services.MqttServices.Logging
{
    /// <summary>
    /// A class representing a Logger.
    /// </summary>
    public static class Logger
    {

        public static void Debug(string message)
        {
            Console.WriteLine($"{DateTime.Now} | DEBUG | {message}");
        }

        public static void Info(string message)
        {
            Console.WriteLine($"{DateTime.Now} | INFO | {message}");
        }

        public static void Warn(string message)
        {
            Console.WriteLine($"{DateTime.Now} | WARNING | {message}");
        }

        public static void Error(string message)
        {
            Console.WriteLine($"{DateTime.Now} | ERROR | {message}");
        }
    }
}
