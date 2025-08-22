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
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _LicenseID;
        private int _personID;
        private string _nationalNo;
        public frmReleaseDetainedLicense(int LicenseID = -1)
        {
            InitializeComponent();

            if(LicenseID != -1)
            {
                _LicenseID = LicenseID;
                ctrDriverLicenseInfoWithFilter1.LicenseID = LicenseID;
                ctrDriverLicenseInfoWithFilter1.del += FilterDataBack;
                ctrDriverLicenseInfoWithFilter1.LoadLicneceInfo();
                ctrDriverLicenseInfoWithFilter1.DesableFilter();
            }

            ctrDriverLicenseInfoWithFilter1.del += FilterDataBack;
        }


        private void btnRelease_Click(object sender, EventArgs e)
        {
            clsDetainedLicense RelLicense = _ReleaseLicesne();

            if (RelLicense != null)
            {
                MessageBox.Show("The license has been released successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRelease.Enabled = false;
                lblReleaseAppID.Text = RelLicense.ReleaseApplicationID.ToString();
                ctrDriverLicenseInfoWithFilter1.DesableFilter();
            }
            else
            {
                MessageBox.Show("An error occurred while releasing the license. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private clsDetainedLicense _ReleaseLicesne()
        {
            clsApplication newApplication = new clsApplication();
            newApplication.ApplicantPersonID = _personID;
            newApplication.ApplicationDate = DateTime.Now;
            newApplication.ApplicationTypeID = 5;
            newApplication.ApplicationStatus = 0;
            newApplication.LastStatusDate = DateTime.Now;
            newApplication.PaidFees = (decimal)clsApplicationType.FindApplicationType(5).AppFees;
            newApplication.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (newApplication.Save())
            {
                clsDetainedLicense ReleasedLicense = clsDetainedLicense.FindUnReleasedLicenseByLicenseID(_LicenseID);
                ReleasedLicense.IsReleased = true;
                ReleasedLicense.ReleaseDate = DateTime.Now;
                ReleasedLicense.ReleasedByUserID = clsGlobalSettings.CurrentUser.UserID;
                ReleasedLicense.ReleaseApplicationID = newApplication.ApplicationID;

                if (ReleasedLicense.Save())
                {
                    newApplication.ApplicationStatus = 3;
                    newApplication.LastStatusDate = DateTime.Now;

                    if (!newApplication.Save())
                        MessageBox.Show("An error occurred while completing the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    return ReleasedLicense;
                }
                else
                    return null;

            }
            else
            {
                MessageBox.Show("An error occurred while creating the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }
        private void FilterDataBack(object sender, int value)
        {
            if (value != -1)
            {
                _LicenseID = value;
                linklblHistory.Enabled = true;
                linkLblInfo.Enabled = true;
                btnRelease.Enabled = true;

                lblLicenseID.Text = _LicenseID.ToString();

                _personID = clsDriver.Find((int)clsLicense.Find(_LicenseID).DriverID).PersonID;
                _nationalNo = clsPerson.FindPerson(_personID).NationalNo;

                clsLicense License = clsLicense.Find(_LicenseID);
                if (!License.IsActive)
                {
                    MessageBox.Show("The selected license is not active. Please choose an active license.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    btnRelease.Enabled = false;
                    return;
                }
                if (!clsDetainedLicense.IsLicenseDetained(_LicenseID))
                {
                    MessageBox.Show("The selected license is not detained. Please choose a license that is detained.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    btnRelease.Enabled = false;
                    return;
                }

                clsDetainedLicense DetainedLic = clsDetainedLicense.FindByLicenseID(_LicenseID);

                lblDetainID.Text = DetainedLic.DetainID.ToString();
                lblDetainDate.Text = DetainedLic.DetainDate.Date.ToShortDateString();
                lblApplicationFees.Text = clsApplicationType.FindApplicationType(5).AppFees.ToString();
                lblCreatedBy.Text = clsGlobalSettings.CurrentUser.UserName;
                lblFineFees.Text =  DetainedLic.FineFees.ToString();
                lblTotalFees.Text = (DetainedLic.FineFees + (decimal)clsApplicationType.FindApplicationType(5).AppFees).ToString();
            }
            else
            {
                linkLblInfo.Enabled = false;
                linklblHistory.Enabled = false;
                btnRelease.Enabled = false;
                _personID = -1;
                _nationalNo = "";
            }

        }
        private void linkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense Lic = clsLicense.Find(_LicenseID);

            frmLicenseInfo frm = new frmLicenseInfo(Lic);
            frm.ShowDialog();
        }
        private void linklblHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_nationalNo);
            frm.ShowDialog();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
