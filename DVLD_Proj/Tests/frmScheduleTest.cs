using BusinessLayer;
using DVLD_Proj.Properties;
using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmScheduleTest : Form
    {
        enum enMode { AddNew = 0, Update = 1 }

        private enMode _mode;

        private int _testTypeID;
        private int _DLAppID;
        private int _TestAppointmentID;
        private float _totalFees = 0;
        public frmScheduleTest(int DLAppID, int TestType, int TestAppointmentID)
        {
            InitializeComponent();

            _DLAppID = DLAppID;
            _testTypeID = TestType;
            _TestAppointmentID = TestAppointmentID;

            if (_TestAppointmentID == -1)
            {
                _mode = enMode.AddNew;
            }
            else
            {
                _mode = enMode.Update;
            }
        }
        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            double retakeFees;

            _PrintTitle(_testTypeID);

            _LoadCtrTestInfo();

            //initial Info:
            gbRetakeTestInfo.Enabled = false;
            lblRAppFees.Text = Convert.ToString(0);
            lblRTestAppID.Text = "N/A";

            if (_mode == enMode.AddNew)
            {
                if (ctrTest1.Trials > 0)
                {
                    gbRetakeTestInfo.Enabled = true;
                    retakeFees = clsApplicationType.FindApplicationType(8).AppFees;
                    lblRAppFees.Text = Convert.ToString(retakeFees);
                }
            }
            else
            {
                gbRetakeTestInfo.Enabled = true;

                if (clsTestAppointment.Find(_TestAppointmentID).IsLocked == true)
                {
                    lblMessage.Visible = true;
                    btnSave.Enabled = false;
                    ctrTest1.DisableDatePicker();
                    lblTotalFees.Text = ctrTest1.PaidFees.ToString();

                    return;
                }

                if (ctrTest1.Trials > 0)
                {
                    retakeFees = clsApplicationType.FindApplicationType(8).AppFees;
                    lblRAppFees.Text = Convert.ToString(retakeFees);

                    int applicationID = clsLocalDrivingLicenseApp.Find(_DLAppID).ApplicationID;
                    int personID = clsApplication.Find(applicationID).ApplicantPersonID;

                    int RetakeAppID = clsApplication.FindByPersonIDInStatusWithType(personID, 1, 8).ApplicationID;
                    lblRTestAppID.Text = RetakeAppID.ToString();
                }
            }

            _totalFees = Convert.ToSingle(ctrTest1.PaidFees) + Convert.ToSingle(lblRAppFees.Text);
            lblTotalFees.Text = _totalFees.ToString();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_mode == enMode.AddNew)
            {
                if (CreateNewAppointment())
                {
                    if (Convert.ToByte(ctrTest1.Trials) >= 1)
                    {
                        CreateRetakeTestApplication();
                    }

                    MessageBox.Show("The new appointment has been saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("The new appointment could not be saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

            else
            {
                clsTestAppointment CurrentAppointment = clsTestAppointment.Find(_TestAppointmentID);

                if (CurrentAppointment != null)
                {
                    CurrentAppointment.AppointmentDate = ctrTest1.AppointmentDate;

                    if (CurrentAppointment.Save())
                    {
                        MessageBox.Show("The appointment has been updated successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("The new appointment could not be updated successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }



            this.Close();
        }
        private bool CreateNewAppointment()
        {
            clsTestAppointment newAppointment = new clsTestAppointment();

            newAppointment.TestTypeID = _testTypeID;
            newAppointment.LocalDrivingLicenseApplicationID = _DLAppID;
            newAppointment.AppointmentDate = ctrTest1.AppointmentDate;
            newAppointment.PaidFees = _totalFees;
            newAppointment.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;
            newAppointment.IsLocked = false;

            return newAppointment.Save();
        }
        private bool CreateRetakeTestApplication()
        {
            clsApplication TestApplication = new clsApplication();

            //which is the (New Local Driving License Service Application):
            int OriginalApplicationID = clsLocalDrivingLicenseApp.Find(_DLAppID).ApplicationID;

            //the new Application
            TestApplication.ApplicantPersonID = clsApplication.Find(OriginalApplicationID).ApplicantPersonID;
            TestApplication.ApplicationDate = DateTime.Now;
            TestApplication.ApplicationTypeID = 8;
            TestApplication.ApplicationStatus = 1;
            TestApplication.LastStatusDate = DateTime.Now;
            TestApplication.PaidFees = Convert.ToDecimal(_totalFees);
            TestApplication.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            return TestApplication.Save();
        }
        private void _LoadCtrTestInfo()
        {
            clsLocalDrivingLicenseAppView LDLAppView = clsLocalDrivingLicenseAppView.Find(_DLAppID);

            if (LDLAppView != null)
            {

                switch (_testTypeID)
                {
                    case 1:
                        ctrTest1.TestImage = Resources.vistionTestBig;
                        break;
                    case 2:
                        ctrTest1.TestImage = Resources.writtenTestBig;
                        break;
                    default:
                        ctrTest1.TestImage = Resources.PracticalTestBig;
                        break;
                }

                ctrTest1.DLApplicationID = _DLAppID;
                ctrTest1.DLClass = LDLAppView.ClassName;
                ctrTest1.PersonName = LDLAppView.FullName;
                ctrTest1.Trials = Convert.ToByte(clsTestAppointment.GetTrials(_testTypeID, _DLAppID));
                ctrTest1.PaidFees = Convert.ToSingle(clsTestType.Find(_testTypeID).TestFees);

                if (_mode == enMode.AddNew)
                {
                    ctrTest1.AppointmentDate = DateTime.Now;
                }
                else
                {
                    ctrTest1.AppointmentDate = clsTestAppointment.Find(_TestAppointmentID).AppointmentDate;
                }
                ctrTest1.LoadData();
            }
        }
        private void _PrintTitle(int TestType)
        {
            switch (TestType)
            {
                case 1:
                    gbTestType.Text = "Vistion Test";
                    break;
                case 2:
                    gbTestType.Text = "Theory Test";
                    break;
                case 3:
                    gbTestType.Text = "Practical Test";
                    break;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
