using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsTestAppointments_View
    {
        public int TestAppointmentID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public string TestTypeTitle { get; set; }
        public string ClassName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public double PaidFees { get; set; }
        public string FullName { get; set; }
        public bool IsLocked { get; set; }

        public clsTestAppointments_View()
        {
            TestAppointmentID = -1;
            LocalDrivingLicenseApplicationID = -1;
            TestTypeTitle = "";
            ClassName = "";
            AppointmentDate = DateTime.Now;
            PaidFees = 0.0;
            FullName = "";
            IsLocked = false;
        }

        private clsTestAppointments_View(int testAppointmentID, int localDrivingLicenseApplicationID,
            string testTypeTitle, string className, DateTime appointmentDate,
            double paidFees, string fullName, bool isLocked)
        {
            TestAppointmentID = testAppointmentID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            TestTypeTitle = testTypeTitle;
            ClassName = className;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            FullName = fullName;
            IsLocked = isLocked;
        }

        public static clsTestAppointments_View Find(int testAppointmentID)
        {
            int localDrivingLicenseApplicationID = -1;
            string testTypeTitle = "", className = "", fullName = "";
            DateTime appointmentDate = DateTime.Now;
            double paidFees = 0;
            bool isLocked = false;

            bool isFound = DataAccessLayer.clsTestAppointments_ViewData.GetAppointmentByID(
                testAppointmentID,
                ref localDrivingLicenseApplicationID,
                ref testTypeTitle,
                ref className,
                ref appointmentDate,
                ref paidFees,
                ref fullName,
                ref isLocked
            );

            if (isFound)
            {
                return new clsTestAppointments_View(
                    testAppointmentID,
                    localDrivingLicenseApplicationID,
                    testTypeTitle,
                    className,
                    appointmentDate,
                    paidFees,
                    fullName,
                    isLocked
                );
            }

            return null;
        }

        public static bool IsExists(int testAppointmentID)
        {
            return DataAccessLayer.clsTestAppointments_ViewData.IsAppointmentExists(testAppointmentID);
        }

        public static DataTable GetAll()
        {
            return DataAccessLayer.clsTestAppointments_ViewData.GetAllAppointments();
        }
    }
}
