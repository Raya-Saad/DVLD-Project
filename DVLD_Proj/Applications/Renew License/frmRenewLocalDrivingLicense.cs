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

namespace DVLD_Proj
{
    public partial class frmRenewLocalDrivingLicense : Form
    {
        enum enMode { NewApplication, IssuedLicense }

        private enMode _mode;
        private int _licenseID;
        private int _personID;
        private string _nationalNo;

        private clsLocalDrivingLicenseApp _LocalDrivingLicenseApp;
        private clsLicense _newLicense;

        public frmRenewLocalDrivingLicense()
        {
            InitializeComponent();

            _mode = enMode.NewApplication;
        }

        private void ctrDriverLicenseInfoWithFilter1_Load(object sender, EventArgs e)
        {
            ctrDriverLicenseInfoWithFilter1.del += FilterDataBack;
        }

        private void FilterDataBack(object sender, int value)
        {
            if(value != -1)
            {
                _licenseID = value;
                linklblHistory.Enabled = true;

                ctrRenewApplicationInfo1.OldLicenseID = value;
                ctrRenewApplicationInfo1.LoadInfo();

                clsLicense LocalLicense = clsLicense.Find(_licenseID);

                _personID = clsDriver.Find((int)clsLicense.Find(_licenseID).DriverID).PersonID;
                _nationalNo = clsPerson.FindPerson(_personID).NationalNo;

                bool isExpiredLicense = ((DateTime.Compare(DateTime.Now.Date, LocalLicense.ExpirationDate.Date) >= 0) ? true : false);

                if (!isExpiredLicense)
                {
                    MessageBox.Show($"The selected license has not expired yet. It will expire on: {LocalLicense.ExpirationDate:dd/MM/yyyy}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    btnRenew.Enabled = false;
                    return;
                }

                if (clsLicense.Find(_licenseID).IsActive == false)
                {
                    MessageBox.Show("The selected license is not active. Please select the most recently issued license for this person.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    btnRenew.Enabled = false;
                    return;
                }

                btnRenew.Enabled = true;
            }
            else
            {
                linklblHistory.Enabled = false;
                linkLblInfo.Enabled = false;
                btnRenew.Enabled = false;
                _personID = -1;
                _nationalNo = "";
            }
            
        }

        private void linklblHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_nationalNo);
            frm.ShowDialog();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to renew this license?", "Confirm Renewal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _LocalDrivingLicenseApp = GetNewLocalDrivingLicenseApplication();

                if (_LocalDrivingLicenseApp != null)
                {
                    clsLicense newLicense = GetNewLicense(_LocalDrivingLicenseApp.ApplicationID);

                    if (newLicense != null)
                    {
                        if (!CompleteApplication(_LocalDrivingLicenseApp.ApplicationID))
                        {
                            MessageBox.Show("An error occurred while completing the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                        clsLicense OldLicense = clsLicense.Find(_licenseID);
                        OldLicense.IsActive = false;

                        if (!OldLicense.Save())
                        {
                            MessageBox.Show("An error occurred while deactivating the old license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                        _mode = enMode.IssuedLicense;
                        _newLicense = newLicense;
                        ctrRenewApplicationInfo1.LoadInfo(newLicense);

                        MessageBox.Show($"The license has been renewed successfully. License ID: {newLicense.LicenseID}", "Renewal Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        linkLblNewInfo.Enabled = true;
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
        private void linkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_LocalDrivingLicenseApp.LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
        }

        private clsLocalDrivingLicenseApp GetNewLocalDrivingLicenseApplication()
        {
            clsApplication newApp = new clsApplication();

            newApp.ApplicantPersonID = _personID;
            newApp.ApplicationDate = DateTime.Now;
            newApp.ApplicationTypeID = 2;
            newApp.ApplicationStatus = 0;
            newApp.LastStatusDate = DateTime.Now;
            newApp.PaidFees = (decimal)clsApplicationType.FindApplicationType(2).AppFees;
            newApp.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if(newApp.Save())
            {
                clsLocalDrivingLicenseApp newLocalDrivingLicenseApp = new clsLocalDrivingLicenseApp();
                newLocalDrivingLicenseApp.ApplicationID = newApp.ApplicationID;
                newLocalDrivingLicenseApp.LicenseClassID = clsLicense.Find(_licenseID).LicenseClass;

                return newLocalDrivingLicenseApp.Save()? newLocalDrivingLicenseApp : null;
            }
            else
            {
                MessageBox.Show("An error occurred while creating the application. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }
        private clsLicense GetNewLicense(int applicationID)
        {
            clsLicense newLicense = new clsLicense();
            clsLicenseClass LClass = clsLicenseClass.Find(clsLicense.Find(_licenseID).LicenseClass);

            newLicense.ApplicationID = applicationID;
            newLicense.DriverID = clsDriver.FindByPersonID(_personID).DriverID;
            newLicense.LicenseClass = LClass.LicenseClassID;
            newLicense.IssueDate = DateTime.Now;
            newLicense.ExpirationDate = newLicense.IssueDate.AddYears(LClass.DefaultValidityLength);
            newLicense.Notes = ctrRenewApplicationInfo1.Notes;
            newLicense.PaidFees = (decimal)LClass.ClassFees + (decimal)clsApplicationType.FindApplicationType(2).AppFees;
            newLicense.IsActive = true;
            newLicense.IssueReason = 2;
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

    }
}
