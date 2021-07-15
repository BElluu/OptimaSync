﻿using OptimaSync.Constant;
using Serilog;
using System.Windows.Forms;
namespace OptimaSync.UI
{
    public class ValidatorUI
    {
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
    }
}
