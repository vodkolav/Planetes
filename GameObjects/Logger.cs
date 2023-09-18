using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
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
                    return $"[{loglevel}]Logger.message: {_message}";
                }
                return $"[{loglevel}]Logger.Exception: {Exception.Message}";
            }
            set { _message = value; }
        }
    }

    public enum LogLevel {Nothing, Status, Info, Warning, Debug, CSV, Trace, All }

    public class LogWriter : StreamWriter
    {
        public LogWriter(Stream stream) : base(stream)
        {
        }

        public LogWriter(string path) : base(path)
        {
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string message)
        {
            Logger.Log("Intercepted trace: " + message, LogLevel.Trace);
        }
        public override void Write(string message)
        {
            Logger.Log("Intercepted trace: " + message, LogLevel.Trace);
        }     
    }

    public class Logger
    {

        private BlockingCollection<LogMsg> messages;
        private Thread T;
        private TextWriter output { get; set; }

        private LogWriter _trace_interceptor { get; set; }

        public static TextWriter TraceInterceptor
        {
            get { return Instance._trace_interceptor; }
        }

        private readonly static Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());

        public static Logger Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        // loglevels moved to config

        public Logger()
        {
            T = new Thread(WriteLoop)
            {
                IsBackground = false,
                Name = "Logger"
            };
            messages = new BlockingCollection<LogMsg>();
            output = Console.Out;
            _trace_interceptor = new LogWriter("wtf.txt");
            T.Start();

            string text = "\n";
            GameConfig.loglevels.ForEach( ll => text += ll.ToString() + "\n");

            Console.WriteLine("LogLevels enabled:" + text , LogLevel.Info);
            Console.WriteLine("Log Output :" + output.ToString(), LogLevel.Info);
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
            RedirectToFile(GameConfig.LogRedirectToFile);
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
            if (!GameConfig.loglevels.Contains(ll))
                return;
            AddMessage(new LogMsg { Exception = exception, loglevel = ll });
        }

        public static void Log(string message, LogLevel ll)
        {
            if (!GameConfig.loglevels.Contains(ll))
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
                case LogLevel.Trace:
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
            _trace_interceptor.Dispose();
        }

        /// <summary>
        /// use this code to investigate problems when signalR ceases to receive model updates from server
        /// </summary>
        /// <param name="conn"></param>
        internal static void TraceConnection(Connection conn)
        {
            if (GameConfig.loglevels.Contains(LogLevel.Trace))
            {
                conn.TraceLevel = TraceLevels.All;
                conn.TraceWriter = TraceInterceptor;
                conn.Error += (e) => Log(e, LogLevel.Debug);
            }
            else
            {
                conn.TraceLevel = TraceLevels.None;
            }
        }
    }
}
