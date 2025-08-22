using BusinessLayer;
using DVLD_Proj.Properties;
using System;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmTakeTest : Form
    {
        enum enMode { Done = 0, NotDone = 1 }
        private enMode _mode;

        private int _testTypeID;
        private int _DLAppID;
        private int _TestAppointmentID;

        public frmTakeTest(int DLAppID, int TestType, int TestAppointmentID)
        {
            InitializeComponent();

            _DLAppID = DLAppID;
            _testTypeID = TestType;
            _TestAppointmentID = TestAppointmentID;

            if (clsTestAppointment.Find(_TestAppointmentID).IsLocked == false)
            {
                _mode = enMode.NotDone;
            }
            else
            {
                _mode = enMode.Done;
            }
        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _PrintTitle(_testTypeID);
            _LoadCtrTestInfo();

            if (_mode == enMode.Done)
            {
                clsTest Test = clsTest.FindByTestAppoinmentID(_TestAppointmentID);

                lblMessage.Visible = true;

                lblTestID.Text = Test.TestID.ToString();

                if (Test.TestResult == true)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;

                panel1.Enabled = false;

                tbNotes.Text = Test.Notes.ToString();

                tbNotes.Enabled = false;

                btnSave.Enabled = false;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (rbPass.Checked == false && rbFail.Checked == false)
            {
                MessageBox.Show("You must select a result before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            clsTest newTest = new clsTest();

            newTest.TestAppointmentID = _TestAppointmentID;

            if (rbPass.Checked == true)
                newTest.TestResult = true;
            else
                newTest.TestResult = false;

            newTest.Notes = tbNotes.Text;

            newTest.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            int Trials = Convert.ToByte(clsTestAppointment.GetTrials(_testTypeID, _DLAppID));

            if (newTest.Save())
            {
                clsTestAppointment CurrentAppointment = clsTestAppointment.Find(_TestAppointmentID);
                CurrentAppointment.IsLocked = true;

                if (CurrentAppointment.Save())
                {
                    MessageBox.Show("The test has been completed successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Trials > 0)
                    {
                        //update the retakeTestApplication
                        int OriginalDLAppID = clsLocalDrivingLicenseApp.Find(_DLAppID).ApplicationID;
                        int PersonID = clsApplication.Find(OriginalDLAppID).ApplicantPersonID;
                        clsApplication retakeTestApp = clsApplication.FindByPersonIDInStatusWithType(PersonID, 1, 8);
                        retakeTestApp.ApplicationStatus = 3; //complete

                        if (!retakeTestApp.Save())
                        {
                            MessageBox.Show("The status of the retake test application could not be updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                    this.Close();
                }
                else
                    MessageBox.Show("The test was not completed successfully due to an error in saving the current appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("The test was not completed successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
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
                ctrTest1.AppointmentDate = clsTestAppointment.Find(_TestAppointmentID).AppointmentDate;

                ctrTest1.HideDatePicker();

                lblDate.Text = clsTestAppointment.Find(_TestAppointmentID).AppointmentDate.ToShortDateString();

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
