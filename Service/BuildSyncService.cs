using System;
using System.IO;
using OptimaSync.Constant;
using OptimaSync.UI;
using OptimaSync.Helper;
using OptimaSync.Common;
using Serilog.Events;
using System.Threading.Tasks;
using System.Threading;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        SyncUI syncUI;
        RegisterOptimaService registerDLL;
        BuildSyncServiceHelper buildSyncHelper;
        SearchBuildService searchBuild;

        public BuildSyncService(SyncUI syncUI, RegisterOptimaService registerDLL, BuildSyncServiceHelper buildSyncHelper, SearchBuildService searchBuild)
        {
            this.syncUI = syncUI;
            this.registerDLL = registerDLL;
            this.buildSyncHelper = buildSyncHelper;
            this.searchBuild = searchBuild;
        }

        public async void GetOptimaBuild()
        {
            syncUI.EnableElementsOnForm(false);
            var lastBuildDir = searchBuild.FindLastBuild();
            string extractionPath = buildSyncHelper.ChooseExtractionPath(lastBuildDir);

            try
            {
                if (lastBuildDir == null || string.IsNullOrEmpty(extractionPath) || haveLatestVersion(lastBuildDir, extractionPath))
                {
                    syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return;
                }

                await DownloadOptimaFiles(lastBuildDir, extractionPath);
                registerDLL.RegisterOptima(extractionPath);
                /*            if(DownloadBuild(dir, extractionPath))
                            {
                                registerDLL.RegisterOptima(extractionPath);
                            }*/
                /*            var downloadResult = await DownloadBuildAsync(dir, extractionPath);
                            if (downloadResult)
                            {
                                registerDLL.RegisterOptima(extractionPath);
                            }*/
            }
            catch
            {
                syncUI.EnableElementsOnForm(true);
            }
            finally
            {
                syncUI.EnableElementsOnForm(true);
            }
        }

        public async Task DownloadOptimaFiles(DirectoryInfo lastBuildDir, string extractionPath)
        {
            var files = filesToCopy(lastBuildDir);
            int i = 0;

            if (lastBuildDir == null || extractionPath == null)
            {
                return;
            }

            if (!buildSyncHelper.DoesLockFileExist(extractionPath) &&
                buildSyncHelper.BuildVersionsAreSame(lastBuildDir.ToString(), lastBuildDir.Name))
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                Logger.Write(LogEventLevel.Information, Messages.YOU_HAVE_LATEST_BUILD);
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return;
            }
            /*            Func<string, bool> downloadStatus = (currFile) => {

                            string currentFilename = Path.GetFileName(currFile);

                            string targetFilePath = extractionPath + "\\" + currentFilename;
                            try
                            {
                                syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                                foreach (string dirPath in Directory.GetDirectories(lastBuildDir.ToString(), "*", SearchOption.AllDirectories))
                                {
                                    Directory.CreateDirectory(dirPath.Replace(lastBuildDir.ToString(), extractionPath));
                                }
                                //syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));

                                File.Copy(currFile, targetFilePath, true);
                                syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, files.Length));
                                Logger.Write(LogEventLevel.Information, "Skopiowano " + lastBuildDir.Name);
                            }
                            catch (Exception ex)
                            {
                                Logger.Write(LogEventLevel.Error, ex.Message);
                                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                                return false;
                            }
                            return true;
                        };*/

            await Task.Run(() =>
            {
                Parallel.ForEach(files,
                 new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 }, (currFile) =>
                  {

                      string currentFilename = Path.GetFileName(currFile);

                      string targetFilePath = extractionPath + "\\" + currentFilename;
                      try
                      {
                          syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                          foreach (string dirPath in Directory.GetDirectories(lastBuildDir.ToString(), "*", SearchOption.AllDirectories))
                          {
                              Directory.CreateDirectory(dirPath.Replace(lastBuildDir.ToString(), extractionPath));
                          }
                          //syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));

                          File.Copy(currFile, targetFilePath, true);
                          syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, files.Length));
                          Logger.Write(LogEventLevel.Information, "Skopiowano " + lastBuildDir.Name);
                      }
                      catch (Exception ex)
                      {
                          Logger.Write(LogEventLevel.Error, ex.Message);
                          syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                          SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                      }
                  });
            });
        }

        private string[] filesToCopy(DirectoryInfo lastBuildDir)
        {
            return Directory.GetFiles(lastBuildDir.ToString(), "*.*", SearchOption.AllDirectories);
        }

        private bool haveLatestVersion(DirectoryInfo lastBuildDir, string extractionPath)
        {
            if (!buildSyncHelper.DoesLockFileExist(extractionPath) &&
                buildSyncHelper.BuildVersionsAreSame(lastBuildDir.ToString(), lastBuildDir.Name))
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                Logger.Write(LogEventLevel.Information, Messages.YOU_HAVE_LATEST_BUILD);
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return true;
            }
            return false;
        }

        /*        public bool DownloadBuild(DirectoryInfo lastBuildDir, string extractionPath)
                {
                    if (lastBuildDir == null || extractionPath == null)
                    {
                        return false;
                    }

                    if (!buildSyncHelper.DoesLockFileExist(extractionPath) &&
                        buildSyncHelper.BuildVersionsAreSame(lastBuildDir.ToString(), lastBuildDir.Name))
                    {
                        SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                        Logger.Write(LogEventLevel.Information ,Messages.YOU_HAVE_LATEST_BUILD);
                        syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                        return false;
                    }

                    if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                            !Directory.Exists(extractionPath))
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory(extractionPath);
                    }

                    if (!buildSyncHelper.DoesLockFileExist(extractionPath))
                    {
                        buildSyncHelper.CreateLockFile(extractionPath);
                    }

                    try
                    {
                        if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                            !Directory.Exists(extractionPath))
                        {
                            DirectoryInfo directoryInfo = Directory.CreateDirectory(extractionPath);
                        }

                        if (!buildSyncHelper.DoesLockFileExist(extractionPath))
                        {
                            buildSyncHelper.CreateLockFile(extractionPath);
                        }

                        syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                        foreach (string dirPath in Directory.GetDirectories(lastBuildDir.ToString(), "*", SearchOption.AllDirectories))
                        {
                            Directory.CreateDirectory(dirPath.Replace(lastBuildDir.ToString(), extractionPath));
                        }

                        string[] filesToCopy = Directory.GetFiles(lastBuildDir.ToString(), "*.*", SearchOption.AllDirectories);
                        syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, filesToCopy.Length));
                        int i = 0;

                        foreach (string newPath in filesToCopy)
                        {
                            File.Copy(newPath, newPath.Replace(lastBuildDir.ToString(), extractionPath), true);
                            syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, filesToCopy.Length));
                        }

                        Logger.Write(LogEventLevel.Information, "Skopiowano " + lastBuildDir.Name);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(LogEventLevel.Error, ex.Message);
                        syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                        SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                        return false;
                    }
                }*/
/*        public async Task<bool> DownloadBuildAsync(DirectoryInfo lastBuildDir, string extractionPath)
        {
            if (lastBuildDir == null || extractionPath == null)
            {
                return false;
            }

            if (!buildSyncHelper.DoesLockFileExist(extractionPath) &&
                buildSyncHelper.BuildVersionsAreSame(lastBuildDir.ToString(), lastBuildDir.Name))
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                Logger.Write(LogEventLevel.Information, Messages.YOU_HAVE_LATEST_BUILD);
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                    !Directory.Exists(extractionPath))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(extractionPath);
            }

            if (!buildSyncHelper.DoesLockFileExist(extractionPath))
            {
                buildSyncHelper.CreateLockFile(extractionPath);
            }

            using (var throttler = new SemaphoreSlim(4))
            {
                Task<bool> copyFiles = Task.Run(async () =>
                {
                    await throttler.WaitAsync();
                    try
                    {
                        syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                        foreach (string dirPath in Directory.GetDirectories(lastBuildDir.ToString(), "*", SearchOption.AllDirectories))
                        {
                            Directory.CreateDirectory(dirPath.Replace(lastBuildDir.ToString(), extractionPath));
                        }

                        string[] filesToCopy = Directory.GetFiles(lastBuildDir.ToString(), "*.*", SearchOption.AllDirectories);
                        syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, filesToCopy.Length));
                        int i = 0;

                        foreach (string newPath in filesToCopy)
                        {
                            File.Copy(newPath, newPath.Replace(lastBuildDir.ToString(), extractionPath), true);
                            syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, filesToCopy.Length));
                        }
                        Logger.Write(LogEventLevel.Information, "Skopiowano " + lastBuildDir.Name);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(LogEventLevel.Error, ex.Message);
                        syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                        SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                        return false;
                    }
                    finally
                    {
                        throttler.Release();
                    }
                });
                await Task.WhenAll(copyFiles);
                return true;
            }
        }*/
        }
}
