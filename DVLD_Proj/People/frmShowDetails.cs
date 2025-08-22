using System;
using System.Windows.Forms;

namespace DVLD_Proj
{
    public partial class frmShowDetails : Form
    {
        int _PersonID;
        public frmShowDetails(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowDetails_Load(object sender, EventArgs e)
        {
            personInfo1.LoadInfo(_PersonID);
        }
    }
}
