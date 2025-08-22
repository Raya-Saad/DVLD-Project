using BusinessLayer;
using DVLD_Proj.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class ctrPersonInfo : UserControl
    {
        private clsPerson _person;

        public int PersonID
        {
            get;set;
        }

        public ctrPersonInfo()
        {
            InitializeComponent();
        }

        public void LoadInfo(int personID)
        {
            this.PersonID = personID;

            _person = clsPerson.FindPerson(PersonID);


            if (_person != null)
            {
                lblPersonID.Text = PersonID.ToString();

                lblNationalNo.Text = _person.NationalNo.ToString();

                string FullName = _person.FirstName + " " + _person.SecondName + " "
                    + _person.ThirdName + " " + _person.LastName;

                lblName.Text = FullName;

                if (_person.Gendor == 0)
                {
                    lblGender.Text = "Male";
                    pbGendor.Image = Resources.male;
                }
                else
                {
                    lblGender.Text = "Female";
                    pbGendor.Image = Resources.female;
                }

                lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();

                lblEmail.Text = _person.Email.ToString();

                lblPhone.Text = _person.Phone.ToString();

                lblAddress.Text = _person.Address.ToString();

                lblCountry.Text = clsCountry.FindCountry(_person.NationalityCountryID).CountryName;

                if (!string.IsNullOrWhiteSpace(_person.ImagePath) && File.Exists(_person.ImagePath))
                {
                    Pic.Image = Image.FromFile(_person.ImagePath);
                }
                else
                {
                    _LoadDefultImage();
                }

                linkLblEdit.Visible = true;
            }
            else
            {
                MessageBox.Show("The person was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void _LoadDefultImage()
        {
            if (_person.Gendor == 0)
                Pic.Image = Resources.maleImageBig;

            else
                Pic.Image = Resources.femaleImageBig;
        }

        private void linkLblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(Convert.ToInt32(lblPersonID.Text));

            frm.ShowDialog();

            LoadInfo(Convert.ToInt32(lblPersonID.Text));
        }
    }
}
