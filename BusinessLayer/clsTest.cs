using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsTest
    {
        private enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;

            _Mode = enMode.AddNew;
        }

        private clsTest(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;

            _Mode = enMode.Update;
        }

        public static clsTest Find(int testID)
        {
            int testAppointmentID = -1;
            bool testResult = false;
            string notes = "";
            int createdByUserID = -1;

            bool isFound = clsTestsData.GetTestByID(testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUserID);

            if (isFound)
            {
                return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
            }

            return null;
        }

        public static clsTest FindByTestAppoinmentID(int TAppID)
        {
            int testID = -1;
            bool testResult = false;
            string notes = "";
            int createdByUserID = -1;

            bool isFound = clsTestsData.GetTestByTestAppointmentID(ref testID, TAppID, ref testResult, ref notes, ref createdByUserID);

            if (isFound)
            {
                return new clsTest(testID, TAppID, testResult, notes, createdByUserID);
            }

            return null;
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestsData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return this.TestID != -1;
        }

        private bool _UpdateTest()
        {
            return clsTestsData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static bool DeleteTest(int testID)
        {
            return clsTestsData.DeleteTest(testID);
        }

        public static DataTable GetAllTests()
        {
            return clsTestsData.GetAllTests();
        }

        public static bool IsTestExists(int testID)
        {
            return clsTestsData.IsTestExists(testID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateTest();
            }

            return false;
        }
    }
}
