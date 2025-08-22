using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsApplicationType
    {
        public int AppTypeID;
        public string AppTypeTitle;
        public double AppFees;

        public clsApplicationType() 
        { 
            AppTypeID = -1;
            AppTypeTitle = "";
            AppFees = 0.0;
        }

        public clsApplicationType(int AppTypeID, string AppTypeTitle, double AppFees)
        {
            this.AppTypeID = AppTypeID;
            this.AppTypeTitle = AppTypeTitle;
            this.AppFees = AppFees;
        }

        public static clsApplicationType FindApplicationType(int AppTypeID)
        {
            string AppTypeTitle = "";
            double AppFees = 0.0;

            if (DataAccessLayer.clsApplicationTypesData.GetApplicationTypeByID(AppTypeID, ref AppTypeTitle, ref AppFees))
            {
                return new clsApplicationType(AppTypeID, AppTypeTitle, AppFees);
            }

            else
                return null;
        }

        private bool _UpdateApplicationType()
        {
            return (clsApplicationTypesData.UpdateApplicationType(this.AppTypeID, this.AppTypeTitle, this.AppFees));
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }

        public bool Save()
        {
            return _UpdateApplicationType();
        }
    }
}
