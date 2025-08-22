using BusinessLayer;
using DVLD_Proj.Global_Classes;
using System;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmUpdateApplicationType : Form
    {
        private int _ApplicationTypeID;
        private clsApplicationType _ApplicationType;
        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fields are not valid. Hover the mouse over the red icon(s) to see the error details.",
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _ApplicationType.AppTypeTitle = tbTitle.Text;
            _ApplicationType.AppFees = Convert.ToDouble(tbFees.Text);

            if (_ApplicationType.Save())
            {
                MessageBox.Show("Saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Save failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _LoadData();
        }

        private void _LoadData()
        {
            _ApplicationType = BusinessLayer.clsApplicationType.FindApplicationType(_ApplicationTypeID);

            lblID.Text = _ApplicationTypeID.ToString();
            tbTitle.Text = _ApplicationType.AppTypeTitle;
            tbFees.Text = Convert.ToString(_ApplicationType.AppFees);
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void tbFees_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFees, "Fees cannot be blank");
                return;
            }
            if (!clsValidations.IsNumber(tbFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFees, "Fees can only be a number");
                return;
            }
            else
            {
                errorProvider1.SetError(tbFees, null);
            }
            ;
        }

        private void tbTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbTitle, "Fees cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(tbTitle, null);
            }
            ;
        }
    }
}
