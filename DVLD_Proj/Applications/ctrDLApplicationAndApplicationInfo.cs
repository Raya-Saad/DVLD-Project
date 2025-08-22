using System;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD_Proj.UserControls
{
    public partial class ctrDLApplicationAndApplicationInfo : UserControl
    {
        private int _DLAppID = -1;

        private clsLocalDrivingLicenseAppView _LDLAView;
        private clsApplication _Application;
        public int DLApplicationID
        {
            get { return _DLAppID; }
            set { _DLAppID = value; }
        }
        public int PersonID
        {
            get 
            {
                if (_Application != null)
                {
                    return _Application.ApplicantPersonID;
                }
                return -1;
            }
        }

        public int ApplicationID
        {
            get
            {
                if (_Application != null)
                {
                    return _Application.ApplicationID;
                }
                return -1;
            }
        }
        public ctrDLApplicationAndApplicationInfo()
        {
            InitializeComponent();
        }
        private void ctrDLApplicationAndApplicationInfo_Load(object sender, EventArgs e)
        {
            _LDLAView = clsLocalDrivingLicenseAppView.Find(_DLAppID);

            if (_LDLAView != null)
            {
                //Filling Driving License App Info:
                lblDLAppID.Text = _LDLAView.LocalDrivingLicenseApplicationID.ToString();
                lblAppliedFor.Text = _LDLAView.ClassName;
                lblPassedTests.Text = _LDLAView.PassedTestCount.ToString() + "/3";

                //Filling Basic App Info:
                clsLocalDrivingLicenseApp _LDLApp = clsLocalDrivingLicenseApp.Find(_LDLAView.LocalDrivingLicenseApplicationID);

                if (_LDLApp != null)
                {
                    _Application = clsApplication.Find(_LDLApp.ApplicationID);

                    if (_Application != null)
                    {
                        lblID.Text = _Application.ApplicationID.ToString();
                        lblStatus.Text = _LDLAView.Status;
                        lblFees.Text = Convert.ToSingle(_Application.PaidFees).ToString();

                        lblType.Text = clsApplicationType.FindApplicationType(_Application.ApplicationTypeID).AppTypeTitle;

                        lblApplicant.Text = _LDLAView.FullName;
                        lblDate.Text = _Application.ApplicationDate.ToShortDateString();
                        lblStatusDate.Text = _Application.LastStatusDate.ToShortDateString();

                        var user = clsUser.FindUser(_Application.CreatedByUserID);
                        lblCreatedBy.Text = user != null ? user.UserName : "Unknown";
                    }
                }
                else { MessageBox.Show("Can't Load Data! Driving License Application Is Not Found!"); }
            }
        }
        private void linkLblPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Application != null)
            {
                frmShowDetails frm = new frmShowDetails(_Application.ApplicantPersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Person information is not loaded yet.");
            }
        }
    }
}
