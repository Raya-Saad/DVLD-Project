using DataAccessLayer;
using System;
using System.Data;
using System.Xml.Linq;

namespace BusinessLayer
{
    public class clsLicenseClass
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode = enMode.Update;

        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        private clsLicenseClass(int id, string name, string description, byte minAge, byte validity, decimal fees)
        {
            LicenseClassID = id;
            ClassName = name;
            ClassDescription = description;
            MinimumAllowedAge = minAge;
            DefaultValidityLength = validity;
            ClassFees = fees;

            _Mode = enMode.Update;
        }

        public static clsLicenseClass Find(int id)
        {
            string name = "", description = "";
            byte age = 18, validity = 1;
            decimal fees = 0;

            bool found = clsLicenseClassesData.GetLicenseClassByID(id, ref name, ref description, ref age, ref validity, ref fees);

            if (found)
                return new clsLicenseClass(id, name, description, age, validity, fees);

            return null;
        }

        public static clsLicenseClass Find(string name)
        {
            string description = "";
            int id = -1;
            byte age = 18, validity = 1;
            decimal fees = 0;

            bool found = clsLicenseClassesData.GetLicenseClassByName(ref id, name, ref description, ref age, ref validity, ref fees);

            if (found)
                return new clsLicenseClass(id, name, description, age, validity, fees);

            return null;
        }

        public bool Save()
        {
            return clsLicenseClassesData.UpdateLicenseClass(
                LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
        }

        public static DataTable GetAll()
        {
            return clsLicenseClassesData.GetAllLicenseClasses();
        }
    }
}
