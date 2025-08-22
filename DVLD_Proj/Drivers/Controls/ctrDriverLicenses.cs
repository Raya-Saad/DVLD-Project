using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD_Proj.UserControls
{
    public partial class ctrDriverLicenses : UserControl
    {
        private DataTable _LocalTable;
        private DataTable _InternationalTable;
        public int driverID { get; set; }
        public ctrDriverLicenses()
        {
            InitializeComponent();
        }

        public void LoadInfo(int DriverID)
        {
            this.driverID = DriverID;

            LoadLocalLicensesHistory();
            LoadInterLicensesHistory();

            dgvLocalLicenses.DataSource = _LocalTable;
            dgvInterLicenses.DataSource = _InternationalTable;

            lblRecordsNumber.Text = dgvLocalLicenses.RowCount.ToString();
        }
        private void LoadLocalLicensesHistory()
        {
            _LocalTable = clsLicense.GetAllLicensesWithDriverID(driverID);

            if (_LocalTable.Rows.Count == 0)
            {
                return;
            }

            _LocalTable.Columns["LicenseID"].ColumnName = "Lic.ID";
            _LocalTable.Columns["ApplicationID"].ColumnName = "App.ID";
            _LocalTable.Columns["IssueDate"].ColumnName = "Issue Date";
            _LocalTable.Columns["ExpirationDate"].ColumnName = "Expiration Date";
            _LocalTable.Columns["IsActive"].ColumnName = "Is Active";
            _LocalTable.Columns.Add("Class Name", typeof(string)).SetOrdinal(2);

            foreach (DataRow row in _LocalTable.Rows)
            {
                row["Class Name"] = clsLicenseClass.Find((int)row["LicenseClass"]).ClassName;
            }
            _LocalTable.Columns.Remove("LicenseClass");

            _LocalTable.Columns.Remove("DriverID");
            _LocalTable.Columns.Remove("Notes");
            _LocalTable.Columns.Remove("PaidFees");
            _LocalTable.Columns.Remove("IssueReason");
            _LocalTable.Columns.Remove("CreatedByUserID");
        }
        private void LoadInterLicensesHistory()
        {
            _InternationalTable = clsInternationalLicense.GetAllWithDriverID(driverID);

            if (_InternationalTable.Rows.Count == 0)
            {
                return;
            }

            _InternationalTable.Columns["InternationalLicenseID"].ColumnName = "Lic.ID";
            _InternationalTable.Columns["ApplicationID"].ColumnName = "App.ID";
            _InternationalTable.Columns["IssueDate"].ColumnName = "Issue Date";
            _InternationalTable.Columns["ExpirationDate"].ColumnName = "Expiration Date";
            _InternationalTable.Columns["IsActive"].ColumnName = "Is Active";
            _InternationalTable.Columns.Add("Class Name", typeof(string)).SetOrdinal(2);

            foreach (DataRow row in _InternationalTable.Rows)
            {
                row["Class Name"] = clsLicenseClass.Find(2).ClassName;
            }

            _InternationalTable.Columns.Remove("DriverID");
            _InternationalTable.Columns.Remove("IssuedUsingLocalLicenseID");
            _InternationalTable.Columns.Remove("CreatedByUserID");
        }
        private void tp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tp.SelectedTab.Text == "Local")
            {
                lblRecordsNumber.Text = dgvLocalLicenses.RowCount.ToString();
            }
            else
            {
                lblRecordsNumber2.Text = dgvInterLicenses.RowCount.ToString();
            }

        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLocalLicenses.CurrentRow.Cells["Lic.ID"].Value;

            frmLicenseInfo frm = new frmLicenseInfo(clsLicense.Find(LicenseID));
            frm.ShowDialog();
        }
    }
}
