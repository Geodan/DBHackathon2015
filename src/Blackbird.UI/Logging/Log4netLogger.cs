using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Blackbird.WPF.Logging
{
    public static class Log4netLogger
    {
        private static bool Initialized = false;

        #region Log functions WITHOUT format option

        public static void Debug(string message)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Debug, message);
        }

        public static void Debug(string message, int stacktraceFrame)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Debug, message, stacktraceFrame);
        }

        public static void Info(string message)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Info, message);
        }

        public static void Info(string message, int stacktraceFrame)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Info, message, stacktraceFrame);
        }

        public static void Warn(string message)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Warn, message);
        }

        public static void Warn(string message, int stacktraceFrame)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Warn, message, stacktraceFrame);
        }

        public static void Error(string message)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Error, message);
        }

        public static void Error(string message, int stacktraceFrame)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Error, message, stacktraceFrame);
        }

        public static void Fatal(string message)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Fatal, message);
        }

        public static void Fatal(string message, int stacktraceFrame)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            Write2LogFile(LogLevels.Fatal, message, stacktraceFrame);
        }

        #endregion

        #region Log functions WITH format option

        public static void DebugFormat(string format, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Debug, 2, format, args);
        }

        public static void DebugFormat(string format, int stacktraceFrame, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Debug, stacktraceFrame, format, args);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Info, 2, format, args);
        }

        public static void InfoFormat(string format, int stacktraceFrame, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Info, stacktraceFrame, format, args);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Warn, 2, format, args);
        }

        public static void WarnFormat(string format, int stacktraceFrame, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Warn, stacktraceFrame, format, args);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Error, 2, format, args);
        }

        public static void ErrorFormat(string format, int stacktraceFrame, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Error, stacktraceFrame, format, args);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Fatal, 2, format, args);
        }

        public static void FatalFormat(string format, int stacktraceFrame, params object[] args)
        {
            if(!Initialized) InitializeLoggingConfiguration();
            WriteFormat2LogFile(LogLevels.Fatal, stacktraceFrame, format, args);
        }

        #endregion

        private static void Write2LogFile(LogLevels level, string message, int stacktraceFrame = 2)
        {
            var currentStackTrace = new StackTrace(true); //NeedFileInfo = true for displaying line numbers
            var callerType = currentStackTrace.GetFrame(stacktraceFrame).GetMethod().DeclaringType;
            var linenumber = currentStackTrace.GetFrame(stacktraceFrame).GetFileLineNumber();

            var logmessage = String.Format("{0} (line: {1}) - {2}", callerType, linenumber, message);
            var logger = LogManager.GetLogger(callerType);
            
            //Call log method
            if(level == LogLevels.Debug) logger.Debug(logmessage);
            else if(level == LogLevels.Info) logger.Info(logmessage);
            else if(level == LogLevels.Warn) logger.Warn(logmessage);
            else if(level == LogLevels.Error) logger.Error(logmessage);
            else if(level == LogLevels.Fatal) logger.Fatal(logmessage);
        }

        private static void WriteFormat2LogFile(LogLevels level, int stacktraceFrame, string format, params object[] args)
        {
            var currentStackTrace = new StackTrace(true); //NeedFileInfo = true for displaying line numbers
            var callerType = currentStackTrace.GetFrame(stacktraceFrame).GetMethod().DeclaringType;
            var linenumber = currentStackTrace.GetFrame(stacktraceFrame).GetFileLineNumber();

            var logmessage = String.Format("{0} (line: {1}) - {2}", callerType, linenumber, String.Format(format, args));
            var logger = LogManager.GetLogger(callerType);

            //Call log method
            if(level == LogLevels.Debug) logger.Debug(logmessage);
            else if(level == LogLevels.Info) logger.Info(logmessage);
            else if(level == LogLevels.Warn) logger.Warn(logmessage);
            else if(level == LogLevels.Error) logger.Error(logmessage);
            else if(level == LogLevels.Fatal) logger.Fatal(logmessage);
        }

        private static void InitializeLoggingConfiguration()
        {
            if(!Initialized)
            {
                XmlConfigurator.Configure(new FileInfo(Assembly.GetEntryAssembly().Location + ".config"));
                Initialized = true;
            }
        }
    }
}