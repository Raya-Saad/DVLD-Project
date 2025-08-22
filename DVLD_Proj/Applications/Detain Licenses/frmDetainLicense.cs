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

namespace DVLD_Proj
{
    public partial class frmDetainLicense : Form
    {
        private int _LicenseID;
        private int _personID;
        private string _nationalNo;

        public frmDetainLicense()
        {
            InitializeComponent();
        }
        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            ctrDriverLicenseInfoWithFilter1.del += FilterDataBack;

            lblDetainDate.Text = DateTime.Now.Date.ToShortDateString();
            lblCreatedBy.Text = clsGlobalSettings.CurrentUser.UserName;
        }
        private void FilterDataBack(object sender, int value)
        {
            tbFineFees.Clear();
            tbFineFees.Enabled = true;

            if (value != -1)
            {
                _LicenseID = value;
                linklblHistory.Enabled = true;
                linkLblInfo.Enabled = true;
                btnDetain.Enabled = true;

                lblLicenseID.Text = _LicenseID.ToString();

                _personID = clsDriver.Find((int)clsLicense.Find(_LicenseID).DriverID).PersonID;
                _nationalNo = clsPerson.FindPerson(_personID).NationalNo;

                clsLicense License = clsLicense.Find(_LicenseID);
                if (!License.IsActive)
                {
                    MessageBox.Show("The selected license is not active. Please choose an active license.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnDetain.Enabled = false;
                    return;
                }
                if (clsDetainedLicense.IsLicenseDetained(_LicenseID))
                {
                    MessageBox.Show("The selected license is already detained. Please choose a license that is not detained.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnDetain.Enabled = false;
                    return;
                }
            }
            else
            {
                linkLblInfo.Enabled=false;
                linklblHistory.Enabled=false;
                btnDetain.Enabled=false;
                _personID = -1;
                _nationalNo = "";
            }
            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFineFees.Text))
            {
                MessageBox.Show("Please enter the fine fees before continuing.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            clsDetainedLicense DetainLicense = _DetainLicense();
            if (DetainLicense != null)
            {
                MessageBox.Show("The license has been detained successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnDetain.Enabled = false;
                tbFineFees.Enabled = false;
                lblDetainID.Text = DetainLicense.DetainID.ToString();
            }

        }

        private clsDetainedLicense _DetainLicense()
        {
            clsDetainedLicense newDetain = new clsDetainedLicense();

            newDetain.LicenseID = _LicenseID;
            newDetain.DetainDate = DateTime.Now;
            newDetain.FineFees = Convert.ToDecimal(tbFineFees.Text);
            newDetain.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;
            newDetain.IsReleased = false;

            return newDetain.Save() ? newDetain : null;
        }

        private void tbFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true; // Block the input
            }
        }
        private void linkLblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense Lic = clsLicense.Find(_LicenseID);

            frmLicenseInfo frm = new frmLicenseInfo(Lic);
            frm.ShowDialog();
        }
        private void linklblHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_nationalNo);
            frm.ShowDialog();
        }
    }
}
