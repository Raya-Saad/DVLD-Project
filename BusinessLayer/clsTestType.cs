using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsTestType
    {
        public int TestTypeID;
        public string TestTypeTitle;
        public string TestTypeDescription;
        public double TestFees;

        public clsTestType()
        {
            TestTypeID = -1;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestFees = 0.0;
        }

        public clsTestType(int testTypeID, string testTypeTitle, string testTypeDescription, double testFees)
        {
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription = testTypeDescription;
            TestFees = testFees;
        }

        public static clsTestType Find(int testTypeID)
        {
            string title = "";
            string description = "";
            double fees = 0.0;

            if (clsTestTypesData.GetTestTypeByID(testTypeID, ref title, ref description, ref fees))
            {
                return new clsTestType(testTypeID, title, description, fees);
            }

            return null;
        }

        private bool _Update()
        {
            return clsTestTypesData.UpdateTestType(this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestFees);
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }

        public bool Save()
        {
            return _Update();
        }
    }
}
