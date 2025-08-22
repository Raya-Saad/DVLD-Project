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
    public partial class frmInternationalLicenseApplications : Form
    {
        public frmInternationalLicenseApplications()
        {
            InitializeComponent();

            _RefreshInterLicenseAppsList();
        }

        private void _RefreshInterLicenseAppsList()
        {
            DataTable table = clsInternationalLicense.GetAll();

            table.Columns["InternationalLicenseID"].ColumnName = "Int.License ID";
            table.Columns["ApplicationID"].ColumnName = "Application ID";
            table.Columns["DriverID"].ColumnName = "Driver ID";
            table.Columns["IssuedUsingLocalLicenseID"].ColumnName = "L.License ID";
            table.Columns["IssueDate"].ColumnName = "Issue Date";
            table.Columns["ExpirationDate"].ColumnName = "Expiration Date";
            table.Columns["IsActive"].ColumnName = "Is Active";
            
            table.Columns.Remove("CreatedByUserID");

            dgvLicensesList.DataSource = table;

            lblRecordsNumber.Text = dgvLicensesList.Rows.Count.ToString();
        }
        
        


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
            _RefreshInterLicenseAppsList();
        }
        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.Find((int) dgvLicensesList.CurrentRow.Cells["Driver ID"].Value);

            frmShowDetails frm = new frmShowDetails(Driver.PersonID);
            frm.ShowDialog();
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsInternationalLicense license = clsInternationalLicense.Find((int)dgvLicensesList.CurrentRow.Cells["Int.License ID"].Value);

            frmDriverInternationalLicenseInfo frm = new frmDriverInternationalLicenseInfo(license);
            frm.ShowDialog();
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.Find((int)dgvLicensesList.CurrentRow.Cells["Driver ID"].Value);
            clsPerson person = clsPerson.FindPerson(Driver.PersonID);

            frmLicenseHistory frm = new frmLicenseHistory(person.NationalNo);
            frm.ShowDialog();
        }

    }
}
