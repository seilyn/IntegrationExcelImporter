using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;


namespace IntegrationExcelImporter.Common
{
    public static class Log
    {
        public enum Priority : int
        {
            OFF = 200,
            DEBUG = 100,
            INFO = 75,
            WARN = 50,
            ERROR = 25,
            FATAL = 0
        }

        public enum eType : int
        {
            Seq = 0,
            IO = 1,
            Thread = 2,
            Machine = 3,
            FillData = 4
        }

        private static string LoggingLevel = "Normal";
        private readonly static String[] logDirectory = new String[5];
        private readonly static StreamWriter[] sw = new StreamWriter[5];
        private readonly static DateTime[] lastLogTime = new DateTime[5];
        private static readonly object[] locker = new object[5];

        private static CultureInfo GetLogCulture()
        {
            CultureInfo culture = new CultureInfo("en");
            return culture;
        }

        private static string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");//@".\Log";
        public static string LogPath
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
                Init();
            }
        }

        private static int logFileRetentionDays = 90;
        public static int LogFileRetentionDays
        {
            get
            {
                return logFileRetentionDays < 1 ? 90 : logFileRetentionDays;
            }
            set
            {
                logFileRetentionDays = value;
            }
        }

        static Log()
        {
            Init();
        }

        public static void Socket(String message)
        {
            Log.Append("[S]" + message, Priority.INFO);
        }

        public static void Debug(String message)
        {
            Log.Append("[D]" + message, Priority.DEBUG);
        }

        public static void Info(String message, eType type = eType.Seq)
        {
            Log.Append(String.Format("[I]{0}", message), Priority.INFO, type);
        }

        public static void Warn(String message)
        {
            Log.Append("[W]" + message, Priority.WARN);
        }

        public static void Error(String message, eType type = eType.Seq)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            Log.Append(String.Format("[E][{1}.{2}]{0}", message, methodBase.DeclaringType.Name, methodBase.Name), Priority.ERROR, type);
        }

        public static void Error(Exception ex, string userMessage = null, eType type = eType.Seq)
        {

            string message = ex == null ? string.Empty : ex.ToString();
            if (userMessage != null)
                message += "\r\nUserMessage : " + userMessage;

            Log.Append("[E]" + message, Priority.ERROR, type);
        }

        public static void Fatal(String message)
        {
            Log.Append("[F]" + message, Priority.FATAL);
        }

        private static void Init()
        {
            if (string.IsNullOrWhiteSpace(LogPath) == false)
            {
                if (Directory.Exists(LogPath) == false)
                {
                    Directory.CreateDirectory(LogPath);
                }
            }
        }
        private static void CreateLogFile(int i)
        {
            logDirectory[i] = GetRealLogPath((eType)i);

            if (File.Exists(logDirectory[i]) == false)
            {
                FileStream fs = File.Create(logDirectory[i]);
                fs.Close();
            }

            locker[i] = new object();
        }

        public static void Append(string message, Priority level, eType type = eType.Seq)
        {
            DateTime now = DateTime.Now;

            if (lastLogTime[(int)type].Date != now.Date)
            {
                CreateLogFile((int)type);
            }

            lastLogTime[(int)type] = now;

            int logLevel = (int)Priority.OFF;
            // find out what logLevel we're currently at, default to 0
            string strLogLevel = LoggingLevel.ToUpper() == "NORMAL" ? "INFO" : "DEBUG";

            switch (strLogLevel)
            {
                case "DEBUG":
                    logLevel = (int)Priority.DEBUG;
                    break;
                case "INFO":
                    logLevel = (int)Priority.INFO;
                    break;
                case "WARN":
                    logLevel = (int)Priority.WARN;
                    break;
                case "ERROR":
                    logLevel = (int)Priority.ERROR;
                    break;
                case "FATAL":
                    logLevel = (int)Priority.FATAL;
                    break;
                default:
                    logLevel = (int)Priority.OFF;
                    break;
            }


            // if this message has a priority greater than or equal to the current logging level, log it
            if (logLevel >= (int)level)
            {
                //System.Diagnostics.Debug.WriteLine("{0} : {1}", DateTime.Now.ToString("HH:mm:ss.fff"), message);

                lock (locker[(int)type])
                {
                    try
                    {
                        using (StreamWriter swTemp = File.AppendText(logDirectory[(int)type]))// sw[(int)type];
                        {
                            swTemp.WriteLine("[{0}]{1}", DateTime.Now.ToString("HH:mm:ss.fff"), message);
                            swTemp.Flush();
                        }
                    }
                    catch
                    {
                        CreateLogFile((int)type);
                    }
                }
            }
        }

        private static string GetRealLogPath(eType type)
        {
            string logFolder = Path.Combine(LogPath, DateTime.Today.ToString("yyyyMM"));

            if (Directory.Exists(logFolder) == false)
            {
                Directory.CreateDirectory(logFolder);
            }

            return Path.Combine(logFolder, String.Format("{0}{1}.txt", type.ToString(), DateTime.Now.ToString("yyyyMMdd", GetLogCulture())));
        }

        /// <summary>
        /// 보존기간(기본값 90일)이 지난 로그 파일을 폴더 단위로 삭제 합니다.
        /// </summary>
        public static void DeleteLogFile()
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(LogPath);

                if (dInfo.Exists == true)
                {
                    foreach (var dir in dInfo.GetDirectories())
                    {
                        if (DateTime.Today.Subtract(dir.CreationTime).Days > LogFileRetentionDays)
                        {
                            dir.Delete(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
