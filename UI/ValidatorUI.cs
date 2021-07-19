using OptimaSync.Constant;
using OptimaSync.Service;
using Serilog;
using System;
using System.Windows.Forms;

namespace OptimaSync.UI
{
    public class ValidatorUI
    {
        WindowsService windowsService = new WindowsService();
        SyncUI syncUI = new SyncUI();
        public bool DestPathIsValid()
        {
            string DestPath = MainForm.Instance.DestTextBox.Text;
            if (string.IsNullOrEmpty(DestPath))
            {
                MessageBox.Show(Messages.DEST_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                return false;
            }
            return true;
        }

        public bool DestSOAPathIsValid()
        {
            string SOAPath = MainForm.Instance.OptimaSOATextBox.Text;
            if (string.IsNullOrEmpty(SOAPath))
            {
                MessageBox.Show(Messages.SOA_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                return false;
            }
            return true;
        }

        public bool WithSOASupport()
        {
            CheckBox SOACheckBox = MainForm.Instance.SOACheckBox;
            if (SOACheckBox.Checked)
            {
                return true;
            }
            return false;

        }

        public bool isProgrammer()
        {
            CheckBox ProgrammerCheckBox = MainForm.Instance.programmerCheckbox;
            if (ProgrammerCheckBox.Checked)
            {
                return true;
            }
            return false;
        }

        public bool SOARequirementsAreMet()
        {
            if (!DestSOAPathIsValid())
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
                throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
            }

            if (!windowsService.DoesSOAServiceExist())
            {
                Log.Error(Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (windowsService.StopSOAService() != 0)
            {
                Log.Error(Messages.SOA_SERVICE_NOT_STOPPED);
                MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
