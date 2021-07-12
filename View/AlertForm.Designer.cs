
namespace OptimaSync.View
{
    partial class AlertForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertForm));
            this.alertMessageLabel = new System.Windows.Forms.Label();
            this.alertButton = new System.Windows.Forms.Button();
            this.alertPicture = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.alertPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // alertMessageLabel
            // 
            this.alertMessageLabel.AutoSize = true;
            this.alertMessageLabel.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.alertMessageLabel.Location = new System.Drawing.Point(79, 44);
            this.alertMessageLabel.Name = "alertMessageLabel";
            this.alertMessageLabel.Size = new System.Drawing.Size(132, 22);
            this.alertMessageLabel.TabIndex = 0;
            this.alertMessageLabel.Text = "Message Text";
            // 
            // alertButton
            // 
            this.alertButton.FlatAppearance.BorderSize = 0;
            this.alertButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.alertButton.ForeColor = System.Drawing.Color.White;
            this.alertButton.Image = ((System.Drawing.Image)(resources.GetObject("alertButton.Image")));
            this.alertButton.Location = new System.Drawing.Point(355, 38);
            this.alertButton.Name = "alertButton";
            this.alertButton.Size = new System.Drawing.Size(38, 35);
            this.alertButton.TabIndex = 1;
            this.alertButton.UseVisualStyleBackColor = true;
            this.alertButton.Click += new System.EventHandler(this.alertButton_Click);
            // 
            // alertPicture
            // 
            this.alertPicture.Image = ((System.Drawing.Image)(resources.GetObject("alertPicture.Image")));
            this.alertPicture.Location = new System.Drawing.Point(12, 31);
            this.alertPicture.Name = "alertPicture";
            this.alertPicture.Size = new System.Drawing.Size(49, 47);
            this.alertPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.alertPicture.TabIndex = 2;
            this.alertPicture.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AlertForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(415, 107);
            this.Controls.Add(this.alertPicture);
            this.Controls.Add(this.alertButton);
            this.Controls.Add(this.alertMessageLabel);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AlertForm";
            this.Text = "AlertForm";
            ((System.ComponentModel.ISupportInitialize)(this.alertPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label alertMessageLabel;
        private System.Windows.Forms.Button alertButton;
        private System.Windows.Forms.PictureBox alertPicture;
        private System.Windows.Forms.Timer timer1;
    }
}