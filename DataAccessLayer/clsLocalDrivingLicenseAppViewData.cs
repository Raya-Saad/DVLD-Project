using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public class clsLocalDrivingLicenseAppViewData
    {
        public static bool GetApplicationByID(int applicationID, ref string className, ref string nationalNo, ref string fullName, ref DateTime applicationDate, ref int passedTestCount, ref string status)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM LocalDrivingLicenseApplications_View WHERE LocalDrivingLicenseApplicationID = @ApplicationID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    className = Convert.ToString(reader["ClassName"]);
                    nationalNo = Convert.ToString(reader["NationalNo"]);
                    fullName = Convert.ToString(reader["FullName"]);
                    applicationDate = Convert.ToDateTime(reader["ApplicationDate"]);
                    passedTestCount = Convert.ToInt32(reader["PassedTestCount"]);
                    status = Convert.ToString(reader["Status"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error in GetApplicationByID:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return isFound;
        }

        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM LocalDrivingLicenseApplications_View";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetAllApplications:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static bool IsApplicationExists(int applicationID)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT 1 FROM LocalDrivingLicenseApplications_View WHERE LocalDrivingLicenseApplicationID = @ApplicationID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();

                isExist = (result != null);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error in IsApplicationExists:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return isExist;
        }

        //not used
        public static bool IsThereAlreadyNewApp(string ClassName, string NationalNo, string Status)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT IsThere = 1 FROM LocalDrivingLicenseApplications_View 
                    WHERE ClassName = @ClassName and NationalNo = @NationalNo and Status = @Status";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClassName", ClassName);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@Status", Status);


            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int _isFound))
                    isExist = true;
                else
                    isExist = false;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in IsThereAlreadyNewApp:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return isExist;
        }

        public static int GetApplicationByNationalNoInClassAndStatus(string ClassName, string NationalNo, string Status)
        {
            int ID = -1;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT LocalDrivingLicenseApplicationID FROM LocalDrivingLicenseApplications_View 
                    WHERE ClassName = @ClassName and NationalNo = @NationalNo and Status = @Status";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ClassName", ClassName);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@Status", Status);


            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int AppID))
                    ID = AppID;
                else
                    ID = -1;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetApplicationByNationalNoInClassAndStatus:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return ID;
        }
    }
}
