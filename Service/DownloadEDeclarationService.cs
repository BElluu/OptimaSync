using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using Serilog.Events;
using System;
using System.IO;

namespace OptimaSync.Service
{
    public class DownloadEDeclarationService
    {
        public DownloadEDeclarationService()
        {
        }

        public bool DownloadEDeclaration(string extractionPath)
        {
            var eDeclarationToDownload = SearchEDeclarationBuildService.FindLastEDeclarationBuild();
            var files = DownloadServiceHelper.filesToCopy(eDeclarationToDownload);
            var declarationDirectory = extractionPath + "\\Deklaracje";

            try
            {
                if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.BASIC.ToString() &&
                    !Directory.Exists(declarationDirectory))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(declarationDirectory);
                }

                SyncUI.ChangeProgressLabel("Pobieranie e-Deklaracji");
                foreach (string dir in Directory.GetDirectories(eDeclarationToDownload.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dir.Replace(eDeclarationToDownload.ToString(), declarationDirectory));
                }

                SyncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));
                int i = 0;

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(eDeclarationToDownload.ToString(), declarationDirectory), true);
                    SyncUI.ChangeProgressLabel(string.Format("Pobieranie e-Deklaracji" + " {0}/{1}", ++i, files.Length));
                }

                Logger.Write(LogEventLevel.Information, "Skopiowano " + "e-Deklaracje");
                SyncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_SUCCESSFUL);
                return true;


            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                SyncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.notificationType.Error));
                return false;
            }
        }
    }
}
