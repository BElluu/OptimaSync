using OptimaSync.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimaSync
{
    public partial class NotificationForm : Form
    {
        public NotificationForm()
        {
            InitializeComponent();
        }

        public enum notificationAction
        {
            wait,
            start,
            close
        }

        public enum notificationType
        {
            Success,
            Warning,
            Error,
            Informaton
        }

        private NotificationForm.notificationAction action;
        private int x, y;

        private void notificationButton_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = notificationAction.close;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case notificationAction.wait:
                    timer1.Interval = 5000;
                    action = notificationAction.close;
                    break;
                case notificationAction.start:
                    timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = notificationAction.wait;
                        }
                    }
                    break;
                case notificationAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;
                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        public void showNotification(string notificationMessage, notificationType notificationType)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 1; i < 10; i++)
            {
                fname = "notification" + i.ToString();
                NotificationForm alertForm = (NotificationForm)Application.OpenForms[fname];

                if (alertForm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i - 5 * i;
                    this.Location = new Point(this.x, this.y);
                    break;
                }
            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            switch (notificationType)
            {
                case notificationType.Success:
                    this.notificationPicture.Image = Resources.icons8_ok_45px_1;
                    this.BackColor = Color.SeaGreen;
                    break;

                case notificationType.Warning:
                    this.notificationPicture.Image = Resources.icons8_warning_shield_45px;
                    this.BackColor = Color.DarkOrange;
                    break;

                case notificationType.Error:
                    this.notificationPicture.Image = Resources.icons8_sad_cloud_45px;
                    this.BackColor = Color.DarkRed;
                    break;

                case notificationType.Informaton:
                    this.notificationPicture.Image = Resources.info_icon;
                    this.BackColor = Color.RoyalBlue;
                    break;
            }

            this.notificationLabel.Text = notificationMessage;
            this.Show();
            this.action = notificationAction.start;

            this.timer1.Interval = 1;
            timer1.Start();
        }
    }
}
