using DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class clsApplicationsData
{
    public static bool GetApplicationByID(int applicationID, ref int applicantPersonID, ref DateTime applicationDate, ref int applicationTypeID, ref byte applicationStatus, ref DateTime lastStatusDate, ref decimal paidFees, ref int createdByUserID)
    {
        bool isFound = false;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
        string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;
                applicantPersonID = Convert.ToInt32(reader["ApplicantPersonID"]);
                applicationDate = Convert.ToDateTime(reader["ApplicationDate"]);
                applicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                applicationStatus = Convert.ToByte(reader["ApplicationStatus"]);
                lastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                paidFees = Convert.ToDecimal(reader["PaidFees"]);
                createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
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

    public static bool GetApplicationByPersonID(int applicantPersonID, ref int applicationID, ref DateTime applicationDate, ref int applicationTypeID, ref byte applicationStatus, ref DateTime lastStatusDate, ref decimal paidFees, ref int createdByUserID)
    {
        bool isFound = false;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
        string query = @"
        SELECT TOP 1 * FROM Applications
        WHERE ApplicantPersonID = @PersonID
        ORDER BY ApplicationDate DESC";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@PersonID", applicantPersonID);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;

                applicationID = Convert.ToInt32(reader["ApplicationID"]);
                applicationDate = Convert.ToDateTime(reader["ApplicationDate"]);
                applicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                applicationStatus = Convert.ToByte(reader["ApplicationStatus"]);
                lastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                paidFees = Convert.ToDecimal(reader["PaidFees"]);
                createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Error in GetApplicationByPersonID:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            conn.Close();
        }

        return isFound;
    }

    public static int AddNewApplication(int applicantPersonID, DateTime applicationDate, int applicationTypeID, byte applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
    {
        int applicationID = -1;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

        string query = @"
            INSERT INTO Applications
                (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
            VALUES
                (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
            SELECT SCOPE_IDENTITY();";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
        cmd.Parameters.AddWithValue("@ApplicationDate", applicationDate);
        cmd.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
        cmd.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
        cmd.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
        cmd.Parameters.AddWithValue("@PaidFees", paidFees);
        cmd.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

        try
        {
            conn.Open();
            object result = cmd.ExecuteScalar();

            if (result != null && int.TryParse(result.ToString(), out int id))
                applicationID = id;
        }
        catch (Exception ex)
        {
           // MessageBox.Show("Error in AddNewApplication:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            conn.Close();
        }

        return applicationID;
    }

    public static bool UpdateApplication(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID, byte applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
    {
        int rowsAffected = 0;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

        string query = @"
            UPDATE Applications SET
                ApplicantPersonID = @ApplicantPersonID,
                ApplicationDate = @ApplicationDate,
                ApplicationTypeID = @ApplicationTypeID,
                ApplicationStatus = @ApplicationStatus,
                LastStatusDate = @LastStatusDate,
                PaidFees = @PaidFees,
                CreatedByUserID = @CreatedByUserID
            WHERE
                ApplicationID = @ApplicationID";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
        cmd.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
        cmd.Parameters.AddWithValue("@ApplicationDate", applicationDate);
        cmd.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
        cmd.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
        cmd.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
        cmd.Parameters.AddWithValue("@PaidFees", paidFees);
        cmd.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

        try
        {
            conn.Open();
            rowsAffected = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Error in UpdateApplication:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            conn.Close();
        }

        return rowsAffected > 0;
    }

    public static bool DeleteApplication(int applicationID)
    {
        int rowsAffected = 0;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
        string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);

        try
        {
            conn.Open();
            rowsAffected = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
           // MessageBox.Show("Error in DeleteApplication:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            conn.Close();
        }

        return rowsAffected > 0;
    }

    public static bool IsApplicationExists(int applicationID)
    {
        bool exists = false;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
        string query = "SELECT 1 FROM Applications WHERE ApplicationID = @ApplicationID";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ApplicationID", applicationID);

        try
        {
            conn.Open();
            object result = cmd.ExecuteScalar();
            exists = result != null;
        }
        catch (Exception ex)
        {
           // MessageBox.Show("Error in IsApplicationExists:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            conn.Close();
        }

        return exists;
    }

    public static DataTable GetAllApplications()
    {
        DataTable dt = new DataTable();
        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
        string query = "SELECT * FROM Applications";

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





    //not used
    public static bool GetApplicationByPersonIDInStatusWithType(int PersonID, byte Status, int Type, ref int applicationID, ref DateTime applicationDate, ref DateTime lastStatusDate, ref decimal paidFees,
        ref int createdByUserID)
    {
        bool isFound = false;

        SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);
        string query = @"select * from Applications 
                        where ApplicantPersonID = @PersonID and ApplicationStatus = @Status 
                        and ApplicationTypeID = @Type;";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@PersonID", PersonID);
        cmd.Parameters.AddWithValue("@Status", Status);
        cmd.Parameters.AddWithValue("@Type", Type);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                isFound = true;

                applicationID = Convert.ToInt32(reader["ApplicationID"]);
                applicationDate = Convert.ToDateTime(reader["ApplicationDate"]);
                lastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                paidFees = Convert.ToDecimal(reader["PaidFees"]);
                createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
            }

            reader.Close();
        }
        catch (Exception ex)
        {
           // MessageBox.Show("Error in GetApplicationByPersonIDWithStatus:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            conn.Close();
        }

        return isFound;
    }

}
