using BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmChangePassword : Form
    {
        private int _userID;
        private clsUser _user;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _userID = UserID;

            if (UserID != -1)
            {
                _user = clsUser.FindUser(_userID);
            }
            else
                _user = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tbCurrentPassword_Leave(object sender, EventArgs e)
        {
            if(tbCurrentPassword.Text != _user.Password)
            {
                errorProvider1.SetError(tbCurrentPassword, "The current password is incorrect.");

                return;
            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ctrUserInfo1.LoadUserInfo(_userID);
        }
        private void tbConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbConfirmPassword.Text))
            {
                errorProvider1.SetError(tbConfirmPassword, "Please fill in this field.");

            }
            else if (!string.Equals(tbConfirmPassword.Text, tbNewPassword.Text, StringComparison.Ordinal))
            {
                errorProvider1.SetError(tbConfirmPassword, "The passwords do not match.");

            }
            else
            {
                errorProvider1.Clear();
            }
        }
        private void tbNewPassword_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbNewPassword.Text))
            {
                errorProvider1.SetError(tbNewPassword, "Please fill in this field.");

            }
            else
            {
                errorProvider1.Clear();
            }
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

            if (tbCurrentPassword.Text != _user.Password)
            {
                MessageBox.Show("You Must Enter The Current Password Correctly!", "Error!", MessageBoxButtons.OK);
                return;
            }

            if(string.Equals(tbConfirmPassword.Text, tbNewPassword.Text, StringComparison.Ordinal))
            {
                _user.Password = tbNewPassword.Text;

                if( _user.Save())
                {
                    MessageBox.Show("The password has been updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ;
                }
                else
                {
                    MessageBox.Show("Failed to update the password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                tbCurrentPassword.Clear();
                tbNewPassword.Clear();
                tbConfirmPassword.Clear();
                errorProvider1.Clear();
            }
            else
            {
                MessageBox.Show("The new password and its confirmation do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
