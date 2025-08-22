using System;
using BusinessLayer;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmFirstTimeDrivingLicenseIssue : Form
    {
        private int _DLAppID;
        public frmFirstTimeDrivingLicenseIssue(int DLAppID)
        {
            InitializeComponent();

            _DLAppID = DLAppID;

            ctrDLApplicationAndApplicationInfo1.DLApplicationID = _DLAppID;
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsDriver Driver = new clsDriver();

            if (clsDriver.FindByPersonID(ctrDLApplicationAndApplicationInfo1.PersonID) == null)
            {
                Driver.PersonID = ctrDLApplicationAndApplicationInfo1.PersonID;

                //if (newDriver.PersonID == -1)
                //{
                //    MessageBox.Show("Error!, Person Info not Loaded Seccessfully!", "Error");
                //    return;
                //}

                Driver.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;
                Driver.CreatedDate = DateTime.Now;

                if (!Driver.Save())
                {
                    MessageBox.Show("An error occurred while saving the new driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            else
            {
                Driver = clsDriver.FindByPersonID(ctrDLApplicationAndApplicationInfo1.PersonID);
            }

            clsLicense License = new clsLicense();
            License.ApplicationID = ctrDLApplicationAndApplicationInfo1.ApplicationID;

            if (License.ApplicationID == -1)
            {
                MessageBox.Show("The application information could not be loaded successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            clsLicenseClass LClass = clsLicenseClass.Find((clsLocalDrivingLicenseApp.Find(_DLAppID).LicenseClassID));

            License.DriverID = Driver.DriverID;

            License.LicenseClass = LClass.LicenseClassID;

            License.IssueDate = DateTime.Now;

            byte LicenseValidityLength = LClass.DefaultValidityLength;
            License.ExpirationDate = License.IssueDate.AddYears(LicenseValidityLength);

            License.Notes = tbNotes.Text;

            License.PaidFees = LClass.ClassFees;

            License.IsActive = true;

            clsApplication Application = clsApplication.Find(License.ApplicationID);
            License.IssueReason = (byte)Application.ApplicationTypeID;

            License.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (License.Save())
            {
                MessageBox.Show($"The license has been issued successfully with License ID = {License.LicenseID}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                Application.ApplicationStatus = 3;

                if (!Application.Save())
                {
                    MessageBox.Show("The application status could not be updated successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                this.Close();
            }
            else
            {
                MessageBox.Show("The license could not be issued successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
