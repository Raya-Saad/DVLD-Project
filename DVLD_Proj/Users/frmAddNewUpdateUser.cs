using BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmAddNewUpdateUser : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _mode = enMode.AddNew;

        int _UserID = -1;
        clsUser _user;

        clsPerson _person;

        public frmAddNewUpdateUser(int UserID)
        {
            InitializeComponent();

            this._UserID = UserID;

            if (_UserID == -1)
                _mode = enMode.AddNew;
            else
                _mode = enMode.Update;
        }
        private void frmAddNewUpdateUser_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }
        private void _LoadForm()
        {
            switch (_mode)
            {
                case enMode.AddNew:
                    lblTitle.Text = "Add New User";
                    ctrPersonInfoWithFilter1.cbFindby.SelectedIndex = 0;
                    
                    _user = new clsUser();
                    return;

                case enMode.Update:
                    lblTitle.Text = "Update User";
                    ctrPersonInfoWithFilter1.gbFilter.Enabled = false;

                    _user = clsUser.FindUser(_UserID);

                    if (_user != null)
                    {
                        ctrPersonInfoWithFilter1.PersonInfo.LoadInfo(_user.PersonID);

                        lblUserID.Text = _user.PersonID.ToString();
                        tbUserName.Text = _user.UserName;
                        tbPassword.Text = _user.Password;
                        tbConfirmPassword.Text = _user.Password;
                        cbIsActive.Checked = _user.IsActive;
                    }
                    break;
            }


        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid. Hover the mouse over the red icon(s) to see the error details.",
                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _person = clsPerson.FindPerson(ctrPersonInfoWithFilter1.PersonInfo.PersonID);

            if (_person == null)
            {
                MessageBox.Show("You must find or add a person to link with the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                if (tbPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("The passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _user.PersonID = _person.PersonID;
            _user.UserName = tbUserName.Text;
            _user.Password = tbPassword.Text;
            _user.IsActive = cbIsActive.Checked;

            if (_user.Save())
            {
                MessageBox.Show("The user has been saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _mode = enMode.Update;
                lblTitle.Text = "Update User";
                ctrPersonInfoWithFilter1.gbFilter.Enabled = false;
                tbUserName.Enabled = false;
                tbPassword.Enabled = false;
                tbConfirmPassword.Enabled = false;
                cbIsActive.Enabled = false;

                _user = clsUser.FindUser(tbUserName.Text);
                lblUserID.Text = _user.UserID.ToString();
            }

            else
                MessageBox.Show("Failed to save the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tbPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbPassword.Text))
            {
                errorProvider1.SetError(tbPassword, "Please fill in this field.");

            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void tbConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbConfirmPassword.Text))
            {
                errorProvider1.SetError(tbPassword, "Please fill in this field.");

            }
            else if (!string.Equals(tbConfirmPassword.Text, tbPassword.Text, StringComparison.Ordinal))
            {
                errorProvider1.SetError(tbConfirmPassword, "The passwords do not match.");

            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbPassword.Text))
            {
                errorProvider1.SetError(tbPassword, "Please fill in this field.");

            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            _person = clsPerson.FindPerson(ctrPersonInfoWithFilter1.PersonInfo.PersonID);

            if(_mode == enMode.Update)
            {
                tcInfo.SelectedTab = tpLoginInfo;
                return;
            }

            if (_person == null)
            {
                MessageBox.Show("You must find or add a person to link with the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            else if (clsUser.IsPersonLinkedToUser(_person.PersonID))
            {
                MessageBox.Show("This person is already linked to a user. Please find or add another person.", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            else
                tcInfo.SelectedTab = tpLoginInfo;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            tcInfo.SelectedTab = tpPersonalInfo;

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
