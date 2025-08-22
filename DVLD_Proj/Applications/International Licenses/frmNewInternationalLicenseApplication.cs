using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Proj
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        enum enMode { NewApplication, IssuedLicense }

        private enMode _mode;
        private int _licenseID;
        private int _personID;
        private string _nationalNo;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();

            _mode = enMode.NewApplication;

            ctrInternationalLicenseApplicationInfo1.LoadInfo();
        }
        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrDriverLicenseInfoWithFilter1.del += FilterDataBack;
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsInternationalLicense License = clsInternationalLicense.FindByLocalLicenseID(_licenseID);

                if (License != null)
                {
                    if (License.IsActive == true)
                    {
                        MessageBox.Show(
                            $"This person already has an active international driving license with ID = {License.InternationalLicenseID}.",
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }
                else
                {
                    if(!clsLicense.DoesPersonHaveLicenseWithClass((clsDriver.FindByPersonID(_personID).DriverID), 3))
                    {
                        MessageBox.Show(
                            "This person does not have an active local driving license with Class 3 - Ordinary Driving License. They must have it first",
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }


                    int appID = GetNewApplicationID();

                    if (appID != -1)
                    {
                        int InternationalLicenseID = GetIssuedInternationlLicenseID(appID);

                        if (InternationalLicenseID != -1)
                        {
                            if (CompleteApplication(appID))
                            {
                                MessageBox.Show($"International license issued seccessfully with ID = {InternationalLicenseID}", "License issued!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                _mode = enMode.IssuedLicense;

                                //refresh
                                ctrInternationalLicenseApplicationInfo1.LoadInfo(clsInternationalLicense.Find(InternationalLicenseID));

                                linkLblInfo.Enabled = true;
                                return;
                            }
                            else
                            {
                                MessageBox.Show("An error occurred while completing the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                return;
                            }

                        }
                        else
                        {
                            MessageBox.Show("An error occurred while creating the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while creating the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        return;
                    }
                }
            }
            else
            {
                return;
            }

        }

        private int GetNewApplicationID()
        {
            clsApplication newApplication = new clsApplication();

            newApplication.ApplicantPersonID = _personID;
            newApplication.ApplicationDate = DateTime.Now;
            newApplication.ApplicationTypeID = 6;
            newApplication.ApplicationStatus = 1;
            newApplication.LastStatusDate = DateTime.Now;
            newApplication.PaidFees = (decimal)clsApplicationType.FindApplicationType(6).AppFees;
            newApplication.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            return (newApplication.Save()) ? newApplication.ApplicationID : -1;
        }

        private bool CompleteApplication(int applicationID)
        {
            clsApplication app = clsApplication.Find(applicationID);

            app.ApplicationStatus = 3;
            app.LastStatusDate = DateTime.Now;

            return app.Save();
        }
        private int GetIssuedInternationlLicenseID(int applicationID)
        {
            clsInternationalLicense License = new clsInternationalLicense();
            clsLicense LocalLicense = clsLicense.Find(_licenseID);

            License.ApplicationID = applicationID;
            License.DriverID = LocalLicense.DriverID;
            License.IssuedUsingLocalLicenseID = _licenseID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(1);
            License.IsActive = true;
            License.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            return (License.Save()) ? License.InternationalLicenseID : -1;
        }

        private void linklblHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_nationalNo);
            frm.ShowDialog();
        }

        private void FilterDataBack(object sender, int value)
        {
            if(value != -1)
            {
                _licenseID = value;
                linklblHistory.Enabled = true;
                btnIssue.Enabled = true;

                ctrInternationalLicenseApplicationInfo1.LocalLicenseID = value;
                ctrInternationalLicenseApplicationInfo1.LoadInfo();

                _personID = clsDriver.Find((int)clsLicense.Find(_licenseID).DriverID).PersonID;
                _nationalNo = clsPerson.FindPerson(_personID).NationalNo;

                clsInternationalLicense interLicense = clsInternationalLicense.FindByLocalLicenseID(_licenseID);
                if (interLicense != null)
                {
                    linkLblInfo.Enabled = true;
                }
                else
                {
                    linkLblInfo.Enabled = false;
                }
            }
            else
            {
                linklblHistory.Enabled = false;
                linkLblInfo.Enabled = false;
                btnIssue.Enabled = false;
                _personID = -1;
                _nationalNo = "";
            }
            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void linkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverInternationalLicenseInfo frm = new frmDriverInternationalLicenseInfo(clsInternationalLicense.FindByLocalLicenseID(_licenseID));
            frm.ShowDialog();
            return;
        }
    }
}
