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
        SearchEDeclarationBuildService searchEDeclaration;
        DownloadServiceHelper downloadHelper;
        SyncUI syncUI;
        public DownloadEDeclarationService(SearchEDeclarationBuildService searchEDeclarationBuildService, DownloadServiceHelper downloadServiceHelper, SyncUI syncUI)
        {
            searchEDeclaration = searchEDeclarationBuildService;
            downloadHelper = downloadServiceHelper;
            this.syncUI = syncUI;
        }

        public bool DownloadEDeclaration(string extractionPath)
        {
            var eDeclarationToDownload = searchEDeclaration.FindLastEDeclarationBuild();
            var files = downloadHelper.filesToCopy(eDeclarationToDownload);
            var declarationDirectory = extractionPath + "\\Deklaracje";

            try
            {
                if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                    !Directory.Exists(declarationDirectory))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(declarationDirectory);
                }

                syncUI.ChangeProgressLabel("Pobieranie e-Deklaracji");
                foreach (string dir in Directory.GetDirectories(eDeclarationToDownload.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dir.Replace(eDeclarationToDownload.ToString(), declarationDirectory));
                }

                syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));
                int i = 0;

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(eDeclarationToDownload.ToString(), declarationDirectory), true);
                    syncUI.ChangeProgressLabel(string.Format("Pobieranie e-Deklaracji" + " {0}/{1}", ++i, files.Length));
                }

                Logger.Write(LogEventLevel.Information, "Skopiowano " + "e-Deklaracje");
                syncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_SUCCESSFUL);
                return true;


            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return false;
            }
        }
    }
}
