using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace DataAccessLayer
{
    public class clsApplicationTypesData
    {
        public static bool GetApplicationTypeByID(int ApplicationTypeID,
            ref string ApplicationTypeTitle, ref double ApplicationFees)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationTypeTitle = Convert.ToString(reader["ApplicationTypeTitle"]);
                    ApplicationFees = Convert.ToDouble(reader["ApplicationFees"]);

                    reader.Close();
                }
                else
                {
                    isFound = false;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                //MessageBox.Show("Error in GetApplicationTypeByID:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return isFound;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, double ApplicationFees)
        {
            int rowsAffected = 0;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                UPDATE ApplicationTypes SET 
                    ApplicationTypeTitle = @ApplicationTypeTitle,
                    ApplicationFees = @ApplicationFees
                    WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            cmd.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            cmd.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

            try
            {
                conn.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in UpdateApplicationType:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return rowsAffected > 0;
        }

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM ApplicationTypes";

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
            catch
            {
               // MessageBox.Show("Error in UpdateApplicationType:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

    }
}
