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
        SyncUI syncUI;
        public SearchEDeclarationBuildService(SyncUI syncUI)
        {
            this.syncUI = syncUI;
        }
        public DirectoryInfo FindLastEDeclarationBuild()
        {
            try
            {
                syncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var eDeclarationLocation = new DirectoryInfo(AppConfigHelper.GetConfigValue("eDeclarationPath"));
                var lastEDeclarationBuildFirstStage = eDeclarationLocation.GetDirectories()
                    .Where(q => !q.Name.Contains("Deklaracje", StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                var lastEDeclarationBuildSecondStage = lastEDeclarationBuildFirstStage.GetDirectories()
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                var lastEDeclarationBuildDirectory = lastEDeclarationBuildSecondStage.GetDirectories()
                    .Where(q => q.Name.Contains("unpacked"))
                    .First();

                return lastEDeclarationBuildDirectory;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return null;
            }
        }
    }
}
