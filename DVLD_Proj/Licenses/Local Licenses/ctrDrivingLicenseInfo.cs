using BusinessLayer;
using DVLD_Proj.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Proj.UserControls
{
    public partial class ctrDrivingLicenseInfo : UserControl
    {
        private clsLicense _License;
        private clsDriver _Driver;
        private clsPerson _person;

        public int DLApplicationID
        {
            get; set;
        }

        public ctrDrivingLicenseInfo()
        {
            InitializeComponent();

            _License = new clsLicense();
            _Driver = new clsDriver();
            _person = new clsPerson();
        }

        private void _LoadDefultImage()
        {
            if (_person.Gendor == 0)
                pb.Image = Resources.maleImageBig;

            else
                pb.Image = Resources.femaleImageBig;
        }

        public void LoadInfoWithLicense(clsLicense license)
        {
            _License = license;

            if (_License != null)
            {
                _Driver = clsDriver.Find(_License.DriverID);
            }
            else
            {
                return;
            }

            lblClass.Text = clsLicenseClass.Find(_License.LicenseClass).ClassName;

            _person = clsPerson.FindPerson(_Driver.PersonID);

            string fullName = _person.FirstName + " " + _person.SecondName + " " + _person.ThirdName + " " + _person.LastName;
            lblName.Text = fullName;

            lblLicenseID.Text = _License.LicenseID.ToString();

            lblNationalNo.Text = _person.NationalNo;

            if (_person.Gendor == 0)
            {
                lblGendor.Text = "Male";
                pbGendor.Image = Resources.male;
            }
            else
            {
                lblGendor.Text = "Female";
                pbGendor.Image = Resources.female;
            }

            lblIssueDate.Text = _License.IssueDate.ToShortDateString();

            switch (_License.IssueReason)
            {
                case 1:
                    lblReason.Text = "First Time";
                    break;
                case 2:
                    lblReason.Text = "Renew";
                    break;
                case 3:
                    lblReason.Text = "Replacement For Lost";
                    break;
                case 4:
                    lblReason.Text = "Replacement For Damage";
                    break;
            }

            lblNotes.Text = string.IsNullOrEmpty(_License.Notes) ? "No Notes" : _License.Notes;

            lblIsActive.Text = (_License.IsActive == true) ? "Yes" : "No";

            lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();

            lblDriverID.Text = _Driver.DriverID.ToString();

            lblExpirationDate.Text = _License.ExpirationDate.ToShortDateString();

            lblIsDetained.Text = clsDetainedLicense.IsLicenseDetained(_License.LicenseID) ? "Yes" : "No";

            pb.ImageLocation = _person.ImagePath;

            if (!string.IsNullOrWhiteSpace(this._person.ImagePath) && File.Exists(this._person.ImagePath))
            {
                pb.Image = System.Drawing.Image.FromFile(this._person.ImagePath);
            }
            else
            {
                _LoadDefultImage();
            }
        }
        public void LoadInfo()
        {
            if (DLApplicationID == -1)
            {
                MessageBox.Show("The information could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            clsLocalDrivingLicenseApp LDLApp = clsLocalDrivingLicenseApp.Find(DLApplicationID);

            if (LDLApp != null)
            {
                int ApplicationID = LDLApp.ApplicationID;
                _License = clsLicense.FindByApplicationID(ApplicationID);

                LoadInfoWithLicense(_License);
            }
            else
            {
                return;
            }
        }

       
    }
}
