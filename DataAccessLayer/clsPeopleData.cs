using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class clsPeopleData
    {
        public static bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {

            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People where PersonID = @personID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@personID", PersonID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    NationalNo = Convert.ToString(reader["NationalNo"]);
                    FirstName = Convert.ToString(reader["FirstName"]);
                    SecondName = Convert.ToString(reader["SecondName"]);
                    LastName = Convert.ToString(reader["LastName"]);
                    DateOfBirth = Convert.ToDateTime(reader["dateOfBirth"]);
                    Gendor = Convert.ToInt16(reader["Gendor"]);
                    Address = Convert.ToString(reader["Address"]);
                    Phone = Convert.ToString(reader["Phone"]);
                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);

                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = Convert.ToString(reader["ThirdName"]);
                    else
                        ThirdName = "";

                    if (reader["Email"] != DBNull.Value)
                        Email = Convert.ToString(reader["Email"]);
                    else
                        Email = "";

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = Convert.ToString(reader["ImagePath"]);
                    else
                        ImagePath = "";

                    reader.Close();
                }

                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;

                //MessageBox.Show("Error in GetPersonByID:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {

            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People where NationalNo = @nationalNo";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nationalNo", NationalNo);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = Convert.ToInt32(reader["PersonID"]);
                    FirstName = Convert.ToString(reader["FirstName"]);
                    SecondName = Convert.ToString(reader["SecondName"]);
                    LastName = Convert.ToString(reader["LastName"]);
                    DateOfBirth = Convert.ToDateTime(reader["dateOfBirth"]);
                    Gendor = Convert.ToInt16(reader["Gendor"]);
                    Address = Convert.ToString(reader["Address"]);
                    Phone = Convert.ToString(reader["Phone"]);
                    NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);

                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = Convert.ToString(reader["ThirdName"]);
                    else
                        ThirdName = "";

                    if (reader["Email"] != DBNull.Value)
                        Email = Convert.ToString(reader["Email"]);
                    else
                        Email = "";

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = Convert.ToString(reader["ImagePath"]);
                    else
                        ImagePath = "";

                    reader.Close();
                }

                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
              //  MessageBox.Show("Error in GetPersonByNationalNo:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return isFound;
        }

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                           INSERT INTO [dbo].[People]
                                  ([NationalNo]
                                  ,[FirstName]
                                  ,[SecondName]
                                  ,[ThirdName]
                                  ,[LastName]
                                  ,[DateOfBirth]
                                  ,[Gendor]
                                  ,[Address]
                                  ,[Phone]
                                  ,[Email]
                                  ,[NationalityCountryID]
                                  ,[ImagePath])
                            VALUES
                                  (@NationalNo
                                  ,@FirstName
                                  ,@SecondName
                                  ,@ThirdName
                                  ,@LastName
                                  ,@DateOfBirth
                                  ,@Gendor
                                  ,@Address
                                  ,@Phone
                                  ,@Email
                                  ,@NationalityCountryID
                                  ,@ImagePath)"
                            + "SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            cmd.Parameters.AddWithValue("@Gendor", Gendor);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ThirdName != "")
                cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                cmd.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != "")
                cmd.Parameters.AddWithValue("@Email", Email);
            else
                cmd.Parameters.AddWithValue("@Email", System.DBNull.Value);

            if (ImagePath != "")
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                cmd.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    PersonID = id;
                }
            }
            catch 
            {
              //  MessageBox.Show("Error in AddNewPerson:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int rawsAffected = 0;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE People SET 
                         NationalNo = @NationalNo
                        ,FirstName = @FirstName
                        ,SecondName = @SecondName
                        ,ThirdName = @ThirdName
                        ,LastName = @LastName
                        ,DateOfBirth = @DateOfBirth
                        ,Gendor = @Gendor
                        ,Address = @Address
                        ,Phone = @Phone
                        ,Email = @Email
                        ,NationalityCountryID = @NationalityCountryID
                        ,ImagePath = @ImagePath
                    WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            cmd.Parameters.AddWithValue("@Gendor", Gendor);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ThirdName != "")
                cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                cmd.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != "")
                cmd.Parameters.AddWithValue("@Email", Email);
            else
                cmd.Parameters.AddWithValue("@Email", System.DBNull.Value);

            if (ImagePath != "")
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                cmd.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                conn.Open();

                rawsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in UpdatePerson:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return rawsAffected > 0;

        }
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select 
	                            People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, 
	                            People.LastName, People.DateOfBirth, People.Gendor, 
		                    case 
		                        when People.Gendor = 0 then 'Male'
		                            else 'Female'
		                            end as GendorCaption ,
	                            People.Address, People.Phone, People.Email, People.NationalityCountryID, Countries.CountryName, People.ImagePath
                            from
	                            People inner join Countries on People.NationalityCountryID = Countries.CountryID
                            order by People.FirstName;";

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
               // MessageBox.Show("Error in GetAllPeople:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static bool DeletePerson(int PersonID)
        {
            int rawsAffected = 0;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE FROM People WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();

                rawsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error in DeletePerson Due To Data Connected To It!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return rawsAffected > 0;
        }

        public static bool IsPersonExists(int PersonID)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT IsThere = 1 FROM People WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if(result != null && int.TryParse(result.ToString(), out int _isFound))
                    isExist = true;       
                else
                    isExist = false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error in IsPersonExists:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return isExist;
        }

        public static bool IsPersonExists(string NationalNo)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT IsThere = 1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

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
               // MessageBox.Show("Error in IsPersonExists:\n", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }

            return isExist;
        }
    }
}
