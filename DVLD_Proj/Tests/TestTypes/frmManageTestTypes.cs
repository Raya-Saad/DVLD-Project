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
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void _RefreshTestTypes()
        {
            DataTable TestTypesTable = clsTestType.GetAllTestTypes();

            TestTypesTable.Columns["TestTypeID"].ColumnName = "ID";
            TestTypesTable.Columns["TestTypeTitle"].ColumnName = "Title";
            TestTypesTable.Columns["TestTypeDescription"].ColumnName = "Description";
            TestTypesTable.Columns["TestTypeFees"].ColumnName = "Fees";

            dgvTestTypesList.DataSource = TestTypesTable.DefaultView;

            lblRecordsNumber.Text = dgvTestTypesList.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshTestTypes();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestType frm = new frmUpdateTestType
                (Convert.ToInt32(dgvTestTypesList.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshTestTypes();
        }
    }
}
