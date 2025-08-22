using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsUser
    {
        enum enMode { AddNew = 0, Update = 1};

        enMode _mode = enMode.AddNew;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = false;

            _mode = enMode.AddNew;
        }

        public clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            _mode = enMode.Update;
        }

        public static clsUser FindUser(int UserID)
        {
            int personID = -1;
            string userName = "", password = "";
            bool isActive = false;

            if (clsUsersData.GetUserByID(UserID, ref personID, ref userName, ref password, ref isActive))
            {
                return new clsUser(UserID, personID, userName, password, isActive);
            }
            else
                return null;
        }

        public static clsUser FindUser(string UserName)
        {
            int UserID = -1, personID = -1;
            string password = "";
            bool isActive = false;

            if (clsUsersData.GetUserByUserName(ref UserID, ref personID, UserName, ref password, ref isActive))
            {
                return new clsUser(UserID, personID, UserName, password, isActive);
            }
            else
                return null;
        }

        private bool _AddNewUser()
        {
            int UserID = clsUsersData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);

            return UserID != -1;
        }

        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }

        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUsersData.IsUserExists(UserID);
        }

        public static bool IsPersonLinkedToUser(int PersonID)
        {
            return clsUsersData.IsPersonLinkedToUser(PersonID);
        }

        public bool Save()
        {
            switch(this._mode)
            {
                case enMode.AddNew:

                    if (_AddNewUser())
                    {
                        _mode = enMode.Update;
                        return true;
                    }
                    else return false;

                case enMode.Update:
                    return (_UpdateUser());
            }

            return false;
        }
    }
}
