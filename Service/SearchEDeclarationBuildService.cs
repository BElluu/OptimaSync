using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;

namespace OptimaSync.Service
{
    public class SearchEDeclarationBuildService
    {
        protected SearchEDeclarationBuildService()
        {
        }
        public static DirectoryInfo FindLastEDeclarationBuild()
        {
            try
            {
                SyncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var eDeclarationLocation = new DirectoryInfo(AppConfigHelper.GetConfigValue("eDeclarationPath"));
                var lastEDeclarationBuildFirstStage = eDeclarationLocation.GetDirectories()
                    .Where(q => !q.Name.Contains("Deklaracje", StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                var lastEDeclarationBuildSecondStage = lastEDeclarationBuildFirstStage.GetDirectories()
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                var lastEDeclarationBuildDirectory = lastEDeclarationBuildSecondStage.GetDirectories()
                    .First(q => q.Name.Contains("unpacked"));

                return lastEDeclarationBuildDirectory;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                SyncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.notificationType.Error));
                return null;
            }
        }
    }
}
