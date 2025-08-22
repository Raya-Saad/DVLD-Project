namespace DVLD_Proj
{
    partial class frmRenewLocalDrivingLicense
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnRenew = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.linkLblNewInfo = new System.Windows.Forms.LinkLabel();
            this.linklblHistory = new System.Windows.Forms.LinkLabel();
            this.linkLblInfo = new System.Windows.Forms.LinkLabel();
            this.ctrRenewApplicationInfo1 = new DVLD_Proj.UserControls.ctrRenewApplicationInfo();
            this.ctrDriverLicenseInfoWithFilter1 = new DVLD_Proj.UserControls.ctrDriverLicenseInfoWithFilter();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 18F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.label1.Location = new System.Drawing.Point(201, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(467, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "Renew License Application";
            // 
            // btnRenew
            // 
            this.btnRenew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnRenew.Enabled = false;
            this.btnRenew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.btnRenew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenew.Location = new System.Drawing.Point(701, 903);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(139, 56);
            this.btnRenew.TabIndex = 20;
            this.btnRenew.Text = "Renew";
            this.btnRenew.UseVisualStyleBackColor = false;
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.btnClose.Image = global::DVLD_Proj.Properties.Resources.greenClose;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(556, 903);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 56);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // linkLblNewInfo
            // 
            this.linkLblNewInfo.AutoSize = true;
            this.linkLblNewInfo.Enabled = false;
            this.linkLblNewInfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.linkLblNewInfo.Location = new System.Drawing.Point(256, 916);
            this.linkLblNewInfo.Name = "linkLblNewInfo";
            this.linkLblNewInfo.Size = new System.Drawing.Size(183, 20);
            this.linkLblNewInfo.TabIndex = 19;
            this.linkLblNewInfo.TabStop = true;
            this.linkLblNewInfo.Text = "Show New Licenses Info";
            this.linkLblNewInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblInfo_LinkClicked);
            // 
            // linklblHistory
            // 
            this.linklblHistory.AutoSize = true;
            this.linklblHistory.Enabled = false;
            this.linklblHistory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.linklblHistory.Location = new System.Drawing.Point(51, 916);
            this.linklblHistory.Name = "linklblHistory";
            this.linklblHistory.Size = new System.Drawing.Size(169, 20);
            this.linklblHistory.TabIndex = 18;
            this.linklblHistory.TabStop = true;
            this.linklblHistory.Text = "Show Licenses History";
            this.linklblHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklblHistory_LinkClicked);
            // 
            // linkLblInfo
            // 
            this.linkLblInfo.AutoSize = true;
            this.linkLblInfo.Enabled = false;
            this.linkLblInfo.Location = new System.Drawing.Point(256, 916);
            this.linkLblInfo.Name = "linkLblInfo";
            this.linkLblInfo.Size = new System.Drawing.Size(183, 20);
            this.linkLblInfo.TabIndex = 19;
            this.linkLblInfo.TabStop = true;
            this.linkLblInfo.Text = "Show New Licenses Info";
            // 
            // ctrRenewApplicationInfo1
            // 
            this.ctrRenewApplicationInfo1.Location = new System.Drawing.Point(0, 541);
            this.ctrRenewApplicationInfo1.Name = "ctrRenewApplicationInfo1";
            this.ctrRenewApplicationInfo1.Notes = null;
            this.ctrRenewApplicationInfo1.OldLicenseID = 0;
            this.ctrRenewApplicationInfo1.Size = new System.Drawing.Size(845, 356);
            this.ctrRenewApplicationInfo1.TabIndex = 2;
            // 
            // ctrDriverLicenseInfoWithFilter1
            // 
            this.ctrDriverLicenseInfoWithFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ctrDriverLicenseInfoWithFilter1.DLApplicationID = 0;
            this.ctrDriverLicenseInfoWithFilter1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.ctrDriverLicenseInfoWithFilter1.LicenseID = 0;
            this.ctrDriverLicenseInfoWithFilter1.Location = new System.Drawing.Point(0, 62);
            this.ctrDriverLicenseInfoWithFilter1.Name = "ctrDriverLicenseInfoWithFilter1";
            this.ctrDriverLicenseInfoWithFilter1.Size = new System.Drawing.Size(845, 485);
            this.ctrDriverLicenseInfoWithFilter1.TabIndex = 0;
            this.ctrDriverLicenseInfoWithFilter1.Load += new System.EventHandler(this.ctrDriverLicenseInfoWithFilter1_Load);
            // 
            // frmRenewLocalDrivingLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(845, 965);
            this.Controls.Add(this.btnRenew);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.linkLblNewInfo);
            this.Controls.Add(this.linklblHistory);
            this.Controls.Add(this.ctrRenewApplicationInfo1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrDriverLicenseInfoWithFilter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRenewLocalDrivingLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Renew Local Driving License";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControls.ctrDriverLicenseInfoWithFilter ctrDriverLicenseInfoWithFilter1;
        private System.Windows.Forms.Label label1;
        private UserControls.ctrRenewApplicationInfo ctrRenewApplicationInfo1;
        private System.Windows.Forms.Button btnRenew;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel linkLblNewInfo;
        private System.Windows.Forms.LinkLabel linklblHistory;
        private System.Windows.Forms.LinkLabel linkLblInfo;
    }
}