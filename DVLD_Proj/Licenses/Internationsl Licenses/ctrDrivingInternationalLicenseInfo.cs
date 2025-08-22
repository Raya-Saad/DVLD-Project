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

namespace DVLD_Proj.UserControls
{
    public partial class ctrDrivingInternationalLicenseInfo : UserControl
    {
        private clsPerson _person;
        private clsInternationalLicense _license;
        public int IntLicenseID
        {
            get; set;
        }

        public ctrDrivingInternationalLicenseInfo()
        {
            InitializeComponent();

            IntLicenseID = -1;
        }

        public void LoadInfo(int IntLicenseID)
        {
            _license = clsInternationalLicense.Find(IntLicenseID);

            if (_license != null)
            {
                int personID = clsDriver.Find((int)clsLicense.Find(_license.IssuedUsingLocalLicenseID).DriverID).PersonID;
                _person = clsPerson.FindPerson(personID);

                if (_person != null)
                {
                    lblName.Text = _person.FirstName + " " + _person.SecondName + " " + _person.ThirdName + " " + _person.LastName;
                    lblIntLicenseID.Text = _license.InternationalLicenseID.ToString();
                    lblLicenseID.Text = _license.IssuedUsingLocalLicenseID.ToString();
                    lblNationalNo.Text = _person.NationalNo;

                    if(_person.Gendor == 0)
                    {
                        lblGendor.Text = "Male";
                        pbGendor.Image = Resources.male;
                    }
                    else
                    {
                        lblGendor.Text = "Female";
                        pbGendor.Image = Resources.female;
                    }

                        lblIssueDate.Text = _license.IssueDate.ToShortDateString();
                    lblApplicationID.Text = _license.ApplicationID.ToString();
                    lblIsActive.Text = (_license.IsActive == true) ? "Yes" : "No";
                    lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
                    lblDriverID.Text = _license.DriverID.ToString();
                    lblExpirationDate.Text = _license.ExpirationDate.ToShortDateString();

                    if (!string.IsNullOrWhiteSpace(this._person.ImagePath) && File.Exists(this._person.ImagePath))
                    {
                        pbImage.Image = System.Drawing.Image.FromFile(this._person.ImagePath);
                    }
                    else
                    {
                        _LoadDefultImage();
                    }
                }


            }
        }

        private void _LoadDefultImage()
        {
            if (_person.Gendor == 0)
                pbImage.Image = Resources.maleImageBig;

            else
                pbImage.Image = Resources.femaleImageBig;
        }
    }
}
