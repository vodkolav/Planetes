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
            LogLevel.Warning,
            LogLevel.Status,
            LogLevel.CSV
        };

        public Logger()
        {
            T = new Thread(WriteLoop)
            {
                IsBackground = true,
                Name = "Logger"
            };
            messages = new BlockingCollection<LogMsg>();
            RedirectToFile(GameConfig.LogRedirectToFile);
            T.Start();
        }

        public static string LogFile { get; set; }

        public static void RedirectToFile(bool swtch)
        {
            if (swtch)
            {
                if (LogFile is null)
                {
                    throw new NullReferenceException("LogFile param must be populated for this to work");
                }

                Instance.output = new StreamWriter(LogFile);
            }
            else
                Instance.output = Console.Out;
        }

        public static void WriteLoop()
        {
            foreach (var message in Instance.messages.GetConsumingEnumerable())
            {
                Write(message);
            }
            Instance.messages.Dispose();
        }

        public static void AddMessage(LogMsg lm)
        {
            try
            {
                Instance.messages.Add(lm);
            }
            catch (ObjectDisposedException)
            {
                // when disposing of logger, the app still sends log messages, which throws ObjectDisposedException.
                // We just ignore it
            }
            catch (InvalidOperationException)
            {
                //same here
            }
        }

        public static void Log(Exception exception, LogLevel ll)
        {
            if (!loglevels.Contains(ll))
                return;
            AddMessage(new LogMsg { Exception = exception, loglevel = ll });
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
                AddMessage(new LogMsg { Message = message, loglevel = ll });
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
