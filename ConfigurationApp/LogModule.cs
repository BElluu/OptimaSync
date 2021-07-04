using Serilog;
using System;
using System.IO;

namespace OptimaSync.ConfigurationApp
{
    class LogModule
    {
        public void log()
        {
            var logFile = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "OSync.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("----LOG START----");
        }
    }
}
