using BusinessLayer;
using DVLD_Proj.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD_Proj
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        private int _oldLicenseID;
        private int _personID;
        private string _nationalNo;

        private int _ReplacementTypeID;
        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }
        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            ctrDriverLicenseInfoWithFilter1.del += FilterDataBack;

            LoadApplicationInfo();
        }
        public void LoadApplicationInfo(clsLicense ReplacedLicense = null)
        {
            lblAppDate.Text = DateTime.Now.Date.ToShortDateString();

            if (rdbtnDamaged.Checked)
            {
                lblTitle.Text = "Replacement For Damaged License";
                lblAppFees.Text = clsApplicationType.FindApplicationType(4).AppFees.ToString();
                _ReplacementTypeID = 4;
            }
            else
            {
                lblTitle.Text = "Replacement For Lost License";
                lblAppFees.Text = clsApplicationType.FindApplicationType(3).AppFees.ToString();
                _ReplacementTypeID = 3;
            }

            lblCreatedBy.Text = clsGlobalSettings.CurrentUser.UserName;
            
        }
        private void FilterDataBack(object sender, int value)
        {
            if(value != -1)
            {
                _oldLicenseID = value;
                linklblHistory.Enabled = true;

                lblOldLicenseID.Text = _oldLicenseID.ToString();

                _personID = clsDriver.Find((int)clsLicense.Find(_oldLicenseID).DriverID).PersonID;
                _nationalNo = clsPerson.FindPerson(_personID).NationalNo;

                if (!clsLicense.Find(_oldLicenseID).IsActive)
                {
                    MessageBox.Show("The selected license is not active. Please choose an active license.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    btnIssue.Enabled = false;
                    linkLblInfo.Enabled = false;
                    return;
                }

                btnIssue.Enabled = true;
            }
            else
            {
                linklblHistory.Enabled = false;
                lblOldLicenseID.Text = "???";
                _personID = -1;
                _nationalNo = "";
                btnIssue.Enabled = false;
                linkLblInfo.Enabled = false;
            }
            
        }
        private void linkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense newLic = clsLicense.Find(Convert.ToInt32(lblReplacedLicenseID.Text));

            frmLicenseInfo frm = new frmLicenseInfo(newLic);
            frm.ShowDialog();
        }
        private void linklblHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_nationalNo);
            frm.ShowDialog();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to replace this license?", "Confirm Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                clsApplication newApp = GetNewApplication(_ReplacementTypeID);

                if (newApp != null)
                {
                    clsLicense newLicense = GetNewLicense(newApp.ApplicationID);

                    if (newLicense != null)
                    {
                        if (!CompleteApplication(newApp.ApplicationID))
                        {
                            MessageBox.Show("An error occurred while completing the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                        //deactivate old one
                        clsLicense OldLicense = clsLicense.Find(_oldLicenseID);
                        OldLicense.IsActive = false;

                        if (!OldLicense.Save())
                        {
                            MessageBox.Show("An error occurred while deactivating the old license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                        lblLRApplicationID.Text = newLicense.ApplicationID.ToString();
                        lblReplacedLicenseID.Text = newLicense.LicenseID.ToString();

                        MessageBox.Show($"The license has been replaced successfully. License ID: {newLicense.LicenseID}", "Replacement Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        linkLblInfo.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while creating the new license. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("An error occurred while creating the application. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

        }
        private void rdbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnDamaged.Checked)
            {
                lblTitle.Text = "Replacement For Damaged License";
                lblAppFees.Text = clsApplicationType.FindApplicationType(4).AppFees.ToString();
                _ReplacementTypeID = 4;
            }
            else
            {
                lblTitle.Text = "Replacement For Lost License";
                lblAppFees.Text = clsApplicationType.FindApplicationType(3).AppFees.ToString();
                _ReplacementTypeID = 3;
            }
        }
        private clsApplication GetNewApplication(int ApplicationTypeID)
        {
            clsApplication newApp = new clsApplication();

            newApp.ApplicantPersonID = _personID;
            newApp.ApplicationDate = DateTime.Now;
            newApp.ApplicationTypeID = ApplicationTypeID;
            newApp.ApplicationStatus = 0;
            newApp.LastStatusDate = DateTime.Now;
            newApp.PaidFees = (decimal)clsApplicationType.FindApplicationType(ApplicationTypeID).AppFees;
            newApp.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            return newApp.Save() ? newApp : null;
            
        }
        private clsLicense GetNewLicense(int applicationID)
        {
            clsLicense oldLicense = clsLicense.Find(_oldLicenseID);
            clsLicense newLicense = new clsLicense();
            clsLicenseClass LClass = clsLicenseClass.Find(clsLicense.Find(_oldLicenseID).LicenseClass);

            newLicense.ApplicationID = applicationID;
            newLicense.DriverID = oldLicense.DriverID;
            newLicense.LicenseClass = oldLicense.LicenseClass;
            newLicense.IssueDate = oldLicense.IssueDate;
            newLicense.ExpirationDate = oldLicense.ExpirationDate;
            newLicense.Notes = oldLicense.Notes;
            newLicense.PaidFees = Convert.ToInt32(lblAppFees.Text);
            newLicense.IsActive = true;
            
            if(rdbtnDamaged.Checked)
                newLicense.IssueReason = 4;
            else
                newLicense.IssueReason = 3;

            newLicense.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            return (newLicense.Save()) ? newLicense : null;
        }
        private bool CompleteApplication(int applicationID)
        {
            clsApplication app = clsApplication.Find(applicationID);

            app.ApplicationStatus = 3;
            app.LastStatusDate = DateTime.Now;

            return app.Save();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
