using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace OptimaSync.Common
{
    public static class Logger
    {
        public static void Write(LogEventLevel logEvent, string message)
        {
            Log.Write(logEvent, message);
            Log.CloseAndFlush();
        }

        public static void ConfigureSerilog()
        {
            var logFile = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "OSync\\OSync.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 2097152, retainedFileCountLimit: 7)
                .CreateLogger();
        }
    }
}
