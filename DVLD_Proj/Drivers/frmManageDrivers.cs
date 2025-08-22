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
    public partial class frmManageDrivers : Form
    {
        enum enFilterOptions
        {
            None = 0, DriverID, PersonID, NotionalNo, FullName
        };
        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            cbSelectFilterType.SelectedIndex = 0;

            _ShowDriversList();
        }

        private void _ShowDriversList(string columnName = "Driver ID", string filterPattern = "")
        {
            DataTable Table = clsDriver_View.GetAll();

            if (filterPattern == "")
                Table.DefaultView.RowFilter = "";
            else
            {
                if (columnName == "DriverID"
                    || columnName == "PersonID")
                    Table.DefaultView.RowFilter = $"{columnName} = {Convert.ToInt32(filterPattern)}";

                else
                    Table.DefaultView.RowFilter = $"[{columnName}] LIKE '{filterPattern}'";
            }

            Table.Columns["DriverID"].ColumnName = "Driver ID";
            Table.Columns["PersonID"].ColumnName = "Person ID";
            Table.Columns["NationalNo"].ColumnName = "National No.";
            Table.Columns["FullName"].ColumnName = "Full Name";
            Table.Columns["CreatedDate"].ColumnName = "Date";
            Table.Columns["NumberOfActiveLicenses"].ColumnName = "Active Licenses";



            dgvDrivers.DataSource = Table;

            lblRecordsNumber.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbSelectFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Clear();
            _ShowDriversList();


            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            tbFilter.Visible = false;

            switch (Option)
            {
                case enFilterOptions.None:
                    break;

                default:
                    tbFilter.Visible = true;
                    break;
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)

                && ((cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("PersonID"))
                || (cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("DriverID"))))
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
                    _ShowDriversList();
                    break;
                case enFilterOptions.PersonID:
                    _ShowDriversList("PersonID", tbFilter.Text);
                    break;
                case enFilterOptions.NotionalNo:
                    _ShowDriversList("NationalNo", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.DriverID:
                    _ShowDriversList("DriverID", tbFilter.Text);
                    break;
                case enFilterOptions.FullName:
                    _ShowDriversList("FullName", "%" + tbFilter.Text + "%");
                    break;

            }
        }
    }
}
