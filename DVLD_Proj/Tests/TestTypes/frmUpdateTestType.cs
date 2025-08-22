using BusinessLayer;
using DVLD_Proj.Global_Classes;
using System;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmUpdateTestType : Form
    {
        private int _TestTypeID;
        private clsTestType _TestType;
        public frmUpdateTestType(int TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
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
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestType.TestTypeTitle = tbTitle.Text;
            _TestType.TestTypeDescription = tbDescription.Text;
            _TestType.TestFees = Convert.ToDouble(tbFees.Text);

            if (_TestType.Save())
            {
                MessageBox.Show("Saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("An error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            _LoadData();
        }

        private void _LoadData()
        {
            _TestType = BusinessLayer.clsTestType.Find(_TestTypeID);

            lblID.Text = _TestTypeID.ToString();
            tbTitle.Text = _TestType.TestTypeTitle;
            tbDescription.Text = _TestType.TestTypeDescription;
            tbFees.Text = Convert.ToString(_TestType.TestFees);
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
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
