using System;
using System.Data;
using System.Windows.Forms;
using BusinessLayer;
using DVLD_Proj.UserControls;

namespace DVLD_Proj
{
    public partial class frmTestAppointment : Form
    {
        private int _TestType = -1;
        private int _DLAppID = -1;
        public frmTestAppointment(int DLAppID, int TestType)
        {
            InitializeComponent();

            _DLAppID = DLAppID;
            _TestType = TestType;

            ctrDLApplicationAndApplicationInfo1.DLApplicationID = _DLAppID;
        }
        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvAppointmentsList.Rows)
            {
                if (Convert.ToInt32(row.Cells["Is Locked"].Value) == 0)
                {
                    MessageBox.Show("This person already has an active appointment for this test. You cannot add a new appointment.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                clsTest Test = clsTest.FindByTestAppoinmentID(Convert.ToInt32(row.Cells[0].Value));

                if (Test != null)
                {
                    if (Test.TestResult == true)
                    {
                        MessageBox.Show("This person has already passed this test. You cannot add a new appointment.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        return;
                    }
                }
            }

            frmScheduleTest frm = new frmScheduleTest(_DLAppID, _TestType, -1);

            frm.ShowDialog();

            _RefreshAppointmentsList();
        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            frmTakeTest frm = new frmTakeTest(_DLAppID, _TestType, (int)dgvAppointmentsList.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            _RefreshAppointmentsList();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(dgvAppointmentsList.CurrentRow.Cells["Is Locked"].Value) == true)
            //{
            //    MessageBox.Show("This Test Is Already Done, You Can't Edit it", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //    return;
            //}

            frmScheduleTest frm = new frmScheduleTest(_DLAppID, _TestType, Convert.ToInt32(dgvAppointmentsList.CurrentRow.Cells[0].Value));

            frm.ShowDialog();

            _RefreshAppointmentsList();
        }
        private void _RefreshAppointmentsList()
        {
            DataTable table = clsTestAppointment.GetAllWithApplicationIDAndType(_DLAppID, _TestType);

            if(table.Columns.Count != 0)
            {
                table.Columns.Remove("TestTypeID");
                table.Columns.Remove("LocalDrivingLicenseApplicationID");
                table.Columns.Remove("CreatedByUserID");

                table.Columns[0].ColumnName = "Appointment ID";
                table.Columns[1].ColumnName = "Appointment Date";
                table.Columns[2].ColumnName = "Paid Fees";
                table.Columns[3].ColumnName = "Is Locked";
            }
            
            dgvAppointmentsList.DataSource = table.DefaultView;

            lblRecordsNumber.Text = dgvAppointmentsList.Rows.Count.ToString();
        }
        private void frmTestAppointment_Load(object sender, EventArgs e)
        {
            _LoadTitle(_TestType);

            _RefreshAppointmentsList();
        }
        private void _LoadTitle(int TestType)
        {
            switch (_TestType)
            {
                case 1:
                    lblTitle.Text = "Vistion Test Appointment";
                    break;
                case 2:
                    lblTitle.Text = "Theory Test Appointment";
                    break;
                case 3:
                    lblTitle.Text = "Practical Test Appointment";
                    break;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
