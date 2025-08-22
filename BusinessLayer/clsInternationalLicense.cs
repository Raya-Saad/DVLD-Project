using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsInternationalLicense
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode = enMode.AddNew;

        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public clsInternationalLicense()
        {
            InternationalLicenseID = -1;
            ApplicationID = DriverID = IssuedUsingLocalLicenseID = CreatedByUserID = -1;
            IssueDate = ExpirationDate = DateTime.Now;
            IsActive = true;
            _Mode = enMode.AddNew;
        }

        private clsInternationalLicense(int id, int appID, int driverID, int localLicenseID, DateTime issueDate, DateTime expirationDate, bool isActive, int userID)
        {
            InternationalLicenseID = id;
            ApplicationID = appID;
            DriverID = driverID;
            IssuedUsingLocalLicenseID = localLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUserID = userID;

            _Mode = enMode.Update;
        }

        public static clsInternationalLicense Find(int ID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;

            bool found = clsInternationalLicensesData.GetLicenseByID(ID, ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID);

            if (found)
                return new clsInternationalLicense(ID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            else
                return null;
        }

        public static clsInternationalLicense FindByLocalLicenseID(int LocalLID)
        {
            int InternationalLicenseID = -1, ApplicationID = -1, DriverID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;

            bool found = clsInternationalLicensesData.GetLicenseByLocalLicenseID(ref InternationalLicenseID, ref ApplicationID, ref DriverID, LocalLID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID);

            if (found)
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, LocalLID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            else
                return null;
        }

        private bool _Add()
        {
            this.InternationalLicenseID = clsInternationalLicensesData.AddNewLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            return this.InternationalLicenseID != -1;
        }

        private bool _Update()
        {
            return clsInternationalLicensesData.UpdateLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
        }

        public static bool Delete(int ID)
        {
            return clsInternationalLicensesData.DeleteLicense(ID);
        }

        public static DataTable GetAll()
        {
            return clsInternationalLicensesData.GetAllLicenses();
        }

        public static DataTable GetAllWithDriverID(int DriverID)
        {
            return clsInternationalLicensesData.GetAllLicensesWithDriverID(DriverID);
        }
        public static bool IsExists(int ID)
        {
            return clsInternationalLicensesData.IsLicenseExists(ID);
        }

        public static bool IsExistsUsingLocalLicenseID(int LocalLID)
        {
            return clsInternationalLicensesData.IsLicenseExistsWithLocalLicenseID(LocalLID);
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_Add())
                {
                    _Mode = enMode.Update;
                    return true;
                }
                return false;
            }
            else
            {
                return _Update();
            }
        }
    }
}
