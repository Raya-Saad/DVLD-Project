using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsDriver_View
    {
        public int DriverID { get; private set; }
        public string FullName { get; private set; }
        public string NationalNo { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string CreatedByUserName { get; private set; }

        private clsDriver_View(int driverID, string fullName, string nationalNo, DateTime createdDate, string createdByUserName)
        {
            DriverID = driverID;
            FullName = fullName;
            NationalNo = nationalNo;
            CreatedDate = createdDate;
            CreatedByUserName = createdByUserName;
        }

        public static clsDriver_View Find(int driverID)
        {
            string fullName = "", nationalNo = "", createdByUserName = "";
            DateTime createdDate = DateTime.Now;

            bool found = clsDrivers_ViewData.GetDriverByID(driverID, ref fullName, ref nationalNo, ref createdDate, ref createdByUserName);

            if (found)
                return new clsDriver_View(driverID, fullName, nationalNo, createdDate, createdByUserName);

            return null;
        }

        public static bool IsExists(int driverID)
        {
            return clsDrivers_ViewData.IsDriverExists(driverID);
        }

        public static DataTable GetAll()
        {
            return clsDrivers_ViewData.GetAllDrivers();
        }
    }
}
