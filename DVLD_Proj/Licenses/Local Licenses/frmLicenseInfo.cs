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
    public partial class frmLicenseInfo : Form
    {
        private int _DLApplicationID;
        private clsLicense _License;
        public frmLicenseInfo(int DLApplicationID)
        {
            InitializeComponent();

            _DLApplicationID = DLApplicationID;
        }

        public frmLicenseInfo(clsLicense License)
        {
            InitializeComponent();

            _License = License;
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLicenseInfo_Load(object sender, EventArgs e)
        {
            //constructor 2 used
            if (_License != null)
            {
                ctrDrivingLicenseInfo1.LoadInfoWithLicense(_License);
                return;
            }

            //constructor 1 used
            ctrDrivingLicenseInfo1.DLApplicationID = _DLApplicationID;
            ctrDrivingLicenseInfo1.LoadInfo();
        }
    }
}
