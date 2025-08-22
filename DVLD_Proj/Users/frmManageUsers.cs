using BusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmManageUsers : Form
    {
        enum enFilterOptions
        {
            None = 0, UserID, UserName, PersonID, FullName, IsActive
        };
        public frmManageUsers()
        {
            InitializeComponent();
        }
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            cbSelectFilterType.SelectedIndex = 0;

            _RefreshUsersList();
        }
        private void _RefreshUsersList(string columnName = "UserID", string filterPattern = "")
        {
            DataTable table = clsUser.GetAllUsers();

            table.Columns.Add("Full Name",  typeof(string));

            foreach (DataRow row in table.Rows)
            {
                int PersonID = Convert.ToInt32(row["PersonID"]);

                clsPerson Person = clsPerson.FindPerson(PersonID);

                if (Person != null)
                {
                    row["Full Name"] = Person.FirstName + " " + Person.SecondName 
                        + " " + Person.ThirdName + " " + Person.LastName;
                }
                else
                {
                    row["Full Name"] = "N/A";
                }
            }

            table.Columns.Remove("Password");

            DataTable displayTable = table.DefaultView.ToTable(false, "UserID", "PersonID", "Full Name", "UserName", "IsActive");

            if (filterPattern == "")
                displayTable.DefaultView.RowFilter = "";
            else
            {
                if (columnName == "PersonID" || columnName == "UserID" || columnName == "IsActive")
                    displayTable.DefaultView.RowFilter = $"{columnName} = {Convert.ToInt32(filterPattern)}";

                else
                    displayTable.DefaultView.RowFilter = $"[{columnName}] LIKE '{filterPattern}'";
            }

            dgvUsersList.DataSource = displayTable.DefaultView;

            lblRecordsNum.Text = dgvUsersList.Rows.Count.ToString();
        }
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                && (cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("PersonID")
                || cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("UserID")))
            {
                e.Handled = true; // Block the input
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            switch (Option)
            {
                case enFilterOptions.None:
                    _RefreshUsersList("UserID", "");
                    break;
                case enFilterOptions.PersonID:
                    _RefreshUsersList("PersonID", tbFilter.Text);
                    break;
                case enFilterOptions.UserID:
                    _RefreshUsersList("UserID", tbFilter.Text);
                    break;
                case enFilterOptions.UserName:
                    _RefreshUsersList("UserName", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.FullName:
                    _RefreshUsersList("Full Name", "%" + tbFilter.Text + "%");
                    break;
            }
        }
        private void cbActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbActiveFilter.SelectedIndex == cbActiveFilter.FindString("All"))
            {
                _RefreshUsersList("IsActive", "");
                return;
            }

            if (cbActiveFilter.SelectedIndex == cbActiveFilter.FindString("Yes"))
                _RefreshUsersList("IsActive", "1");
            else
                _RefreshUsersList("IsActive", "0");
        }
        private void cbSelectFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Clear();
            cbActiveFilter.SelectedIndex = 0;
            _RefreshUsersList();


            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            tbFilter.Visible = false;
            cbActiveFilter.Visible = false;

            switch (Option)
            {
                case enFilterOptions.None:
                    break;

                case enFilterOptions.IsActive:
                    cbActiveFilter.Visible = true;
                    break;

                default:
                    tbFilter.Visible = true;
                    break;
            }
        }
        private void _AddNewUser(object sender, EventArgs e)
        {
            frmAddNewUpdateUser frm = new frmAddNewUpdateUser(-1);

            frm.ShowDialog();

            _RefreshUsersList();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewUpdateUser frm = new frmAddNewUpdateUser
                (Convert.ToInt32(dgvUsersList.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshUsersList();
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserDetails frm = new frmShowUserDetails
                (Convert.ToInt32(dgvUsersList.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshUsersList();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (clsUser.DeleteUser((int)dgvUsersList.CurrentRow.Cells[0].Value))
                    MessageBox.Show("The user has been deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The user was not deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            _RefreshUsersList();
        }
        private void ChangePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(Convert.ToInt32(dgvUsersList.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshUsersList();
        }
    }
}
