using BusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationTypes();
        }

        private void _RefreshApplicationTypes()
        {
            DataTable ApplicationTypesTable = clsApplicationType.GetAllApplicationTypes();

            ApplicationTypesTable.Columns["ApplicationTypeID"].ColumnName = "ID";
            ApplicationTypesTable.Columns["ApplicationTypeTitle"].ColumnName = "Title";
            ApplicationTypesTable.Columns["ApplicationFees"].ColumnName = "Fees";
            
            dgvApplicationTypesList.DataSource = ApplicationTypesTable.DefaultView;

            lblRecordsNumber.Text = dgvApplicationTypesList.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateApplicationType frm = new frmUpdateApplicationType
                (Convert.ToInt32(dgvApplicationTypesList.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshApplicationTypes();

        }     
    }
}
