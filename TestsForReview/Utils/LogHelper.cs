using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace TestsForReview.Utils
{
    public static class LogHelper
    {
        public static Logger ConfigLog()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Async(a => a.File(FsUtils.GetPath("./log") + "/Log.txt",
                    LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    buffered: true))
                .WriteTo.Async(a => a.File(FsUtils.GetPath("./log") + "/Error.txt",
                    LogEventLevel.Error,
                    rollingInterval: RollingInterval.Day,
                    buffered: false))

                .CreateLogger();
            Log.Logger = logger;
            return logger;
        }

        public static ILogger GetLog<T>(this T instance)
        {
            if (Serilog.Log.Logger == null)
                ConfigLog();
            return Serilog.Log.Logger.ForContext(typeof(T));

        }
        public static ILogger GetLog(Type type)
        {
            if (Serilog.Log.Logger == null)
                ConfigLog();
            return Serilog.Log.Logger.ForContext(type);
        }

        public static void Error(Exception exception)
        {
            GetLog(typeof(object)).Error(exception, "Fail");
        }
    }
}
