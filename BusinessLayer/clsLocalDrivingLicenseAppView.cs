using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsLocalDrivingLicenseAppView
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public string ClassName { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int PassedTestCount { get; set; }
        public string Status { get; set; }

        private clsLocalDrivingLicenseAppView(int id, string className, string nationalNo, string fullName,
            DateTime appDate, int passedCount, string status)
        {
            LocalDrivingLicenseApplicationID = id;
            ClassName = className;
            NationalNo = nationalNo;
            FullName = fullName;
            ApplicationDate = appDate;
            PassedTestCount = passedCount;
            Status = status;
        }

        public static clsLocalDrivingLicenseAppView Find(int LDLAppID)
        {
            string className = "", nationalNo = "", fullName = "", status = "";
            DateTime appDate = DateTime.MinValue;
            int passedTestCount = 0;

            bool isFound = clsLocalDrivingLicenseAppViewData.GetApplicationByID(
                LDLAppID,
                ref className,
                ref nationalNo,
                ref fullName,
                ref appDate,
                ref passedTestCount,
                ref status);

            if (isFound)
            {
                return new clsLocalDrivingLicenseAppView(LDLAppID, className, nationalNo, fullName,
                                                         appDate, passedTestCount, status);
            }

            return null;
        }

        
        public static DataTable GetAll()
        {
            return clsLocalDrivingLicenseAppViewData.GetAllApplications();
        }

        public static bool IsApplicationExists(int applicationID)
        {
            return clsLocalDrivingLicenseAppViewData.IsApplicationExists(applicationID);
        }

        
        
        
        
        //not used
        public static bool IsThereAlreadyNewApp(string ClassName, string NationalNo, string Status)
        {
            return clsLocalDrivingLicenseAppViewData.IsThereAlreadyNewApp(ClassName, NationalNo, Status);
        }

        public static int GetApplicationByNationalNoInClassAndStatus(string ClassName, string NationalNo, string Status)
        {
            return clsLocalDrivingLicenseAppViewData.GetApplicationByNationalNoInClassAndStatus(ClassName, NationalNo, Status); 
        }
    }
}
