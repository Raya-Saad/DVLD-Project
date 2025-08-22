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
    public partial class frmShowUserDetails : Form
    {
        private int _userID;

        public frmShowUserDetails(int UserID)
        {
            InitializeComponent();

            _userID = UserID;
        }

        private void frmShowUserDetails_Load(object sender, EventArgs e)
        {
            ctrUserInfo1.LoadUserInfo(_userID);
        }
    }
}
