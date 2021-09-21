
namespace OptimaSync
{
    partial class NotificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationForm));
            this.notificationLabel = new System.Windows.Forms.Label();
            this.notificationButton = new System.Windows.Forms.Button();
            this.notificationPicture = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.notificationPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // notificationLabel
            // 
            this.notificationLabel.AutoSize = true;
            this.notificationLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.notificationLabel.Location = new System.Drawing.Point(71, 26);
            this.notificationLabel.Name = "notificationLabel";
            this.notificationLabel.Size = new System.Drawing.Size(116, 21);
            this.notificationLabel.TabIndex = 0;
            this.notificationLabel.Text = "Message Text";
            // 
            // notificationButton
            // 
            this.notificationButton.FlatAppearance.BorderSize = 0;
            this.notificationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.notificationButton.ForeColor = System.Drawing.Color.White;
            this.notificationButton.Image = ((System.Drawing.Image)(resources.GetObject("notificationButton.Image")));
            this.notificationButton.Location = new System.Drawing.Point(351, 21);
            this.notificationButton.Name = "notificationButton";
            this.notificationButton.Size = new System.Drawing.Size(25, 26);
            this.notificationButton.TabIndex = 1;
            this.notificationButton.UseVisualStyleBackColor = true;
            this.notificationButton.Click += new System.EventHandler(this.notificationButton_Click);
            // 
            // notificationPicture
            // 
            this.notificationPicture.Image = ((System.Drawing.Image)(resources.GetObject("notificationPicture.Image")));
            this.notificationPicture.Location = new System.Drawing.Point(13, 17);
            this.notificationPicture.Name = "notificationPicture";
            this.notificationPicture.Size = new System.Drawing.Size(52, 39);
            this.notificationPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.notificationPicture.TabIndex = 2;
            this.notificationPicture.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // NotificationForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(387, 72);
            this.Controls.Add(this.notificationPicture);
            this.Controls.Add(this.notificationButton);
            this.Controls.Add(this.notificationLabel);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NotificationForm";
            this.Text = "NotificationForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.notificationPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label notificationLabel;
        private System.Windows.Forms.Button notificationButton;
        private System.Windows.Forms.PictureBox notificationPicture;
        private System.Windows.Forms.Timer timer1;
    }
}