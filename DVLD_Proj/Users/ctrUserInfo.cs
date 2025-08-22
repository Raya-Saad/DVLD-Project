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

namespace DVLD_Proj.UserControls
{
    public partial class ctrUserInfo : UserControl
    {
        private clsPerson _person;
        private clsUser _user;
        
        //public PersonInfo personInfo
        //{
        //    get { return this.personInfo1; }
        //    set { this.personInfo1 = value; }
        //}

        public ctrUserInfo()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _user = clsUser.FindUser(UserID);

            if (_user != null)
            {
                personInfo1.LoadInfo(_user.PersonID);

                lblUserID.Text = _user.UserID.ToString();
                lblUserName.Text = _user.UserName.ToString();

                if (_user.IsActive)
                {
                    lblActive.Text = "Yes";
                }
                else
                    lblActive.Text = "No";
            }
        }

    }
}
