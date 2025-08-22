using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsApplication
    {
        private enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        public clsApplication()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = -1;
            ApplicationStatus = 1; // New
            LastStatusDate = DateTime.Now;
            PaidFees = 15;
            CreatedByUserID = -1;

            _Mode = enMode.AddNew;
        }

        private clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate,
            int applicationTypeID, byte applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;

            _Mode = enMode.Update;
        }

        public static clsApplication Find(int applicationID)
        {
            int applicantPersonID = -1, applicationTypeID = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            byte applicationStatus = 1;
            decimal paidFees = 0;

            bool isFound = clsApplicationsData.GetApplicationByID(applicationID,
                ref applicantPersonID, ref applicationDate, ref applicationTypeID,
                ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID);

            if (isFound)
            {
                return new clsApplication(applicationID, applicantPersonID, applicationDate,
                    applicationTypeID, applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }

            return null;
        }

        public static clsApplication FindByPersonID(int personID)
        {
            int applicationID = -1, applicationTypeID = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            byte applicationStatus = 1;
            decimal paidFees = 0;

            bool isFound = clsApplicationsData.GetApplicationByPersonID(personID,
                ref applicationID, ref applicationDate, ref applicationTypeID,
                ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID);

            if (isFound)
            {
                return new clsApplication(applicationID, personID, applicationDate,
                    applicationTypeID, applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }

            return null;
        }

        private bool _AddNew()
        {
            this.ApplicationID = clsApplicationsData.AddNewApplication(ApplicantPersonID, ApplicationDate,
                ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _Update()
        {
            return clsApplicationsData.UpdateApplication(ApplicationID, ApplicantPersonID, ApplicationDate,
                ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
        }

        public static bool Delete(int applicationID)
        {
            return clsApplicationsData.DeleteApplication(applicationID);
        }

        public static bool Exists(int applicationID)
        {
            return clsApplicationsData.IsApplicationExists(applicationID);
        }

        public static DataTable GetAll()
        {
            return clsApplicationsData.GetAllApplications();
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




        //not used
        public static clsApplication FindByPersonIDInStatusWithType(int personID, byte status, int Type)
        {
            int applicationID = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;

            bool isFound = clsApplicationsData.GetApplicationByPersonIDInStatusWithType(
                personID, status, Type,
                ref applicationID, ref applicationDate,
                ref lastStatusDate, ref paidFees, ref createdByUserID);

            if (isFound)
            {
                return new clsApplication(applicationID, personID, applicationDate,
                    Type, status, lastStatusDate, paidFees, createdByUserID);
            }

            return null;
        }

    }
}
