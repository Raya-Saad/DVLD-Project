using BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD_Proj.UserControls
{
    public partial class ctrPersonInfoWithFilter : UserControl
    {
        private clsPerson _person;

        public ComboBox cbFindby
        {
            get { return  cbFindBy; }
        }
        public TextBox tbFindby
        {
            get { return this.tbFindBy; }
        }
        public GroupBox gbFilter
        {
            get { return groupBox2; }
        }
        public ctrPersonInfo PersonInfo
        {
            get { return ctrPersonInfo1; }
        }

        public ctrPersonInfoWithFilter()
        {
            InitializeComponent();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(-1);

            frm.del += FormAddNewPersonDataBack;

            frm.ShowDialog();
        }

        private void FormAddNewPersonDataBack(object sender, int value)
        {
            ctrPersonInfo1.LoadInfo(value);
        }

        private void cbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFindBy.Clear();
        }

        private void tbFindBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                && cbFindBy.SelectedIndex == cbFindBy.FindString("Person ID"))
            {
                e.Handled = true; // Block the input
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            _person = null;

            string filterValue = tbFindBy.Text.ToString();

            if (string.IsNullOrEmpty(filterValue))
            {
                MessageBox.Show("Please fill in the text box.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (cbFindBy.Text.Trim().Equals("Person ID"))
            {
                _person = clsPerson.FindPerson(Convert.ToInt32(filterValue));

                if (_person == null)
                {
                    MessageBox.Show($"No person was found with Person ID = {filterValue}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            else
            {
                _person = clsPerson.FindPerson((string)tbFindBy.Text.ToString());

                if (_person == null)
                {
                    MessageBox.Show($"No person was found with National No. = {filterValue}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            ctrPersonInfo1.LoadInfo(_person.PersonID);
        }
    }
}
