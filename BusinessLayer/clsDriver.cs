using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsDriver
    {
        enum enMode { AddNew = 0, Update = 1 }
        private enMode _mode;

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDriver()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;

            _mode = enMode.AddNew;
        }

        private clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            DriverID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;

            _mode = enMode.Update;
        }

        public static clsDriver Find(int driverID)
        {
            int personID = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            bool found = clsDriversData.GetDriverByID(driverID, ref personID, ref createdByUserID, ref createdDate);

            if (found)
                return new clsDriver(driverID, personID, createdByUserID, createdDate);

            return null;
        }
        
        public static clsDriver FindByPersonID(int PersonID)
        {
            int driverID = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            bool found = clsDriversData.GetDriverByPersonID(ref driverID, PersonID, ref createdByUserID, ref createdDate);

            if (found)
                return new clsDriver(driverID, PersonID, createdByUserID, createdDate);

            return null;
        }

        private bool _AddNewDriver()
        {
            DriverID = clsDriversData.AddNewDriver(PersonID, CreatedByUserID, CreatedDate);
            return DriverID != -1;
        }

        public bool Save()
        {
            if (_mode == enMode.AddNew)
            {
                if (_AddNewDriver())
                {
                    _mode = enMode.Update;
                    return true;
                }
                return false;
            }

            // No update logic yet as table doesn't include editable fields
            return false;
        }

        public static bool Delete(int driverID)
        {
            return clsDriversData.DeleteDriver(driverID);
        }

        public static bool IsExists(int driverID)
        {
            return clsDriversData.IsDriverExists(driverID);
        }

        public static DataTable GetAll()
        {
            return clsDriversData.GetAllDrivers();
        }
    }
}
