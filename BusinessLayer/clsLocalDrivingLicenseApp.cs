using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsLocalDrivingLicenseApp
    {
        private enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public clsLocalDrivingLicenseApp()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;

            _Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApp(int localAppID, int applicationID, int licenseClassID)
        {
            LocalDrivingLicenseApplicationID = localAppID;
            ApplicationID = applicationID;
            LicenseClassID = licenseClassID;

            _Mode = enMode.Update;
        }

        public static clsLocalDrivingLicenseApp Find(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            bool isFound = clsLocalDrivingLicenseApplicationsData.GetLDLApplicationID(
                LocalDrivingLicenseApplicationID,
                ref ApplicationID,
                ref LicenseClassID);

            if (isFound)
            {
                return new clsLocalDrivingLicenseApp(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);
            }

            return null;
        }

        private bool _AddNew()
        {
            this.LocalDrivingLicenseApplicationID =
                clsLocalDrivingLicenseApplicationsData.AddNewApplication(this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _Update()
        {
            return clsLocalDrivingLicenseApplicationsData.UpdateApplication(
                this.LocalDrivingLicenseApplicationID,
                this.ApplicationID,
                this.LicenseClassID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _Update();
            }

            return false;
        }

        public static bool Delete(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.DeleteApplication(LocalDrivingLicenseApplicationID);
        }

        public static DataTable GetAll()
        {
            return clsLocalDrivingLicenseApplicationsData.GetAllApplications();
        }

        public static bool Exists(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsApplicationExists(LocalDrivingLicenseApplicationID);
        }

        public static int GetApplicationIDByLDLAppID(int LDLApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.GetApplicationIDByLDLApplicationID(LDLApplicationID);
        }
    }
}
