using System;
using System.Collections.Generic;

namespace GameObjects
{

    public enum LogLevel {Nothing, Status, Info, Warning, Debug, All }

    public static class Logger
    {

        private static List<LogLevel> loglevels { get; set; } = new List<LogLevel>()
        {
            LogLevel.Debug,
            LogLevel.Info,
            LogLevel.Status
        };

        public static void Log(Exception exception, LogLevel ll)
        {

            if (!loglevels.Contains(ll))
                return;

            switch (ll)
            {
                case LogLevel.Status:
                    Console.Write('\r' + exception.Message);
                    break;
                case LogLevel.Info:
                    Console.WriteLine(exception.Message);
                    break;
                case LogLevel.Warning:
                    Console.WriteLine(exception.Message);
                    break;
                case LogLevel.Debug:
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);
                    break;
                default:
                    break;
            }
        }

        public static void Log(string message, LogLevel ll)
        {
            Exception exception;
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                exception = new Exception(message, ex);
            }            
            Log(exception, ll);
        }
    }
}
