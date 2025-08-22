using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class clsTestAppointmentsData
    {
        public static bool GetTestAppointmentByID(int testAppointmentID, ref int testTypeID, ref int localAppID, ref DateTime appointmentDate, ref float paidFees, ref int createdByUserID, ref bool isLocked)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @ID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID", testAppointmentID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    testTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    localAppID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    appointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isLocked = Convert.ToBoolean(reader["IsLocked"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetTestAppointmentByID:\n" + ex.Message);
            }
            finally { conn.Close(); }

            return isFound;
        }

        public static bool GetTestAppointmentByLocalAppID(int localAppID, ref int testAppointmentID, ref int testTypeID, ref DateTime appointmentDate, ref float paidFees, ref int createdByUserID, ref bool isLocked)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments WHERE LocalDrivingLicenseApplicationID  = @LocalAppID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@LocalAppID", localAppID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    testAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    testTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    appointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isLocked = Convert.ToBoolean(reader["IsLocked"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error in GetTestAppointmentByLocalAppID:\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isFound;
        }

        public static bool GetTestAppointmentByLocalAppIDAndTestType(int localAppID, int testTypeID, ref int testAppointmentID, ref DateTime appointmentDate, ref float paidFees, ref int createdByUserID, ref bool isLocked)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments "+
                "WHERE LocalDrivingLicenseApplicationID = @LocalAppID and TestTypeID = @TestTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@LocalAppID", localAppID);
            cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    testAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    appointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isLocked = Convert.ToBoolean(reader["IsLocked"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetTestAppointmentByLocalAppIDAndTestType:\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isFound;
        } 

        public static int AddNewTestAppointment(int testTypeID, int localAppID, DateTime appointmentDate, float paidFees, int createdByUserID, bool isLocked)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"INSERT INTO TestAppointments 
                            (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked) 
                             VALUES (@TestTypeID, @LocalAppID, @Date, @Fees, @UserID, @IsLocked);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);
            cmd.Parameters.AddWithValue("@LocalAppID", localAppID);
            cmd.Parameters.AddWithValue("@Date", appointmentDate);
            cmd.Parameters.AddWithValue("@Fees", paidFees);
            cmd.Parameters.AddWithValue("@UserID", createdByUserID);
            cmd.Parameters.AddWithValue("@IsLocked", isLocked);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int newID))
                    id = newID;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in AddNewTestAppointment:\n" + ex.Message);
            }
            finally { conn.Close(); }

            return id;
        }

        public static bool UpdateTestAppointment(int id, int testTypeID, int localAppID, DateTime appointmentDate, float paidFees, int createdByUserID, bool isLocked)
        {
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE TestAppointments SET 
                             TestTypeID = @TestTypeID,
                             LocalDrivingLicenseApplicationID = @LocalAppID,
                             AppointmentDate = @Date,
                             PaidFees = @Fees,
                             CreatedByUserID = @UserID,
                             IsLocked = @IsLocked
                             WHERE TestAppointmentID = @ID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);
            cmd.Parameters.AddWithValue("@LocalAppID", localAppID);
            cmd.Parameters.AddWithValue("@Date", appointmentDate);
            cmd.Parameters.AddWithValue("@Fees", paidFees);
            cmd.Parameters.AddWithValue("@UserID", createdByUserID);
            cmd.Parameters.AddWithValue("@IsLocked", isLocked);

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("Error in UpdateTestAppointment:\n" + ex.Message);
                return false;
            }
            finally { conn.Close(); }
        }

        public static bool DeleteTestAppointment(int id)
        {
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "DELETE FROM TestAppointments WHERE TestAppointmentID = @ID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID", id);

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in DeleteTestAppointment:\n" + ex.Message);
                return false;
            }
            finally { conn.Close(); }
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments";

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
               // MessageBox.Show("Error in GetAllTestAppointments:\n" + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static DataTable GetAllTestAppointmentsWithApplicationIDAndType(int localAppID, int testTypeID)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM TestAppointments 
                       WHERE LocalDrivingLicenseApplicationID = @LocalAppID and TestTypeID = @TestTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@LocalAppID", localAppID);
            cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);

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
               // MessageBox.Show("Error in GetAllTestAppointmentsWithApplicationAndType:\n" + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static bool IsTestAppointmentExists(int id)
        {
            bool exists = false;
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT 1 FROM TestAppointments WHERE TestAppointmentID = @ID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID", id);

            try
            {
                conn.Open();
                exists = cmd.ExecuteScalar() != null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error in IsTestAppointmentExists:\n" + ex.Message);
            }
            finally { conn.Close(); }

            return exists;
        }



        public static int GetTrials(int testTypeID, int LDLAppID)
        {
            int trials = -1;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT count(TestAppointmentID) FROM TestAppointments " +
                "WHERE TestTypeID = @TestTypeID and LocalDrivingLicenseApplicationID = @LDLAppID and IsLocked = 1";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);
            cmd.Parameters.AddWithValue("@LDLAppID", LDLAppID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int Count))
                    trials = Count;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in GetTrialNum:\n" + ex.Message);
            }
            finally { conn.Close(); }

            return trials;
        }
    }
}
