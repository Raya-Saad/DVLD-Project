using BusinessLayer;
using DVLD_Proj.Properties;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using TextBox = System.Windows.Forms.TextBox;


namespace DVLD_Proj
{
    public partial class frmAddUpdatePerson : Form
    {

        public delegate void dataBackEventHandeler(object sender, int PersonID);

        public event dataBackEventHandeler del;

        enum enMode { AddNew = 0, Update = 1 }

        enMode _Mode;

        clsPerson _Person;

        int _PersonID;

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();

            this._PersonID = personID;

            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

        }

        private void frmAddNewUpdatePerson_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Person.NationalNo = tbNationalNo.Text;
            _Person.FirstName = tbFirstName.Text;
            _Person.SecondName = tbSecondName.Text;
            _Person.ThirdName = tbThirdName.Text;
            _Person.LastName = tbLastName.Text;
            _Person.Email = tbEmail.Text;
            _Person.Phone = tbPhone.Text;
            _Person.Address = tbAddress.Text;

            if (rbMale.Checked)
                _Person.Gendor = 0;
            else
                _Person.Gendor = 1;

            _Person.NationalityCountryID = clsCountry.FindCountry(cbCountry.Text).CountryID;

            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (pbImage.Tag.Equals("Image"))
            {
                string sourcePath = pbImage.ImageLocation;

                // نحصل امتداد الصورة مثل .jpg أو .png
                string extension = Path.GetExtension(sourcePath);

                string newFileName = "";

                // نولد اسم جديد عشوائي بصيغة GUID
                if(string.IsNullOrEmpty(_Person.ImagePath))
                    newFileName = Guid.NewGuid().ToString() + extension;
                else
                    newFileName = Path.GetFileName(_Person.ImagePath);

                string destinationFolder = @"R:\Projects\DVLD_People_Images";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                string destinationPath = destinationFolder + "\\" + newFileName;


                pbImage.Image.Dispose();
                pbImage.Image = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();


                File.Copy(sourcePath, destinationPath, true);


                _Person.ImagePath = destinationPath;

                pbImage.Image = Image.FromFile(destinationPath);

                pbImage.Tag = "Image";
            }

            else
            {
                if(!string.IsNullOrEmpty(_Person.ImagePath))
                {
                    string oldImagePath = _Person.ImagePath;

                    _Person.ImagePath = "";

                    _DeleteOldImage(oldImagePath);

                    _LoadDefultImage();
                }
            }

            if (_Person.Save())
                MessageBox.Show("Saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("The data could not be saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            _Mode = enMode.Update;

            lblTitle.Text = $"Update Person";
            lblPersonID.Text = _Person.PersonID.ToString();

        }

        private void _DeleteOldImage(string oldImagePath)
        {
            if(File.Exists(oldImagePath))
            {
                try
                {
                    pbImage.Image.Dispose();
                    pbImage.Image = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    File.Delete(oldImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the old image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("The image does not exist in the directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void linklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;

                pbImage.Load(selectedFilePath);

                pbImage.Tag = "Image";

                linkLblDelete.Visible = true;
            }
        }

        private void linkLblDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _LoadDefultImage();

            linkLblDelete.Visible = false;

            pbImage.Tag = rbMale.Checked == true ? "Male" : "Female";
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbImage.Tag.Equals("Female"))
            {
                pbImage.Image = Resources.maleImageBig;
                pbImage.Tag = "Male";
            }

        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbImage.Tag.Equals("Male"))
            {
                pbImage.Image = Resources.femaleImageBig;
                pbImage.Tag = "Female";
            }
        }

        private void tbNationalNo_Leave(object sender, EventArgs e)
        {
            if (clsPerson.FindPerson(tbNationalNo.Text) != null)
            {

                errorProvider1.SetError(tbNationalNo, "National No is already used for another person!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void tbEmail_Leave(object sender, EventArgs e)
        {
            string email = tbEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                errorProvider1.Clear();
                return;
            }

            // تحقق من الصيغة
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (Regex.IsMatch(email, pattern))
            {
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(tbEmail, "Inavlid email!");
            }
        }

        private void _LoadData()
        {
            _IntionalizeDateTimePickerFormat();

            _FillCountriesInComboBox();
            cbCountry.SelectedIndex = cbCountry.FindString("Saudi Arabia");

            if (_Mode == enMode.AddNew)
            {
                _LoadDefultImage();

                lblTitle.Text = "Add New Person";
                lblPersonID.Text = "N/A";
                linkLblDelete.Visible = false;

                _Person = new clsPerson();
                return;
            }

            lblTitle.Text = "Update Person";

            _Person = clsPerson.FindPerson(this._PersonID);

            if (_Person != null)
            {
                _Person.PersonID = _PersonID;
                lblPersonID.Text = _PersonID.ToString();
                tbFirstName.Text = _Person.FirstName.ToString();
                tbSecondName.Text = _Person.SecondName.ToString();
                tbThirdName.Text = _Person.ThirdName.ToString();
                tbLastName.Text = _Person.LastName.ToString();
                tbNationalNo.Text = _Person.NationalNo.ToString();
                dtpDateOfBirth.Value = _Person.DateOfBirth;
                tbPhone.Text = _Person.Phone.ToString();
                tbEmail.Text = _Person.Email.ToString();
                tbAddress.Text = _Person.Address.ToString();

                if (_Person.Gendor == 0)
                    rbMale.Checked = true;
                else
                    rbFemale.Checked = true;

                cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.FindCountry(_Person.NationalityCountryID).CountryName);

                _LoadPersonImage();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (lblPersonID.Text != "N/A")
            {
                int PersonID = int.Parse(lblPersonID.Text);

                del?.Invoke(this, PersonID);
            }

            this.Close();
        }

        private void _IntionalizeDateTimePickerFormat()
        {
            dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
            dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";

            dtpDateOfBirth.MinDate = DateTime.Today.AddYears(-100);
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            dtpDateOfBirth.Value = DateTime.Today.AddYears(-18);
        }

        private void _LoadPersonImage()
        {
            if (_Person != null)
            {
                linkLblDelete.Visible = false;

                if (string.IsNullOrWhiteSpace(_Person.ImagePath))
                {
                    _LoadDefultImage();
                }
                else if (!File.Exists(_Person.ImagePath))
                {
                    MessageBox.Show("The image does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    _LoadDefultImage();
                }
                else
                {
                    pbImage.Image = new Bitmap(_Person.ImagePath);
                    linkLblDelete.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("The person does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _LoadDefultImage();
            }
        }

        private void _LoadDefultImage()
        {
            if (rbMale.Checked == true)
                pbImage.Image = Resources.maleImageBig;

            else if (rbFemale.Checked == true)
                pbImage.Image = Resources.femaleImageBig;
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dt = clsCountry.GetAllCountries();

            foreach (DataRow dr in dt.Rows)
            {
                cbCountry.Items.Add(dr["CountryName"]);
            }
        }

        private void tb_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null) return;

            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tb, "This field cannot be empty or whitespace.");
            }
            else
            {
                errorProvider1.SetError(tb, null);
            }
        }
    }
}
