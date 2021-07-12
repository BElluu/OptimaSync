using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimaSync.View
{
    public partial class AlertForm : Form
    {
        public AlertForm()
        {
            InitializeComponent();
        }

        public enum enumAction
        {
            wait,
            start,
            close
        }

        private AlertForm.enumAction action;
        private int x, y;

        private void alertButton_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enumAction.close;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case enumAction.wait:
                    timer1.Interval = 5000;
                    action = enumAction.close;
                    break;
                case enumAction.start:
                    timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if(this.Opacity == 1.0)
                        {
                            action = enumAction.wait;
                        }
                    }
                    break;
                case enumAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;
                    this.Left -= 3;
                    if(base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        public void showAlert(string alertMsg)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 0; i < 10; i++)
            {
                fname = "alert" + i.ToString();
                AlertForm alertForm = (AlertForm)Application.OpenForms[fname];

                if (alertForm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i; // first notification under taskbar
                    this.Location = new Point(this.x, this.y);
                    break;
                }
            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            this.alertMessageLabel.Text = alertMsg;
            this.Show();
            this.action = enumAction.start;

            this.timer1.Interval = 1;
            timer1.Start();
        }
    }
}
