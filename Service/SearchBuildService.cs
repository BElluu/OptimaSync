using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.UI;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OptimaSync.Service
{
    internal class SearchBuildService
    {
        static string[] EXCLUDED_STRINGS = { "CIV", "SQL", "test", "rar", "FIXES" };
        SyncUI syncUI = new SyncUI();
        internal DirectoryInfo FindLastBuild()
        {

            if (!NetworkDrive.HaveAccessToHost("natalie"))
            {
                MessageBox.Show(Messages.ACCESS_TO_HOST_ERROR, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(Messages.ACCESS_TO_HOST_ERROR);
                return null;
            }

            try
            {
                syncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var directory = new DirectoryInfo(Properties.Settings.Default.BuildSourcePath);
                var lastBuild = directory.GetDirectories()
                    .Where(q => EXCLUDED_STRINGS.All(c => !q.Name.Contains(c, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastBuild;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                return null;
            }
        }
    }
}
