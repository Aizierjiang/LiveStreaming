using Serilog;
using System.Collections.Generic;
#if UNITY_ANDROID && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace Logger
{
    public class Log
    {
        private const int ANDROID_LOG_VERBOSE = 2;
        private const int ANDROID_LOG_DEBUG = 3;
        private const int ANDROID_LOG_INFO = 4;
        private const int ANDROID_LOG_WARN = 5;
        private const int ANDROID_LOG_ERROR = 6;

        private static Dictionary<string, Serilog.ILogger> ILoggers = new Dictionary<string, Serilog.ILogger>();

#if UNITY_ANDROID && !UNITY_EDITOR
		[DllImportAttribute("log", EntryPoint = "__android_log_print", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int __android_log_print(int prio, string tag, string fmt, System.IntPtr ptr);
#else
        private static int __android_log_print(int prio, string tag, string fmt, System.IntPtr ptr)
        {
            return 0;
        }
#endif


        //Customized log using constructor --- by Aizierjiang
        public Log(object message, UnityEngine.LogType logType = LogType.Log, string color = "blue")
        {
            switch (logType)
            {
                case LogType.Log:
                    UnityEngine.Debug.Log("<color=" + color + ">" + message + "</color>");
                    break;
                case LogType.Warning:
                    if (color.Equals("blue")) color = "yellow";
                    UnityEngine.Debug.LogWarning("<color=" + color + ">" + message + "</color>");
                    break;
                case LogType.Error:
                    if (color.Equals("blue")) color = "red";
                    UnityEngine.Debug.LogError("<color=" + color + ">" + message + "</color>");
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="message">message</param>
        /// <param name="logInFile">是否记录到文件</param>
        /// <param name="logInEditor">是否在Editor中显示</param>
        /// <param name="loggerType"></param>
        public static void d(string tag, string message, bool logInFile = false, bool logInEditor = false, int loggerType = 0)
        {
            __android_log_print(ANDROID_LOG_DEBUG, tag, message, System.IntPtr.Zero);
#if UNITY_EDITOR
            if (logInEditor)
                Debug.Log(tag + " " + message);
#endif
            if (logInFile)
            {
                Serilog.ILogger logger = null;
                if (ILoggers.TryGetValue(tag, out logger))
                {
                    logger.Debug("{0}: {1}", tag, message);
                }
                else //缺少该Log的信息
                {
                    string logFile = Application.persistentDataPath + "/appFolder/db/" + tag + ".txt";
                    Serilog.ILogger ilogger = new LoggerConfiguration()
                                                .MinimumLevel.Debug()
                                                .WriteTo.File(logFile, rollingInterval: RollingInterval.Hour)
                                                .CreateLogger();
                    ilogger.Debug("{0}: {1}", tag, message);
                    ILoggers.Add(tag, ilogger);

                }
            }
        }

        public static void i(string tag, string message, bool logInFile = false, bool logInEditor = false)
        {
            __android_log_print(ANDROID_LOG_INFO, tag, message, System.IntPtr.Zero);
#if UNITY_EDITOR
            if (logInEditor)
                Debug.Log(tag + " " + message);
#endif
            if (logInFile)
            {
                Serilog.ILogger logger = null;
                if (ILoggers.TryGetValue(tag, out logger))
                {
                    logger.Information("{0}: {1}", tag, message);
                }
                else //缺少该Log的信息
                {
                    string logFile = Application.persistentDataPath + "/appFolder/db/" + tag + ".txt";
                    Serilog.ILogger ilogger = new LoggerConfiguration()
                                                .MinimumLevel.Debug()
                                                .WriteTo.File(logFile, rollingInterval: RollingInterval.Hour)
                                                .CreateLogger();
                    ilogger.Information("{0}: {1}", tag, message);
                    ILoggers.Add(tag, ilogger);

                }
            }
        }

        public static void w(string tag, string message, bool logInFile = false, bool logInEditor = false)
        {
            __android_log_print(ANDROID_LOG_WARN, tag, message, System.IntPtr.Zero);
#if UNITY_EDITOR
            if (logInEditor)
                Debug.LogWarning(tag + " " + message);
#endif
            if (logInFile)
            {
                Serilog.ILogger logger = null;
                if (ILoggers.TryGetValue(tag, out logger))
                {
                    logger.Warning("{0}: {1}", tag, message);
                }
                else //缺少该Log的信息
                {
                    string logFile = Application.persistentDataPath + "/appFolder/db/" + tag + ".txt";
                    Serilog.ILogger ilogger = new LoggerConfiguration()
                                                .MinimumLevel.Debug()
                                                .WriteTo.File(logFile, rollingInterval: RollingInterval.Hour)
                                                .CreateLogger();
                    ilogger.Warning("{0}: {1}", tag, message);
                    ILoggers.Add(tag, ilogger);

                }
            }
        }

        public static void e(string tag, string message, bool logInFile = false, bool logInEditor = false)
        {
            __android_log_print(ANDROID_LOG_ERROR, tag, message, System.IntPtr.Zero);
#if UNITY_EDITOR
            if (logInEditor)
                Debug.LogError(tag + " " + message);
#endif
            if (logInFile)
            {
                Serilog.ILogger logger = null;
                if (ILoggers.TryGetValue(tag, out logger))
                {
                    logger.Information("{0}: {1}", tag, message);
                }
                else //缺少该Log的信息
                {
                    string logFile = Application.persistentDataPath + "/appFolder/db/" + tag + ".txt";
                    Serilog.ILogger ilogger = new LoggerConfiguration()
                                                .MinimumLevel.Debug()
                                                .WriteTo.File(logFile, rollingInterval: RollingInterval.Hour)
                                                .CreateLogger();
                    ilogger.Error("{0}: {1}", tag, message);
                    ILoggers.Add(tag, ilogger);

                }
            }
        }
    }
}