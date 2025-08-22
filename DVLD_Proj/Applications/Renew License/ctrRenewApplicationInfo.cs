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
    public partial class ctrRenewApplicationInfo : UserControl
    {
        public int OldLicenseID
        {
            set; get;
        }
        public string Notes
        {
            get; set;
        }

        public ctrRenewApplicationInfo()
        {
            InitializeComponent();
        }


        public void LoadInfo(clsLicense RenewedLicense = null)
        {
            clsLicense oldLicense = clsLicense.Find(OldLicenseID);
            clsLicenseClass Lclass = clsLicenseClass.Find(oldLicense.LicenseClass);

            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblApplicationFees.Text = ((int)clsApplicationType.FindApplicationType(2).AppFees).ToString();
            lblLicenseFees.Text = ((int)Lclass.ClassFees).ToString();
            lblOldLicenseID.Text = OldLicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(Lclass.DefaultValidityLength).ToShortDateString();
            lblCreatedBy.Text = clsGlobalSettings.CurrentUser.UserName;
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToInt32(lblLicenseFees.Text)).ToString();

            if(RenewedLicense == null)
            {
                return;
            }

            lblRLApplicationID.Text = RenewedLicense.ApplicationID.ToString();

            tbNotes.Text = (string.IsNullOrEmpty(RenewedLicense.Notes)) ? "No Notes": RenewedLicense.Notes;
            tbNotes.Enabled = false;

            lblRenewLicenseID.Text = RenewedLicense.LicenseID.ToString();
        }

        private void tbNotes_TextChanged(object sender, EventArgs e)
        {
            Notes = tbNotes.Text;
        }
    }
}
