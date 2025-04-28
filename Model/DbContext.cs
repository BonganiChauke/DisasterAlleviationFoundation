using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DisasterAlleviationFoundation.Model
{
    public class DbContext
    {
        //connection string to azure database
        public static SqlConnection Connection = new SqlConnection(@"Server=tcp:appr-6312.database.windows.net,1433;Initial Catalog=database;Persist Security Info=False;User ID=st10061533;Password=BR.c11@ST533;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        //method to hash password
        public static string ComputeHash(string password)
        {
            //string builder to append string character
            StringBuilder stringBuilder = new StringBuilder();

            //sha256 to hash password
            using (SHA256 PS_HASH = SHA256.Create())
            {

                byte[] hashBytes = PS_HASH.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x3"));

                }
            }

            return stringBuilder.ToString();
        }//***************************************[E]

        //method to register users
        public static string Registration(string userFirstName, string userLastName, string email, string password, string role)
        {
            //return variables
            string register = "";

            //try catch for error handling
            try
            {
                //open connection to datatbase
                Connection.Open();

                //string query to insert to database
                string query = "INSERT INTO USERS (FIRST_NAME, LAST_NAME, EMAIL, PASSWORD, ROLE) VALUES (@FIRST_NAME, @LAST_NAME, @EMAIL, @PASSWORD, @ROLE)";

                //using sql command to insert to table
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //add user values
                    command.Parameters.AddWithValue("@FIRST_NAME", userFirstName);
                    command.Parameters.AddWithValue("@LAST_NAME", userLastName);
                    command.Parameters.AddWithValue("@EMAIL", email);
                    command.Parameters.AddWithValue("@PASSWORD", ComputeHash(password));
                    command.Parameters.AddWithValue("@ROLE", role);

                    //to check affected rows
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        register = "Successful";
                    }
                    else
                    {
                        register = "Unable to register at this time....";
                    }
                }
            }
            catch (SqlException ex)
            {
                //check for duplicate with primary 
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    if (ex.Message.Contains("unique"))
                    {
                        register = "Email already already exists... please login";
                    }
                    else
                    {
                        register = "Email already already exists... please login";
                    }
                }
                else
                {
                    //any other sql errors
                    register = ex.Message;
                }
            }
            catch (Exception e)
            {
                //assign error message
                register = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return register;
        }//***************************************[E]

        //method to login 
        public static string Login(string email, string password)
        {

            //try catch for error
            try
            {
                //open connection to database
                Connection.Open();

                //string query to select from the table
                string query = "Select ROLE, EMAIL from USERS where EMAIL='" + email + "' AND PASSWORD='" + ComputeHash(password) + "'";

                //sql command to execute the query and connection
                SqlCommand command = new SqlCommand(query, Connection);


                //sql reader to read from the table 
                SqlDataReader sqlData = command.ExecuteReader();

                //reading data
                if (sqlData.Read())
                {
                    //reading user roles
                    string role = sqlData["ROLE"].ToString();

                    //returning user role
                    return role;

                }
                else
                {
                    //return nothin if no role found
                    return null;

                }

            }
            catch (SqlException ex)
            {
                //return error message
                return ex.Message.ToString();
            }
            catch (Exception e)
            {
                //return error message
                return e.Message.ToString();
            }
            finally
            {
                //close connection once operation complete
                Connection.Close();
            }
        }//***************************************[E]

        //method to update password
        public static string ResetPassword(string email, string password)
        {
            //return variable
            string reset = "";

            //try catch for error handling
            try
            {
                //open connection to database
                Connection.Open();

                //string query to update password
                string query = "UPDATE USERS SET PASSWORD = @Password WHERE EMAIL = @Email";


                using (SqlCommand command = new SqlCommand(query, Connection))
                {

                    command.Parameters.AddWithValue("@Password", ComputeHash(password));
                    command.Parameters.AddWithValue("@Email", email);

                    // Execute the query and get the number of affected rows
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the operation was successful
                    if (rowsAffected > 0)
                    {
                        reset = "Success"; // Operation successful
                    }
                    else
                    {
                        reset = "Unable to reset password"; // No rows affected
                    }
                }
            }
            catch (Exception e)
            {
                reset = e.Message.ToString();
            }
            finally
            {
                //close connection once operation complete
                Connection.Close();
            }

            //return statement
            return reset;
        }//***************************************[E]

        //method to donate disaster 
        public static string ReportDisaster(string type, string location, string description, string date, byte[] image, byte[] video, string email)
        {
            //return variable
            string report = "";

            try
            {
                //open connection to database
                Connection.Open();

                //string qeury 
                string query = "Insert into DISASTER_REPORT (DISASTER_TYPE, LOCATION, DESCRIPTION, DATE, IMAGES, VIDEO, EMAIL) values (@DISASTER_TYPE, @LOCATION, @DESCRIPTION, @DATE, CONVERT(VARBINARY(MAX), @IMAGES), CONVERT(VARBINARY(MAX), @VIDEO), @EMAIL)";


                //using sql command to insert to table
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding values to database
                    command.Parameters.AddWithValue("@DISASTER_TYPE", type);
                    command.Parameters.AddWithValue("@LOCATION", location);
                    command.Parameters.AddWithValue("@DESCRIPTION", description);
                    command.Parameters.AddWithValue("@DATE", DateTime.Parse(date));
                    command.Parameters.AddWithValue("@IMAGES", (object)image ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VIDEO", (object)video ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EMAIL", email);

                    //int row count
                    int rowsAffected = command.ExecuteNonQuery();

                    //to check if the are number of rows affected
                    if (rowsAffected > 0)
                    {
                        //used to check if operation was successful
                        report = "Success";
                    }
                    else
                    {
                        //to alert
                        report = "Unable to report at this time";
                    }
                }

            }
            catch (SqlException e)
            {
                //error message
                report = e.Message.ToString();

            }
            catch (Exception e)
            {
                //error message
                report = e.Message.ToString();
            }
            finally
            {
                //close connection once operation complete
                Connection.Close();
            }

            //return statement
            return report;
        }//***************************************[E]

        //method to donate disaster 
        public static string Donate(string donationType, string donationAmount, string foodType, string paymentType, DateTime donated, string clothesNumber, string description, byte[] image, byte[] video, string email)
        {
            //return variable
            string donate = "";

            try
            {
                //open connection to database
                Connection.Open();

                //string query to insert to table
                string query = "Insert into DONATION (DONATION_TYPE, DONATION_AMOUNT, FOOD_TYPE, PAYMENT_TYPE, DONATED, CLOTHES_NUMBER, DESCRIPTION, IMAGE, VIDEO, EMAIL) values (@DONATION_TYPE, @DONATION_AMOUNT, @FOOD_TYPE, @PAYMENT_TYPE, @DONATED, @CLOTHES_NUMBER, @DESCRIPTION, CONVERT(VARBINARY(MAX), @IMAGE), CONVERT(VARBINARY(MAX), @VIDEO), @EMAIL)";

                //using sql command to insert to table
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding values to table
                    command.Parameters.AddWithValue("@DONATION_TYPE", donationType);
                    command.Parameters.AddWithValue("@DONATION_AMOUNT", donationAmount);
                    command.Parameters.AddWithValue("@FOOD_TYPE", foodType);
                    command.Parameters.AddWithValue("@PAYMENT_TYPE", paymentType);
                    command.Parameters.AddWithValue("@DONATED", donated);
                    command.Parameters.AddWithValue("@CLOTHES_NUMBER", clothesNumber);
                    command.Parameters.AddWithValue("@DESCRIPTION", description);
                    command.Parameters.AddWithValue("@IMAGE", (object)image ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VIDEO", (object)video ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EMAIL", email);

                    //int row
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //message
                        donate = "Success";
                    }
                    else
                    {
                        //message
                        donate = "Unable to donate this time";
                    }
                }

            }
            catch (SqlException e)
            {
                //error message
                donate = e.Message.ToString();

            }
            catch (Exception e)
            {
                //error message
                donate = e.Message.ToString();
            }
            finally
            {
                //close connection once operation complete
                Connection.Close();
            }

            //return statement
            return donate;
        }//***************************************[E]

        //method to process image and videos
        public static byte[] ProcessFileUpload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    return memoryStream.ToArray(); // Convert file to byte array
                }
            }
            return null; // Return null if no file is uploaded
        }//end

        //method to add tasks
        public static string Tasks(string TASK_NAME, string DESCRIPTION, string AREA_NAME, string STATUS, string EMAIL)
        {
            //temp variable
            string task = "";

            //try catch for error handling
            try
            {
                //open connection  
                Connection.Open();

                //string query 
                string query = "INSERT INTO TASKS (TASK_NAME, DESCRIPTION, AREA_NAME, STATUS, EMAIL) VALUES (@TASK_NAME, @DESCRIPTION, @AREA_NAME, @STATUS, @EMAIL)";

                //using sql command to insert to database
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding values 
                    command.Parameters.AddWithValue("@TASK_NAME", TASK_NAME);
                    command.Parameters.AddWithValue("@DESCRIPTION", DESCRIPTION);
                    command.Parameters.AddWithValue("@AREA_NAME", AREA_NAME);
                    command.Parameters.AddWithValue("@STATUS", STATUS);
                    command.Parameters.AddWithValue("@EMAIL", EMAIL);

                    //int to count rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        task = "Success";
                    }
                    else
                    {
                        task = "Unable to save task";
                    }
                }
            }
            catch (SqlException ex)
            {
                //check for duplicate with primary 
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    if (ex.Message.Contains("unique"))
                    {
                        task = "Task already exist";
                    }
                    else
                    {
                        task = "Task already exist";
                    }
                }
                else
                {
                    //any other sql errors
                    task = ex.Message;
                }
            }
            catch (Exception e)
            {
                //assign error message
                task = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return task;
        }//end

        //method to add schedules
        public static string Schedules(string SCHEDULE_AREA, string SCHEDULE_TIME, string SCHEDULE_DATE, string STATUS, string EMAIL)
        {
            //return variables
            string schedules = "";

            //try catch for error handling
            try
            {
                //open connection
                Connection.Open();

                //string query
                string query = "INSERT INTO SCHEDULES (SCHEDULE_AREA, SCHEDULE_TIME,SCHEDULE_DATE, STATUS, EMAIL) VALUES (@SCHEDULE_AREA, @SCHEDULE_TIME, @SCHEDULE_DATE, @STATUS, @EMAIL)";

                //using sql command to insert
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding values
                    command.Parameters.AddWithValue("@SCHEDULE_AREA", SCHEDULE_AREA);
                    command.Parameters.AddWithValue("@SCHEDULE_TIME", SCHEDULE_TIME);
                    command.Parameters.AddWithValue("@SCHEDULE_DATE", SCHEDULE_DATE);
                    command.Parameters.AddWithValue("@STATUS", STATUS);
                    command.Parameters.AddWithValue("@EMAIL", EMAIL);

                    //to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        schedules = "Success";
                    }
                    else
                    {
                        //assign return value
                        schedules = "Unable to save task";
                    }
                }

            }
            catch (SqlException ex)
            {
                //any other sql errors
                schedules = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                schedules = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return schedules;

        }//END

        //method to assign a volunteer to a task
        public static string AssignVolunteer(string email, string taskName)
        {
            //temp variables
            string assign = "";

            //try catch for error handling 
            try
            {
                //open connection
                Connection.Open();

                //----------------
                //string query to update email
                string query = "UPDATE TASKS SET EMAIL = @EMAIL WHERE TASK_NAME = @TASK_NAME";

                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding values 
                    command.Parameters.AddWithValue("@EMAIL", email);
                    command.Parameters.AddWithValue("@TASK_NAME", taskName);

                    //to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        assign = "Success";
                    }
                    else
                    {
                        //assign return value
                        assign = "Unable to assign volunteer";
                    }
                }

            }
            catch (SqlException ex)
            {
                //any other sql errors
                assign = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                assign = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return message
            return assign;
        }//end

        //method assign schedules
        public static string AssignSchedules(string email, string scheduleArea)
        {
            //temp variables
            string assign = "";

            //try catch for error handling 
            try
            {
                //open connection
                Connection.Open();

                //string query = "UPDATE TASKS SET EMAIL = @EMAIL WHERE TASK_NAME = @TASK_NAME";

                //string query 
                string query = "UPDATE SCHEDULES SET EMAIL = @EMAIL WHERE SCHEDULE_AREA = @SCHEDULE_AREA";

                //using sql command
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding values
                    command.Parameters.AddWithValue("@EMAIL", email);
                    command.Parameters.AddWithValue("@SCHEDULE_AREA", scheduleArea);

                    //to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        assign = "Success";
                    }
                    else
                    {
                        //assign return value
                        assign = "Unable to assign volunteer";
                    }

                }
            }
            catch (SqlException ex)
            {
                //any other sql errors
                assign = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                assign = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return message
            return assign;

        }//end

        //method to send messages
        public static string SendMessages(string email, string description)
        {
            //temp variable
            string messages = "";

            //try catch for error handling 
            try
            {
                //open connection
                Connection.Open();

                //string query 
                string query = "INSERT INTO MESSAGE_VOLUNTEER (EMAIL, DESCRIPTION) VALUES (@EMAIL, @DESCRIPTION)";

                //using sql command
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding with values 
                    command.Parameters.AddWithValue("@EMAIL", email);
                    command.Parameters.AddWithValue("@DESCRIPTION", description);

                    //to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        messages = "Success";
                    }
                    else
                    {
                        //assign return value
                        messages = "Unable to send message to volunteer";
                    }
                }
            }
            catch (SqlException ex)
            {
                //any other sql errors
                messages = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                messages = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return message
            return messages;
        }//end

        //method to select all volunteer emails
        public static void VolunteerEmails(List<string> email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //user roles
                string role = "Volunteer";

                //query 
                string query = "SELECT * FROM USERS WHERE ROLE ='" + role + "';";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding email
                    email.Add(table.Rows[i]["EMAIL"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to all task name
        public static void TaskNames(List<string> taskNames)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT TASK_NAME FROM TASKS";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding email
                    taskNames.Add(table.Rows[i]["TASK_NAME"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to all task name
        public static void TaskAreas(List<string> taskArea)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT AREA_NAME FROM TASKS";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding email
                    taskArea.Add(table.Rows[i]["AREA_NAME"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to view Donations 
        public static void DonationReport(List<string> ID, List<string> donationType, List<string> donationAmount, List<string> foodType, List<string> paymentType, List<string> donated, List<string> clothesNumber, List<string> description, List<string> email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();


                //query 
                string query = "SELECT DONATION_ID, DONATION_TYPE, DONATION_AMOUNT, FOOD_TYPE, PAYMENT_TYPE, DONATED, CLOTHES_NUMBER, DESCRIPTION, EMAIL  FROM DONATION";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    ID.Add(table.Rows[i]["DONATION_ID"].ToString());
                    donationType.Add(table.Rows[i]["DONATION_TYPE"].ToString());
                    donationAmount.Add(table.Rows[i]["DONATION_AMOUNT"].ToString());
                    foodType.Add(table.Rows[i]["FOOD_TYPE"].ToString());
                    paymentType.Add(table.Rows[i]["PAYMENT_TYPE"].ToString());
                    donated.Add(table.Rows[i]["DONATED"].ToString());
                    clothesNumber.Add(table.Rows[i]["CLOTHES_NUMBER"].ToString());
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    email.Add(table.Rows[i]["EMAIL"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }

        }//end 

        //method to view disaster reports
        public static void DisaterReport(List<string> ID, List<string> disasterType, List<string> location, List<string> description, List<string> date, List<string> email)
        {

            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT DISASTER_REPORT_ID, DISASTER_TYPE, LOCATION, DESCRIPTION, DATE, EMAIL FROM DISASTER_REPORT";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    //DISASTER_REPORT_ID, DISASTER_TYPE, LOCATION, DESCRIPTION, DATE
                    ID.Add(table.Rows[i]["DISASTER_REPORT_ID"].ToString());
                    disasterType.Add(table.Rows[i]["DISASTER_TYPE"].ToString());
                    location.Add(table.Rows[i]["LOCATION"].ToString());
                    date.Add(table.Rows[i]["DATE"].ToString());
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    email.Add(table.Rows[i]["EMAIL"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to view all messages
        public static void VolunteerMessages(List<string> description, string email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT DESCRIPTION from MESSAGE_VOLUNTEER WHERE email ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to view volunteer task
        public static void VolunteerTask(List<string> taskName, List<string> description, List<string> areaName, List<string> status, string email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT TASK_NAME, DESCRIPTION, AREA_NAME, STATUS from TASKS WHERE email ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    taskName.Add(table.Rows[i]["TASK_NAME"].ToString());
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    areaName.Add(table.Rows[i]["AREA_NAME"].ToString());
                    status.Add(table.Rows[i]["STATUS"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to display for volunteer
        public static void VolunteerSchedules(List<string> scheduleArea, List<string> scheduleTime, List<string> scheduleDate, List<string> status, string email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();


                //query 
                string query = "SELECT SCHEDULE_AREA, SCHEDULE_TIME, SCHEDULE_DATE, STATUS from SCHEDULES WHERE email ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    scheduleArea.Add(table.Rows[i]["SCHEDULE_AREA"].ToString());
                    scheduleTime.Add(table.Rows[i]["SCHEDULE_TIME"].ToString());
                    scheduleDate.Add(table.Rows[i]["SCHEDULE_DATE"].ToString());
                    status.Add(table.Rows[i]["STATUS"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end


        //method to display all tasks table
        public static void VolunteersTask(List<string> taskID, List<string> taskName, List<string> description, List<string> areaName, List<string> status, List<string> email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT TASK_ID, TASK_NAME, DESCRIPTION, AREA_NAME, STATUS, EMAIL  from TASKS";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    taskID.Add(table.Rows[i]["TASK_ID"].ToString());
                    taskName.Add(table.Rows[i]["TASK_NAME"].ToString());
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    areaName.Add(table.Rows[i]["AREA_NAME"].ToString());
                    status.Add(table.Rows[i]["STATUS"].ToString());
                    email.Add(table.Rows[i]["EMAIL"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }

        //method to display all schedule table
        public static void VolunteersSchedules(List<string> scheduleID, List<string> scheduleArea, List<string> scheduleTime, List<string> scheduleDate, List<string> status, List<string> email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT SCHEDULE_ID, SCHEDULE_AREA, SCHEDULE_TIME, SCHEDULE_DATE, STATUS, EMAIL from SCHEDULES";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    scheduleID.Add(table.Rows[i]["SCHEDULE_ID"].ToString());
                    scheduleArea.Add(table.Rows[i]["SCHEDULE_AREA"].ToString());
                    scheduleTime.Add(table.Rows[i]["SCHEDULE_TIME"].ToString());
                    scheduleDate.Add(table.Rows[i]["SCHEDULE_DATE"].ToString());
                    status.Add(table.Rows[i]["STATUS"].ToString());
                    email.Add(table.Rows[i]["EMAIL"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to diplay user names
        public static void fullNames(List<string> name, List<string> lastName, string email)
        {
            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT FIRST_NAME, LAST_NAME FROM USERS WHERE EMAIL ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    name.Add(table.Rows[i]["FIRST_NAME"].ToString());
                    lastName.Add(table.Rows[i]["LAST_NAME"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to send emer
        public static string Emergencies(string location, string description, string email)
        {
            //temp variable
            string messages = "";

            //try catch for error handling 
            try
            {
                //open connection
                Connection.Open();

                //string query 
                string query = "INSERT INTO Emergenciess (LOCATION, DESCRIPTION, EMAIL) VALUES (@LOCATION, @DESCRIPTION, @EMAIL)";

                //using sql command
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding with values 
                    command.Parameters.AddWithValue("@LOCATION", location);
                    command.Parameters.AddWithValue("@DESCRIPTION", description);
                    command.Parameters.AddWithValue("@EMAIL", email);

                    //to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        messages = "Success";
                    }
                    else
                    {
                        //assign return value
                        messages = "Unable to send message to volunteer";
                    }
                }
            }
            catch (SqlException ex)
            {
                //any other sql errors
                messages = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                messages = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return messages;

        }//end

        //method to send feedback
        public static string FeedBack(string title, string description, string email)
        {

            //temp variable
            string messages = "";

            //try catch for error handling 
            try
            {
                //open connection
                Connection.Open();

                //string query 
                string query = "INSERT INTO feedBacks (TITLE, DESCRIPTION, EMAIL) VALUES (@TITLE, @DESCRIPTION, @EMAIL)";

                //using sql command
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding with values 
                    command.Parameters.AddWithValue("@TITLE", title);
                    command.Parameters.AddWithValue("@DESCRIPTION", description);
                    command.Parameters.AddWithValue("@EMAIL", email);

                    //to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if to check number of rows affected
                    if (rowsAffected > 0)
                    {
                        //assign return value
                        messages = "Success";
                    }
                    else
                    {
                        //assign return value
                        messages = "Unable to send message to volunteer";
                    }
                }
            }
            catch (SqlException ex)
            {
                //any other sql errors
                messages = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                messages = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return messages;
        }//end

        //method to display users donation
        public static void UserDonation(List<string> ID, List<string> donationType, List<string> donationAmount, List<string> foodType, List<string> paymentType, List<string> donated, List<string> clothesNumber, List<string> description, string email)
        {
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();


                //query 
                string query = "SELECT DONATION_ID, DONATION_TYPE, DONATION_AMOUNT, FOOD_TYPE, PAYMENT_TYPE, DONATED, CLOTHES_NUMBER, DESCRIPTION FROM DONATION WHERE EMAIL ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    ID.Add(table.Rows[i]["DONATION_ID"].ToString());
                    donationType.Add(table.Rows[i]["DONATION_TYPE"].ToString());
                    donationAmount.Add(table.Rows[i]["DONATION_AMOUNT"].ToString());
                    foodType.Add(table.Rows[i]["FOOD_TYPE"].ToString());
                    paymentType.Add(table.Rows[i]["PAYMENT_TYPE"].ToString());
                    donated.Add(table.Rows[i]["DONATED"].ToString());
                    clothesNumber.Add(table.Rows[i]["CLOTHES_NUMBER"].ToString());
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //memthod to display all user reports
        public static void UsersReport(List<string> ID, List<string> disasterType, List<string> location, List<string> description, List<string> date, string email)
        {

            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();

                //query 
                string query = "SELECT DISASTER_REPORT_ID, DISASTER_TYPE, LOCATION, DESCRIPTION, DATE FROM DISASTER_REPORT WHERE EMAIL ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters
                    //DISASTER_REPORT_ID, DISASTER_TYPE, LOCATION, DESCRIPTION, DATE
                    ID.Add(table.Rows[i]["DISASTER_REPORT_ID"].ToString());
                    disasterType.Add(table.Rows[i]["DISASTER_TYPE"].ToString());
                    location.Add(table.Rows[i]["LOCATION"].ToString());
                    date.Add(table.Rows[i]["DATE"].ToString());
                    description.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method to emergency reports
        public static void UsersEmergency(List<string> emergencyID, List<string> emergencyLocation, List<string> emergencyDescription, string email)
        {

            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();


                //query 
                string query = "SELECT Emergencies_ID, LOCATION, DESCRIPTION FROM Emergenciess WHERE EMAIL ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters

                    emergencyID.Add(table.Rows[i]["Emergencies_ID"].ToString());
                    emergencyLocation.Add(table.Rows[i]["LOCATION"].ToString());
                    emergencyDescription.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end


        //method for feedbacks
        public static void UsersFeedback(List<string> feedbackID, List<string> feedBackTitle, List<string> feedBackDescription, string email)
        {

            //
            string message = "";

            //try catch for error handling
            try
            {
                //open connection 
                Connection.Open();



                //query 
                string query = "SELECT FEEDBACK_ID, TITLE, DESCRIPTION FROM feedBacks WHERE EMAIL ='" + email + "'";

                //sql adapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Connection);

                //datatable
                DataTable table = new DataTable();

                //fill table
                dataAdapter.Fill(table);

                //data reader
                SqlCommand read = new SqlCommand(query, Connection);

                //sql data reader
                SqlDataReader dataReader = read.ExecuteReader();

                //while loop to count the values
                int i = 0;
                while (dataReader.Read())
                {
                    //adding values to list parameters

                    feedbackID.Add(table.Rows[i]["FEEDBACK_ID"].ToString());
                    feedBackTitle.Add(table.Rows[i]["TITLE"].ToString());
                    feedBackDescription.Add(table.Rows[i]["DESCRIPTION"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                //close conection
                Connection.Close();
            }
        }//end

        //method for subscribers
        public static string Subscribers(string email)
        {
            //return variable
            string message = "";

            //try catch for error handling
            try
            {
                //open connection
                Connection.Open();

                //string query
                string query = "Insert into Subscribers (EMAIL) values (@EMAIL)";

                //using sql command to insert
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding value
                    command.Parameters.AddWithValue("@EMAIL", email);

                    //int to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if ro check
                    if (rowsAffected > 0)
                    {
                        //alert the user
                        message = "Success";
                    }
                    else
                    {
                        //alert the user
                        message = "Unable to subscribe";
                    }
                }
            }
            catch (SqlException ex)
            {
                //check for duplicate with primary 
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    if (ex.Message.Contains("unique"))
                    {
                        message = "Email already exist";
                    }
                    else
                    {
                        message = "Email already exist";
                    }
                }
                else
                {
                    //any other sql errors
                    message = ex.Message;
                }
            }
            catch (Exception e)
            {
                //assign error message
                message = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return message;
        }//end

        //method for contact form
        public static string ContactForm(string firstName, string lastName, string email, string phoneNumber, string description)
        {
            //return variable
            string message = "";

            //try catch for error handling
            try
            {
                //open connection
                Connection.Open();

                //string query
                string query = "Insert into ContactForm (FIRSTNAME,LASTNAME,EMAIL,PHONE_NUMBER,MESSAGE) values (@FIRSTNAME,@LASTNAME,@EMAIL,@PHONE_NUMBER,@MESSAGE)";

                //using sql command to insert
                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    //adding value
                    command.Parameters.AddWithValue("@FIRSTNAME", firstName);
                    command.Parameters.AddWithValue("@LASTNAME", lastName);
                    command.Parameters.AddWithValue("@EMAIL", email);
                    command.Parameters.AddWithValue("@PHONE_NUMBER", phoneNumber);
                    command.Parameters.AddWithValue("@MESSAGE", description);

                    //int to check rows affected
                    int rowsAffected = command.ExecuteNonQuery();

                    //if ro check
                    if (rowsAffected > 0)
                    {
                        //alert the user
                        message = "Success";
                    }
                    else
                    {
                        //alert the user
                        message = "Unable to submit contact form at this time";
                    }
                }
            }
            catch (SqlException ex)
            {
                //any other sql errors
                message = ex.Message;

            }
            catch (Exception e)
            {
                //assign error message
                message = e.Message.ToString();
            }
            finally
            {
                //close connection once operation completed
                Connection.Close();
            }

            //return statement
            return message;
        }//end
    }
}
