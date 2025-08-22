using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Proj.UserControls
{
    public partial class ctrDriverLicenseInfoWithFilter : UserControl
    {
        public delegate void dataBackEventHandeler(object sender, int value);
        public event dataBackEventHandeler del;

        public int DLApplicationID
        {
            get; set;
        }

        public int LicenseID
        {
            //get { return Convert.ToInt32(tbLicenseID.Text); }
            //set {  }
            get;set;
        }

        public ctrDriverLicenseInfoWithFilter()
        {
            InitializeComponent();

            ctrDrivingLicenseInfo1.DLApplicationID = DLApplicationID;
        }

        private void ctrDriverLicenseInfoWithFilter_Load(object sender, EventArgs e)
        {
            ctrDrivingLicenseInfo1.DLApplicationID = DLApplicationID;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbLicenseID.Text))
            {
                LicenseID = Convert.ToInt32(tbLicenseID.Text);

                clsLicense license = clsLicense.Find(LicenseID);

                if (license != null)
                {
                    ctrDrivingLicenseInfo1.LoadInfoWithLicense(license);

                    int value = license.LicenseID;

                    //delegate
                    del?.Invoke(this, value);
                }
                else
                {
                    MessageBox.Show("The license was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    //delegate
                    del?.Invoke(this, -1);
                }
            }
        }

        private void tbLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)  
            {
                e.Handled = true; // Block the input
            }
        }

        public void DesableFilter()
        {
            tbLicenseID.Text = LicenseID.ToString();
            gbFilter.Enabled = false;
        }
        public void LoadLicneceInfo()
        {
            ctrDrivingLicenseInfo1.LoadInfoWithLicense(clsLicense.Find(LicenseID));

            int value = LicenseID;

            //delegate
            del?.Invoke(this, value);
        }
    }
}
