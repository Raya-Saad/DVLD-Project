using BusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmManagePeople : Form
    {
        enum enFilterOptions { None = 0, PersonID, NationalNumber, FirstName,
            SecondName, ThirdName, LastName, Nationality, Gendor, Phone, Email};
        public frmManagePeople()
        {
            InitializeComponent();
        }
        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            cbSelectFilterType.SelectedIndex = 0;

            _FillCountriesInComboBox();

            _RefreshPeopleList();
        }
        private void _FillCountriesInComboBox()
        {
            DataTable dt = clsCountry.GetAllCountries();

            foreach (DataRow dr in dt.Rows)
            {
                cbNationalityFilter.Items.Add(dr["CountryName"]);
            }
        }
        private void _RefreshPeopleList(string columnName = "PersonID", string filterPattern = "")
        {
            DataTable peopleTable = clsPerson.GetAllPersons();

            if (filterPattern == "")
                peopleTable.DefaultView.RowFilter = "";
            else
            {
                if (columnName == "PersonID" || columnName == "Gendor" || columnName == "NationalityCountryID")
                    peopleTable.DefaultView.RowFilter = $"{columnName} = {Convert.ToInt32(filterPattern)}";

                else
                    peopleTable.DefaultView.RowFilter = $"{columnName} LIKE '{filterPattern}'";
            }

            dgvPeople.DataSource = peopleTable.DefaultView;

            lblRecordsNumber.Text = dgvPeople.Rows.Count.ToString();
        }
        private void dgvPeople_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //To change the 0 and 1 of gendor column to Male and Female

            if (dgvPeople.Columns[e.ColumnIndex].Name == "Gendor" && e.Value != null)
            {
                e.Value = e.Value.ToString() == "0" ? "Male" : "Female";
                e.FormattingApplied = true;
            }

            if (dgvPeople.Columns[e.ColumnIndex].Name == "NationalityCountryID" && e.Value != null)
            {
                int CountryID = Convert.ToInt32(e.Value);

                e.Value = clsCountry.FindCountry(CountryID).CountryName;
                e.FormattingApplied = true;   
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Clear();
            cbGendorFilter.SelectedIndex = 0;
            _RefreshPeopleList();
            

            enFilterOptions Option = (enFilterOptions)cbSelectFilterType.SelectedIndex;

            tbFilter.Visible = false;
            cbNationalityFilter.Visible = false;
            cbGendorFilter.Visible = false;

            switch (Option)
            {
                case enFilterOptions.None:
                    break;

                case enFilterOptions.Gendor:
                    cbGendorFilter.Visible = true;
                    break;

                case enFilterOptions.Nationality:
                    cbNationalityFilter.Visible = true;
                    break;

                default:
                    tbFilter.Visible = true;
                    break;
            }
        }
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) 
                && cbSelectFilterType.SelectedIndex == cbSelectFilterType.FindString("PersonID"))
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
                    _RefreshPeopleList("FirstName", "");
                    break;
                case enFilterOptions.PersonID:
                    _RefreshPeopleList("PersonID", tbFilter.Text);
                    break;
                case enFilterOptions.NationalNumber:
                    _RefreshPeopleList("NationalNo", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.FirstName:
                    _RefreshPeopleList("FirstName", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.SecondName:
                    _RefreshPeopleList("SecondName", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.ThirdName:
                    _RefreshPeopleList("ThirdName", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.LastName:
                    _RefreshPeopleList("LastName", "%" + tbFilter.Text + "%");
                    break; 
                case enFilterOptions.Email:
                    _RefreshPeopleList("Email", "%" + tbFilter.Text + "%");
                    break;
                case enFilterOptions.Phone:
                    _RefreshPeopleList("Phone", "%" + tbFilter.Text + "%");
                    break;
            }
        }
        private void cbGendorFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGendorFilter.SelectedIndex == cbGendorFilter.FindString("All"))
            {
                _RefreshPeopleList("Gendor", "");
                return;
            }

            if (cbGendorFilter.SelectedIndex == cbGendorFilter.FindString("Male"))
                _RefreshPeopleList("Gendor", "0");
            else
                _RefreshPeopleList("Gendor", "1");
        }
        private void cbNationalityFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CountryID = Convert.ToString(clsCountry.FindCountry(cbNationalityFilter.Text).CountryID);

            _RefreshPeopleList("NationalityCountryID", CountryID);
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowDetails frm = new frmShowDetails(((int)dgvPeople.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshPeopleList();
        }
        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(-1);

            frm.ShowDialog();

            _RefreshPeopleList();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentPersonID = (int)(dgvPeople.CurrentRow.Cells[0].Value);

            frmAddUpdatePerson frm = new frmAddUpdatePerson(currentPersonID);
            
            frm.ShowDialog();

            _RefreshPeopleList();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value);

                MessageBox.Show("The person has been deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The person was not deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            _RefreshPeopleList();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(-1);

            frm.ShowDialog();

            _RefreshPeopleList();
        }
    }
}
