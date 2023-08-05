using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GameObjects
{
    public struct LogMsg
    {
        private string _message;
        public Exception Exception;
        public LogLevel loglevel;
        public string Message
        {
            get
            {
                if (Exception is null)
                {
                    return _message;
                }
                return Exception.Message;
            }
            set { _message = value; }
        }
    }

    public enum LogLevel {Nothing, Status, Info, Warning, Debug, CSV, All }

    public class Logger
    {

        private BlockingCollection<LogMsg> messages;
        private Thread T;
        private TextWriter output { get; set; }
        private readonly static Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());

        public static Logger Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private static List<LogLevel> loglevels { get; set; } = new List<LogLevel>()
        {
            LogLevel.Debug,
            LogLevel.Info,
            LogLevel.Status,
            LogLevel.CSV
        };

        public Logger()
        {
            T = new Thread(Process);
            T.IsBackground = true;
            T.Name = "Logger";
            messages = new BlockingCollection<LogMsg>();
            output = Console.Out;
            T.Start();
        }

        public void Init(string filepath)
        {
            // StreamWriter outputFile = outputFile;
            output = new StreamWriter(filepath); 
        }

        public static void Process()
        {
            foreach (var message in Instance.messages.GetConsumingEnumerable())
            {
                Write(message);
            }
            Instance.messages.Dispose();
        }





        public static void Log(Exception exception, LogLevel ll)
        {
            if (!loglevels.Contains(ll))
                return;
            Instance.messages.Add(new LogMsg{Exception = exception, loglevel = ll});
        }

        public static void Log(string message, LogLevel ll)
        {

            if (!loglevels.Contains(ll))
                return;
            if (ll is LogLevel.Debug)
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
            else
            {
                Instance.messages.Add(new LogMsg { Message = message, loglevel = ll });
            }
        }

        public static void Write(LogMsg msg)
        {
            switch (msg.loglevel)
            {
                case LogLevel.Status:
                    Instance.output.Write('\r' + msg.Message);
                    break;
                case LogLevel.Info:
                    Instance.output.WriteLine(msg.Message);
                    break;
                case LogLevel.Warning:
                    Instance.output.WriteLine(msg.Message);
                    break;
                case LogLevel.Debug:
                    Instance.output.WriteLine(msg.Message);
                    Instance.output.WriteLine(msg.Exception.StackTrace);
                    break;
                case LogLevel.CSV:
                    Instance.output.WriteLine(msg.Message);
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {            
            messages.CompleteAdding();
            output.Dispose();
        }
    }
}
