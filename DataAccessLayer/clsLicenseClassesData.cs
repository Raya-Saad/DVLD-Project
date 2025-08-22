using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class clsLicenseClassesData
    {
        public static bool GetLicenseClassByID(int licenseClassID, ref string className, ref string classDescription,
            ref byte minimumAllowedAge, ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;
                        className = reader["ClassName"].ToString();
                        classDescription = reader["ClassDescription"].ToString();
                        minimumAllowedAge = Convert.ToByte(reader["MinimumAllowedAge"]);
                        defaultValidityLength = Convert.ToByte(reader["DefaultValidityLength"]);
                        classFees = Convert.ToDecimal(reader["ClassFees"]);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error in GetLicenseClassByID:\n" + ex.Message);
                }
            }

            return isFound;
        }

        public static bool GetLicenseClassByName(ref int licenseClassID, string ClassName, ref string classDescription,
            ref byte minimumAllowedAge, ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClassName", ClassName);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;
                        licenseClassID = Convert.ToInt32(reader["licenseClassID"]);
                        classDescription = reader["ClassDescription"].ToString();
                        minimumAllowedAge = Convert.ToByte(reader["MinimumAllowedAge"]);
                        defaultValidityLength = Convert.ToByte(reader["DefaultValidityLength"]);
                        classFees = Convert.ToDecimal(reader["ClassFees"]);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error in GetLicenseClassByName:\n" + ex.Message);
                }
            }

            return isFound;
        }

        public static bool UpdateLicenseClass(int licenseClassID, string className, string classDescription,
            byte minAge, byte defaultYears, decimal classFees)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
                    UPDATE LicenseClasses SET
                        ClassName = @ClassName,
                        ClassDescription = @ClassDescription,
                        MinimumAllowedAge = @MinAge,
                        DefaultValidityLength = @DefaultYears,
                        ClassFees = @ClassFees
                    WHERE LicenseClassID = @LicenseClassID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                cmd.Parameters.AddWithValue("@ClassName", className);
                cmd.Parameters.AddWithValue("@ClassDescription", classDescription);
                cmd.Parameters.AddWithValue("@MinAge", minAge);
                cmd.Parameters.AddWithValue("@DefaultYears", defaultYears);
                cmd.Parameters.AddWithValue("@ClassFees", classFees);

                try
                {
                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error in UpdateLicenseClass:\n" + ex.Message);
                }
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM LicenseClasses";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                   // MessageBox.Show("Error in GetAllLicenseClasses:\n" + ex.Message);
                }
            }

            return dt;
        }
    }
}
