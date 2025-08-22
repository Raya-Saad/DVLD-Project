using BusinessLayer;
using BusinessLayer;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmNewLocalDrivingLicenseApp : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _mode = enMode.AddNew;

        clsPerson _person;

        clsApplication _Application;

        clsLocalDrivingLicenseApp _LocalLicenseApplication;
        int _LocalLicenseAppID;

        public frmNewLocalDrivingLicenseApp(int LocalLicenseAppID)
        {
            InitializeComponent();

            _LocalLicenseAppID = LocalLicenseAppID;

            _mode = _LocalLicenseAppID == -1 ? enMode.AddNew : enMode.Update;
        }

        private void _LoadForm()
        {
            _FillLiceseClassesInComboBox();

            switch (_mode)
            {
                case enMode.AddNew:
                    lblTitle.Text = "New Local Driving License Application";
                    ctrPersonInfoWithFilter1.cbFindby.SelectedIndex = 0;

                    _Application = new clsApplication();
                    _LocalLicenseApplication = new clsLocalDrivingLicenseApp();

                    lblApplicationDate.Text = _Application.ApplicationDate.ToString();
                    cbLicenseClass.SelectedIndex = 2;
                    lblApplicationFees.Text = _Application.PaidFees.ToString();
                    lblCreatedBy.Text = clsGlobalSettings.CurrentUser.UserName;

                    return;

                case enMode.Update:
                    lblTitle.Text = "Update Local Driving License Application";
                    ctrPersonInfoWithFilter1.gbFilter.Enabled = false;

                    _LocalLicenseApplication = clsLocalDrivingLicenseApp.Find(_LocalLicenseAppID);

                    if (_LocalLicenseApplication != null)
                    {
                        ctrPersonInfoWithFilter1.PersonInfo.LoadInfo(_Application.ApplicantPersonID);

                        lblDLApplicationID.Text = _LocalLicenseApplication.LocalDrivingLicenseApplicationID.ToString();

                        lblApplicationDate.Text = _Application.ApplicationDate.ToString();

                        clsLicenseClass Lclass = clsLicenseClass.Find(_LocalLicenseApplication.LicenseClassID);
                        if (Lclass != null)
                        {
                            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(Lclass.ClassName);
                        }

                        lblApplicationFees.Text = _Application.PaidFees.ToString();

                        lblCreatedBy.Text = _Application.CreatedByUserID.ToString();
                    }
                    break;
            }


        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            _person = clsPerson.FindPerson(ctrPersonInfoWithFilter1.PersonInfo.PersonID);

            if (_person == null)
            {
                MessageBox.Show("Please select a person first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            clsDriver driver = clsDriver.FindByPersonID(_person.PersonID);
            if (driver != null)
            {
               if(IsAlreadyHasLicenseWithSelectedClass(driver.DriverID))
                {
                    MessageBox.Show("This person already has a license for this license class.\nYou can only add an application with a new license class.", "Duplicate License Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (clsLocalDrivingLicenseAppView.IsThereAlreadyNewApp(cbLicenseClass.Text, _person.NationalNo, "New"))
            {

                _LocalLicenseAppID = clsLocalDrivingLicenseAppView.GetApplicationByNationalNoInClassAndStatus(cbLicenseClass.Text, _person.NationalNo, "New");

                int PrevApplicationID = clsLocalDrivingLicenseApp.GetApplicationIDByLDLAppID(_LocalLicenseAppID);

                MessageBox.Show($"This person already has a previous active application for the same license class. Previous Application ID = {PrevApplicationID}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _Application.ApplicantPersonID = _person.PersonID;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = 1;
            _Application.ApplicationStatus = 1;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = 15;
            _Application.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (_Application.Save())
            {
                _LocalLicenseApplication.ApplicationID = _Application.ApplicationID;
                _LocalLicenseApplication.LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

                if (_LocalLicenseApplication.Save())
                {
                    MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _mode = enMode.Update;
                    lblTitle.Text = "Update Local Driving License Application";
                    ctrPersonInfoWithFilter1.gbFilter.Enabled = false;
                }

                _Application = clsApplication.FindByPersonID(_person.PersonID);
                lblDLApplicationID.Text = _Application.ApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("The data could not be saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private bool IsAlreadyHasLicenseWithSelectedClass(int DriverID)
        {
            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            DataTable dt = clsLicense.GetAllLicenses();
            foreach (DataRow dr in dt.Rows)
            {
                if ((int)dr["DriverID"] == DriverID
                    && (int)dr["LicenseClass"] == LicenseClassID)
                {
                    return true;
                }
            }
            return false;

        }
        private void _FillLiceseClassesInComboBox()
        {
            DataTable dt = clsLicenseClass.GetAll();

            foreach (DataRow dr in dt.Rows)
            {
                cbLicenseClass.Items.Add(dr["ClassName"]);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmNewLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {
            _person = clsPerson.FindPerson(ctrPersonInfoWithFilter1.PersonInfo.PersonID);

            if (_mode == enMode.Update)
            {
                tcInfo.SelectedTab = tpLoginInfo;
                return;
            }

            if (_person == null)
            {
                MessageBox.Show("Please select a person first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }


            else
                tcInfo.SelectedTab = tpLoginInfo;
        }
    }
}
