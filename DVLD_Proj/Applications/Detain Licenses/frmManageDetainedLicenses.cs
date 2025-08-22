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
    public partial class frmManageDetainedLicenses : Form
    {
        enum enFilterOptions
        {
            None = 0, DetainID, IsReleased, NationalNumber, FullName, ReleaseApplicationID
        };

        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            cbSelectFilterType.SelectedIndex = 0;
        }

        private void _RefreshDetainedLicensesList(string columnName = "D.ID", string filterPattern = "")
        {
            DataTable Table = clsDetainedLicense.GetAllDetainedLicenses();
            clsPerson person;
            int personID;
            string nationalNo;

            Table.Columns["DetainID"].ColumnName = "D.ID";
            Table.Columns["LicenseID"].ColumnName = "L.ID";
            Table.Columns["DetainDate"].ColumnName = "D.Date";
            Table.Columns["IsReleased"].ColumnName = "Is Released";
            Table.Columns["FineFees"].ColumnName = "Fine Fees";
            Table.Columns["ReleaseDate"].ColumnName = "Release Date";
            Table.Columns["ReleaseApplicationID"].ColumnName = "Release App.ID";

            Table.Columns.Remove("CreatedByUserID");
            Table.Columns.Remove("ReleasedByUserID");

            Table.Columns.Add("N.No.", typeof(string));
            Table.Columns.Add("Full Name", typeof(string));

            Table.Columns["Is Released"].SetOrdinal(3);
            Table.Columns["Release App.ID"].SetOrdinal(8);

            foreach (DataRow r in Table.Rows)
            {
                personID = clsDriver.Find(clsLicense.Find((int)r["L.ID"]).DriverID).PersonID;
                person = clsPerson.FindPerson(personID);

                r["N.No."] = person.NationalNo;
                r["Full Name"] = person.FirstName + " " + person.SecondName + " " + person.ThirdName + " " + person.LastName;
            }


            if (filterPattern == "")
                Table.DefaultView.RowFilter = "";
            else
            {
                if (columnName == "D.ID" || columnName == "Is Released")
                    Table.DefaultView.RowFilter = $"[{columnName}] = {Convert.ToInt32(filterPattern)}";

                else if (columnName == "Release App.ID")
                    Table.DefaultView.RowFilter = $"[{columnName}] = {Convert.ToInt32(filterPattern)}"; 

                else
                    Table.DefaultView.RowFilter = $"[{columnName}] LIKE '{filterPattern}'";
            }


            dgvDetainedLicenses.DataSource = Table.DefaultView;

            lblRecordsNumber.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }


        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                && (cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("Detained ID")
                || cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("Release Application ID")))


            {
                e.Handled = true; // Block the input
            }
        }
        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            switch (Option)
            {
                case enFilterOptions.None:
                    _RefreshDetainedLicensesList();
                    break;
                case enFilterOptions.DetainID:
                    _RefreshDetainedLicensesList("D.ID", tbFilter.Text);
                    break;
                case enFilterOptions.ReleaseApplicationID:
                    _RefreshDetainedLicensesList("Release App.ID", tbFilter.Text);
                    break;
                case enFilterOptions.NationalNumber:
                    _RefreshDetainedLicensesList("N.No.", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.FullName:
                    _RefreshDetainedLicensesList("Full Name", "%" + tbFilter.Text + "%");
                    break;
            }
        }

        private void cbIsReleasedFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsReleasedFilter.SelectedIndex == cbIsReleasedFilter.FindString("All"))
            {
                _RefreshDetainedLicensesList("Is Released", "");
                return;
            }

            if (cbIsReleasedFilter.SelectedIndex == cbIsReleasedFilter.FindString("Yes"))
                _RefreshDetainedLicensesList("Is Released", "1");
            else
                _RefreshDetainedLicensesList("Is Released", "0");
        }

        private void cbSelectFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Clear();
            cbIsReleasedFilter.SelectedIndex = 0;
            _RefreshDetainedLicensesList();


            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            tbFilter.Visible = false;
            cbIsReleasedFilter.Visible = false;

            switch (Option)
            {
                case enFilterOptions.None:
                    break;

                case enFilterOptions.IsReleased:
                    cbIsReleasedFilter.Visible = true;
                    break;

                default:
                    tbFilter.Visible = true;
                    break;
            }
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }
        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = clsDriver.Find(clsLicense.Find((int)dgvDetainedLicenses.CurrentRow.Cells["L.ID"].Value).DriverID).PersonID;
            frmShowDetails frm = new frmShowDetails(personID);
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }
        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(clsLicense.Find((int)dgvDetainedLicenses.CurrentRow.Cells["L.ID"].Value));
            frm.ShowDialog();
           
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory((string)dgvDetainedLicenses.CurrentRow.Cells["N.No."].Value);
            frm.ShowDialog();

        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense((int)dgvDetainedLicenses.CurrentRow.Cells["L.ID"].Value);
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
