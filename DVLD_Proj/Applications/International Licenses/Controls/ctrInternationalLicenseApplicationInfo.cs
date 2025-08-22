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

namespace DVLD_Proj.UserControls
{
    public partial class ctrInternationalLicenseApplicationInfo : UserControl
    {
        public int LocalLicenseID
        {
            set; get;
        }

        public ctrInternationalLicenseApplicationInfo()
        {
            InitializeComponent();

            LocalLicenseID = -1;
        }

        public void LoadInfo(clsInternationalLicense internationalLicense = null)
        {

            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();

            lblFees.Text = clsApplicationType.FindApplicationType(6).AppFees.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblCreatedBy.Text = clsGlobalSettings.CurrentUser.UserName;

            lblLocalLicenseID.Text = (LocalLicenseID != -1) ? LocalLicenseID.ToString() : "???";

            if(internationalLicense != null)
            {
                lblILAppID.Text = internationalLicense.ApplicationID.ToString();
                lblILicenseID.Text = internationalLicense.InternationalLicenseID.ToString();
            }
        }
    }
}
