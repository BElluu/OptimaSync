using OptimaSync.Common;
using OptimaSync.Constant;
using System.Windows.Forms;
using Serilog.Events;

namespace OptimaSync.UI
{
    public class ValidatorUI
    {
        public static bool DestPathIsValid()
        {
            string DestPath = MainForm.Instance.DestTextBox.Text;
            if (string.IsNullOrEmpty(DestPath))
            {
                MessageBox.Show(Messages.DEST_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Write(LogEventLevel.Error, Messages.DEST_PATH_CANNOT_BE_EMPTY);
                return false;
            }
            return true;
        }

        public static bool DestSOAPathIsValid()
        {
            string SOAPath = MainForm.Instance.OptimaSOATextBox.Text;
            if (string.IsNullOrEmpty(SOAPath))
            {
                MessageBox.Show(Messages.SOA_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Write(LogEventLevel.Error, Messages.SOA_PATH_CANNOT_BE_EMPTY);
                return false;
            }
            return true;
        }
    }
}
