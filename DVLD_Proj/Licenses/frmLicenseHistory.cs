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
    public partial class frmLicenseHistory : Form
    {
        private string _NationalNo;
        public frmLicenseHistory(string NationalNo)
        {
            InitializeComponent();
            _NationalNo = NationalNo;
        }
        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            clsPerson person = clsPerson.FindPerson(this._NationalNo);
            int personID = person.PersonID;

            ctrPersonInfo1.LoadInfo(personID);
            ctrDriverLicenses1.LoadInfo(clsDriver.FindByPersonID(personID).DriverID);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
