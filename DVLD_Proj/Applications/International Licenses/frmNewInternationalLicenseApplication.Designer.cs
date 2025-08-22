namespace DVLD_Proj
{
    partial class frmNewInternationalLicenseApplication
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
            this.linklblHistory = new System.Windows.Forms.LinkLabel();
            this.linkLblInfo = new System.Windows.Forms.LinkLabel();
            this.btnIssue = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrInternationalLicenseApplicationInfo1 = new DVLD_Proj.UserControls.ctrInternationalLicenseApplicationInfo();
            this.ctrDriverLicenseInfoWithFilter1 = new DVLD_Proj.UserControls.ctrDriverLicenseInfoWithFilter();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(193, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(511, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "International License Application";
            // 
            // linklblHistory
            // 
            this.linklblHistory.AutoSize = true;
            this.linklblHistory.Enabled = false;
            this.linklblHistory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.linklblHistory.Location = new System.Drawing.Point(34, 828);
            this.linklblHistory.Name = "linklblHistory";
            this.linklblHistory.Size = new System.Drawing.Size(169, 20);
            this.linklblHistory.TabIndex = 3;
            this.linklblHistory.TabStop = true;
            this.linklblHistory.Text = "Show Licenses History";
            this.linklblHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklblHistory_LinkClicked);
            // 
            // linkLblInfo
            // 
            this.linkLblInfo.AutoSize = true;
            this.linkLblInfo.Enabled = false;
            this.linkLblInfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.linkLblInfo.Location = new System.Drawing.Point(239, 828);
            this.linkLblInfo.Name = "linkLblInfo";
            this.linkLblInfo.Size = new System.Drawing.Size(148, 20);
            this.linkLblInfo.TabIndex = 4;
            this.linkLblInfo.TabStop = true;
            this.linkLblInfo.Text = "Show Licenses Info";
            this.linkLblInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblInfo_LinkClicked);
            // 
            // btnIssue
            // 
            this.btnIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnIssue.Enabled = false;
            this.btnIssue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIssue.Location = new System.Drawing.Point(720, 815);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(139, 56);
            this.btnIssue.TabIndex = 16;
            this.btnIssue.Text = "Issue";
            this.btnIssue.UseVisualStyleBackColor = false;
            this.btnIssue.Click += new System.EventHandler(this.btnIssue_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = global::DVLD_Proj.Properties.Resources.greenClose;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(575, 815);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 56);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrInternationalLicenseApplicationInfo1
            // 
            this.ctrInternationalLicenseApplicationInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ctrInternationalLicenseApplicationInfo1.LocalLicenseID = -1;
            this.ctrInternationalLicenseApplicationInfo1.Location = new System.Drawing.Point(12, 552);
            this.ctrInternationalLicenseApplicationInfo1.Name = "ctrInternationalLicenseApplicationInfo1";
            this.ctrInternationalLicenseApplicationInfo1.Size = new System.Drawing.Size(853, 257);
            this.ctrInternationalLicenseApplicationInfo1.TabIndex = 2;
            // 
            // ctrDriverLicenseInfoWithFilter1
            // 
            this.ctrDriverLicenseInfoWithFilter1.AutoSize = true;
            this.ctrDriverLicenseInfoWithFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ctrDriverLicenseInfoWithFilter1.DLApplicationID = 0;
            this.ctrDriverLicenseInfoWithFilter1.LicenseID = 0;
            this.ctrDriverLicenseInfoWithFilter1.Location = new System.Drawing.Point(12, 71);
            this.ctrDriverLicenseInfoWithFilter1.Name = "ctrDriverLicenseInfoWithFilter1";
            this.ctrDriverLicenseInfoWithFilter1.Size = new System.Drawing.Size(862, 485);
            this.ctrDriverLicenseInfoWithFilter1.TabIndex = 0;
            // 
            // frmNewInternationalLicenseApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.ClientSize = new System.Drawing.Size(868, 889);
            this.Controls.Add(this.btnIssue);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.linkLblInfo);
            this.Controls.Add(this.linklblHistory);
            this.Controls.Add(this.ctrInternationalLicenseApplicationInfo1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrDriverLicenseInfoWithFilter1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(94)))), ((int)(((byte)(32)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmNewInternationalLicenseApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New International License Application";
            this.Load += new System.EventHandler(this.frmNewInternationalLicenseApplication_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControls.ctrDriverLicenseInfoWithFilter ctrDriverLicenseInfoWithFilter1;
        private System.Windows.Forms.Label label1;
        private UserControls.ctrInternationalLicenseApplicationInfo ctrInternationalLicenseApplicationInfo1;
        private System.Windows.Forms.LinkLabel linklblHistory;
        private System.Windows.Forms.LinkLabel linkLblInfo;
        private System.Windows.Forms.Button btnIssue;
        private System.Windows.Forms.Button btnClose;
    }
}