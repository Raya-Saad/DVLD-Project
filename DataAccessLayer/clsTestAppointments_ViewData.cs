using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class clsTestAppointments_ViewData
    {
        public static bool GetAppointmentByID(int testAppointmentID, ref int localDrivingLicenseApplicationID,
            ref string testTypeTitle, ref string className, ref DateTime appointmentDate,
            ref double paidFees, ref string fullName, ref bool isLocked)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments_View WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    localDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    testTypeTitle = Convert.ToString(reader["TestTypeTitle"]);
                    className = Convert.ToString(reader["ClassName"]);
                    appointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    paidFees = Convert.ToDouble(reader["PaidFees"]);
                    fullName = Convert.ToString(reader["FullName"]);
                    isLocked = Convert.ToBoolean(reader["IsLocked"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("Error in GetAppointmentByID:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return isFound;
        }

        public static DataTable GetAllAppointments()
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments_View";

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
               // MessageBox.Show("Error in GetAllAppointments:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static bool IsAppointmentExists(int testAppointmentID)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT 1 FROM TestAppointments_View WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();

                isExist = (result != null);
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in IsAppointmentExists:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return isExist;
        }

    }
}
