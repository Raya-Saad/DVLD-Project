using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsLicense
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int LicenseID { get; private set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public byte IssueReason { get; set; }
        public int CreatedByUserID { get; set; }

        public clsLicense()
        {
            _Mode = enMode.AddNew;
            LicenseID = -1;
            Notes = "";
            IsActive = true;
        }

        private clsLicense(int licenseID, int applicationID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, byte issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;
            _Mode = enMode.Update;
        }

        public static clsLicense Find(int licenseID)
        {
            int applicationID = 0, driverID = 0, licenseClass = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = true;
            byte issueReason = 0;

            bool isFound = clsLicensesData.GetLicenseByID(licenseID, ref applicationID, ref driverID, ref licenseClass, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID);

            if (isFound)
                return new clsLicense(licenseID, applicationID, driverID, licenseClass, issueDate, expirationDate, notes, paidFees, isActive, issueReason, createdByUserID);

            return null;
        }

        public static clsLicense FindByApplicationID(int applicationID)
        {
            int LicenseID = 0, driverID = 0, licenseClass = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = true;
            byte issueReason = 0;

            bool isFound = clsLicensesData.GetLicenseByApplicationID(applicationID, ref LicenseID, ref driverID, ref licenseClass, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID);

            if (isFound)
                return new clsLicense(LicenseID, applicationID, driverID, licenseClass, issueDate, expirationDate, notes, paidFees, isActive, issueReason, createdByUserID);

            return null;
        }

        private bool _AddNew()
        {
            LicenseID = clsLicensesData.AddNewLicense(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
            return LicenseID != -1;
        }

        private bool _Update()
        {
            return clsLicensesData.UpdateLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_AddNew())
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

        public static bool DeleteLicense(int licenseID)
        {
            return clsLicensesData.DeleteLicense(licenseID);
        }

        public static bool IsLicenseExists(int licenseID)
        {
            return clsLicensesData.IsLicenseExists(licenseID);
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicensesData.GetAllLicenses();
        }

        public static DataTable GetAllLicensesWithDriverID(int DriverID)
        {
            return clsLicensesData.GetAllLicensesWithDriverID(DriverID);
        }

        public static bool DoesPersonHaveLicenseWithClass(int DriverID, int LicenseClass)
        {
            return clsLicensesData.DoesPersonHaveLicenseWithClass(DriverID, LicenseClass);
        }
    }
}
