using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimaSync
{
    public class ApplicationController: WindowsFormsApplicationBase
    {
        private MainForm mainForm;

        public ApplicationController(MainForm form)
        {
            mainForm = form;
            this.IsSingleInstance = true;
            this.StartupNextInstance += StartNextInstance;
        }

        private void StartNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            e.BringToForeground = true;
            mainForm.showUpTrayWhenNewInstance();
        }

        protected override void OnCreateMainForm()
        {
            this.MainForm = mainForm;
        }

    }
}
