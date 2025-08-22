using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD_Proj.UserControls
{
    public partial class ctrTest : UserControl
    {
        public Image TestImage { get; set; }
        public int DLApplicationID { get; set; }
        public string DLClass { get; set; }
        public string PersonName { get; set; }
        public byte Trials { get; set; }

        private DateTime _appointmentDate;
        public DateTime AppointmentDate
        {
            get => date.Value;
            set
            {
                if (value < date.MinDate || value == DateTime.MinValue)
                    date.Value = DateTime.Now;
                else
                    date.Value = value;

                _appointmentDate = date.Value;
            }
        }
        public float PaidFees { get; set; }
        
        public ctrTest()
        {
            InitializeComponent();
        }

        private void ctrTest_Load(object sender, EventArgs e)
        {
            date.MinDate = DateTime.Now;
        }

        public void LoadData()
        {
            pbImage.Image = TestImage;
            lblDLAppID.Text = DLApplicationID.ToString();
            lblDClass.Text = DLClass.ToString();
            lblName.Text = PersonName.ToString();
            lblTrial.Text = Trials.ToString();
            date.Value = _appointmentDate;
            lblFees.Text = PaidFees.ToString();
        }

        public void HideDatePicker()
        {
            date.Visible = false;
        }

        public void DisableDatePicker()
        {
            date.Enabled = false;
        }
    }
}
