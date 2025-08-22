using BusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmLocalDrivingLicenseApps : Form
    {
        public frmLocalDrivingLicenseApps()
        {
            InitializeComponent();
        }

        enum enFilterOptions
        {
            None = 0, LDLAppID, NationalNumber, FullName, Status
        };

        private void frmLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            cbSelectFilterType.SelectedIndex = 0;

            _RefreshApplicationsList();
        }
        private void _RefreshApplicationsList(string columnName = "L.D.L.AppID", string filterPattern = "")
        {
            DataTable Table = clsLocalDrivingLicenseAppView.GetAll();

            Table.Columns["LocalDrivingLicenseApplicationID"].ColumnName = "L.D.L.AppID";
            Table.Columns["ClassName"].ColumnName = "Driving Class";
            Table.Columns["NationalNo"].ColumnName = "National No.";
            Table.Columns["FullName"].ColumnName = "Full Name";
            Table.Columns["ApplicationDate"].ColumnName = "Application Date";
            Table.Columns["PassedTestCount"].ColumnName = "Passed Tests";

            if (filterPattern == "")
                Table.DefaultView.RowFilter = "";
            else
            {
                if (columnName == "L.D.L.AppID"
                    || columnName == "Passed Tests")
                    Table.DefaultView.RowFilter = $"{columnName} = {Convert.ToInt32(filterPattern)}";

                //else if (columnName == "ApplicationDate")
                //    Table.DefaultView.RowFilter = $"{columnName} = {Convert.ToDateTime(filterPattern)}";

                else
                    Table.DefaultView.RowFilter = $"[{columnName}] LIKE '{filterPattern}'";
            }


            dgvApplications.DataSource = Table.DefaultView;

            lblRecordsNumber.Text = dgvApplications.Rows.Count.ToString();
        }

        //private void dgvApplications_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    //To change the status nums to words

        //    if (dgvApplications.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
        //    {
        //        if (e.Value.ToString() == "1")
        //            e.Value = "New";

        //        else if (e.Value.ToString() == "2")
        //            e.Value = "Cancelled";

        //        else
        //            e.Value = "Completed";

        //        e.FormattingApplied = true;
        //    }

        //    if (dgvPeople.Columns[e.ColumnIndex].Name == "NationalityCountryID" && e.Value != null)
        //    {
        //        int CountryID = Convert.ToInt32(e.Value);

        //        e.Value = clsCountry.FindCountry(CountryID).CountryName;
        //        e.FormattingApplied = true;
        //    }
        //}
        private void cbSelectFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Clear();
            cbStatusFilter.SelectedIndex = 0;
            _RefreshApplicationsList();


            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            tbFilter.Visible = false;
            cbStatusFilter.Visible = false;


            switch (Option)
            {
                case enFilterOptions.None:
                    break;

                case enFilterOptions.Status:
                    cbStatusFilter.Visible = true;
                    break;

                default:
                    tbFilter.Visible = true;
                    break;
            }
        }
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                && cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("L.D.L.AppID"))
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
                    _RefreshApplicationsList();
                    break;
                case enFilterOptions.LDLAppID:
                    _RefreshApplicationsList("L.D.L.AppID", tbFilter.Text);
                    break;
                case enFilterOptions.NationalNumber:
                    _RefreshApplicationsList("National No.", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.FullName:
                    _RefreshApplicationsList("Full Name", "%" + tbFilter.Text + "%");
                    break;
            }
        }
        private void cbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatusFilter.SelectedIndex == cbStatusFilter.FindString("All"))
            {
                _RefreshApplicationsList("Status", "");
                return;
            }

            if (cbStatusFilter.SelectedIndex == cbStatusFilter.FindString("New"))
                _RefreshApplicationsList("Status", "New");
            else if (cbStatusFilter.SelectedIndex == cbStatusFilter.FindString("Cancelled"))
                _RefreshApplicationsList("Status", "Cancelled");
            else
                _RefreshApplicationsList("Status", "Completed");
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApp frm = new frmNewLocalDrivingLicenseApp(-1);

            frm.ShowDialog();

            _RefreshApplicationsList();
        }
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID = Convert.ToInt32(dgvApplications.CurrentRow.Cells[0].Value);

            int AppID = clsLocalDrivingLicenseApp.GetApplicationIDByLDLAppID(LDLAppID);

            clsApplication app = clsApplication.Find(AppID);

            app.ApplicationStatus = 2; //cancel

            if (app.Save())
            {
                if (MessageBox.Show("Are you sure you want to cancel this application?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    MessageBox.Show("The application has been canceled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshApplicationsList();
                }

            }
            else
            {
                MessageBox.Show("The application could not be canceled successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            deleteApplicationToolStripMenuItem.Enabled = false;
            cancelApplicationToolStripMenuItem.Enabled = false;
            sechduleTestsToolStripMenuItem.Enabled = false;
            issueDrivingLicenseToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;

            switch (dgvApplications.CurrentRow.Cells[6].Value.ToString())
            {
                case "New":
                    deleteApplicationToolStripMenuItem.Enabled = true;
                    cancelApplicationToolStripMenuItem.Enabled = true;

                    sechduleTestsToolStripMenuItem.Enabled = true;
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWrittenTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;

                    switch (dgvApplications.CurrentRow.Cells[5].Value)
                    {
                        case 0:
                            scheduleVisionTestToolStripMenuItem.Enabled = true;
                            break;
                        case 1:
                            scheduleWrittenTestToolStripMenuItem.Enabled = true;
                            break;
                        case 2:
                            scheduleStreetTestToolStripMenuItem.Enabled = true;
                            break;
                        case 3:
                            sechduleTestsToolStripMenuItem.Enabled = false;
                            issueDrivingLicenseToolStripMenuItem.Enabled = true;

                            break;
                    }
                    break;

                case "Completed":
                    showLicenseToolStripMenuItem.Enabled = true;
                    break;
            }
        }
        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointment frm = new frmTestAppointment(Convert.ToInt32(dgvApplications.CurrentRow.Cells[0].Value), 1);

            frm.ShowDialog();

            _RefreshApplicationsList();
        }
        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointment frm = new frmTestAppointment(Convert.ToInt32(dgvApplications.CurrentRow.Cells[0].Value), 2);

            frm.ShowDialog();

            _RefreshApplicationsList();
        }
        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointment frm = new frmTestAppointment(Convert.ToInt32(dgvApplications.CurrentRow.Cells[0].Value), 3);

            frm.ShowDialog();

            _RefreshApplicationsList();
        }



        private void issueDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFirstTimeDrivingLicenseIssue frm = new frmFirstTimeDrivingLicenseIssue((int)dgvApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshApplicationsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo((int)dgvApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DLAppID = (int)dgvApplications.CurrentRow.Cells[0].Value;
            int ApplicationID = clsLocalDrivingLicenseApp.Find(DLAppID).ApplicationID;

            DialogResult result = MessageBox.Show("Are you sure you want to delete this application?",
                                      "Confirm Deletion",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (clsLocalDrivingLicenseApp.Delete(DLAppID) && clsApplication.Delete(ApplicationID))
                {
                    MessageBox.Show("The application was deleted successfully.",
                                    "Deleted",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    _RefreshApplicationsList();
                }
                else
                {
                    MessageBox.Show("The application could not be deleted. Please try again.",
                                    "Deletion Failed",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory((string)dgvApplications.CurrentRow.Cells["National No."].Value);
            frm.ShowDialog();
        }


    }
}
