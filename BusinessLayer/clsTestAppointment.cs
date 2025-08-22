using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsTestAppointment
    {
        private enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }

        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
            IsLocked = false;

            _Mode = enMode.AddNew;
        }

        private clsTestAppointment(int id, int typeID, int appID, DateTime date, float fees, int userID, bool isLocked)
        {
            TestAppointmentID = id;
            TestTypeID = typeID;
            LocalDrivingLicenseApplicationID = appID;
            AppointmentDate = date;
            PaidFees = fees;
            CreatedByUserID = userID;
            IsLocked = isLocked;

            _Mode = enMode.Update;
        }

        public static clsTestAppointment Find(int id)
        {
            int typeID = -1, appID = -1, userID = -1;
            DateTime date = DateTime.Now;
            float fees = 0;
            bool isLocked = false;

            bool found = clsTestAppointmentsData.GetTestAppointmentByID(id, ref typeID, ref appID, ref date, ref fees, ref userID, ref isLocked);

            if (found)
                return new clsTestAppointment(id, typeID, appID, date, fees, userID, isLocked);

            return null;
        }

        public static clsTestAppointment FindByLocalAppID(int localAppID)
        {
            int testAppointmentID = -1, typeID = -1, userID = -1;
            DateTime date = DateTime.Now;
            float fees = 0;
            bool isLocked = false;

            bool found = clsTestAppointmentsData.GetTestAppointmentByLocalAppID(localAppID, ref testAppointmentID, ref typeID, ref date, ref fees, ref userID, ref isLocked);

            if (found)
                return new clsTestAppointment(testAppointmentID, typeID, localAppID, date, fees, userID, isLocked);

            return null;
        }

        public static clsTestAppointment FindByLocalAppIDAndTestType(int localAppID, int testType)
        {
            int testAppointmentID = -1, userID = -1;
            DateTime date = DateTime.Now;
            float fees = 0;
            bool isLocked = false;

            bool found = clsTestAppointmentsData.GetTestAppointmentByLocalAppIDAndTestType
                (localAppID, testType, ref testAppointmentID, ref date, ref fees, ref userID, ref isLocked);

            if (found)
                return new clsTestAppointment(testAppointmentID, testType, localAppID, date, fees, userID, isLocked);

            return null;
        }

        private bool _AddNew()
        {
            TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment(TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked);
            return TestAppointmentID != -1;
        }

        private bool _Update()
        {
            return clsTestAppointmentsData.UpdateTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked);
        }

        public static bool Delete(int id)
        {
            return clsTestAppointmentsData.DeleteTestAppointment(id);
        }

        public static DataTable GetAll()
        {
            return clsTestAppointmentsData.GetAllTestAppointments();
        }

        public static DataTable GetAllWithApplicationIDAndType(int localDrivingLicenseAppID, int testTypeID)
        {
            return clsTestAppointmentsData.GetAllTestAppointmentsWithApplicationIDAndType(localDrivingLicenseAppID, testTypeID);
        }
        public static bool Exists(int id)
        {
            return clsTestAppointmentsData.IsTestAppointmentExists(id);
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


        public static int GetTrials(int TestTypeID, int LDLAppID)
        {
            return clsTestAppointmentsData.GetTrials(TestTypeID, LDLAppID);
        }
    }
}
