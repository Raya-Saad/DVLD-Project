using BusinessLayer;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DVLD_Proj
{
    public partial class frmLogin : Form
    {
        private string[] _loginInfo;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _loginInfo = new string[2];

            _loginInfo = _FillLoginInfo();

            if(_loginInfo == null)
            {
                tbUserName.Clear();
                tbPassword.Clear();
                cbRememberMe.Checked = false;
            }
            else
            {
                tbUserName.Text = _loginInfo[0];
                tbPassword.Text = _loginInfo[1];
                cbRememberMe.Checked = true;
            }

                
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsGlobalSettings.CurrentUser = clsUser.FindUser(tbUserName.Text);

            if (clsGlobalSettings.CurrentUser == null || clsGlobalSettings.CurrentUser.Password != tbPassword.Text)
            {
                MessageBox.Show("Invalid User Name Or Password!", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!clsGlobalSettings.CurrentUser.IsActive)
            {
                MessageBox.Show("Your Account Is Deactivated, Please Contact Your Admin!", "Deactivated Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (cbRememberMe.Checked)
                {
                    _loginInfo = new string[2];

                    _loginInfo[0] = tbUserName.Text;
                    _loginInfo[1] = tbPassword.Text;
                    _SaveLoginInfo(_loginInfo);
                }
                else
                {
                    _ClearFile();
                }

                frmMain frm = new frmMain(this);

                frm.ShowDialog();
            }
        }

        private void _SaveLoginInfo(string[] LoginInfo, string del = "#|#", string FileName = "UsersLogins.txt")
        {
            string filePath = Path.Combine(Application.StartupPath, FileName);

            string Record = LoginInfo[0] + del + LoginInfo[1];

            try
            {
                File.WriteAllText(filePath, Record);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error working with the file:\n" + ex.Message);
            }
        }

        private string[] _FillLoginInfo(string del = "#|#", string FileName = "UsersLogins.txt")
        {
            string filePath = Path.Combine(Application.StartupPath, FileName);

            try
            {
                if (File.Exists(filePath))
                {
                    string record = File.ReadAllText(filePath);

                    if (string.IsNullOrEmpty(record))
                        return null;

                    string[] loginInfo = record.Split(new string[] { del }, StringSplitOptions.None);

                    return loginInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading the file:\n" + ex.Message);
                return null;
            }

        }

        private void _ClearFile(string FileName = "UsersLogins.txt")
        {
            string filePath = Path.Combine(Application.StartupPath, FileName);

            try
            {
                File.WriteAllText(filePath, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing the file:\n" + ex.Message);
            }
        }

        private void tbUserName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbUserName, "Fees cannot be blank or whitespace");
                return;
            }
            else
            {
                errorProvider1.SetError(tbUserName, null);
            }

        }

        private void tbPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbPassword, "Fees cannot be blank or whitespace");
                return;
            }
            else
            {
                errorProvider1.SetError(tbPassword, null);
            }
        }

        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
