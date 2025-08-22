using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class clsDrivers_ViewData
    {
        public static bool GetDriverByID(int DriverID, ref string FullName, ref string NationalNo, ref DateTime CreatedDate, ref string CreatedByUserName)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Drivers_View WHERE DriverID = @DriverID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FullName = reader["FullName"].ToString();
                    NationalNo = reader["NationalNo"].ToString();
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    CreatedByUserName = reader["CreatedByUserName"].ToString();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetDriverByID:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return isFound;
        }

        public static bool IsDriverExists(int DriverID)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT 1 FROM Drivers_View WHERE DriverID = @DriverID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                isExist = result != null;
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("Error in IsDriverExists:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return isExist;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Drivers_View";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetAllDrivers:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
    }
}
