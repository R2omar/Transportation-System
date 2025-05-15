using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Trasnportation_System;

namespace UserInfo
{
    class User
    {
        public int id {  get; set; }
        public string firstName {get;set;}

        public string lastName {get;set;}
        public string email { get; set; }
        public string defaultPaymentMethod { get; set; }
        public string password { get; set; }

        public List<string> phones = new List<string>();



        public User(int id,string firstName,string lastName,string email,string defaultPaymentMethod, string password)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.defaultPaymentMethod = defaultPaymentMethod;
            this.password = password;
        }
    }
    class UserValidation
    {
       public static bool ValidatePhone(string phone)
        {
            return Regex.IsMatch(phone, @"^(011|012|010|015)\d{8}$");
        }
        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@gmail\.com$");
        }


    }

    class UserOperations
    {
        public static void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("--- Register ---");

            Console.Write("Enter Your First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Your Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Your Email: ");
            string email = Console.ReadLine();

            if (!UserValidation.ValidateEmail(email))
            {
                Console.WriteLine("Invalid Email (Must Ends With \"@gmail.com\")");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;
            };

            Console.Write("Enter Your Default Payment Method: ");
            string paymentMethod = Console.ReadLine();

            Console.Write("Enter Your Password: ");
            string password = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                conn.Open();


                string checkQuery = "SELECT COUNT(*) FROM [USER] WHERE EMAIL = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                int exists = (int)checkCmd.ExecuteScalar();
                if (exists > 0)
                {
                    Console.WriteLine("Email already registered. Press Enter To Return...");
                    Console.ReadLine();
                    return;
                }


                string insertQuery = @"INSERT INTO [USER] (FIRSTNAME, LASTNAME, EMAIL, DEFAULTPAYMENTMETHOD, PASSWORD) 
                                       VALUES (@FirstName, @LastName, @Email, @PaymentMethod, @Password)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@FirstName", firstName);
                insertCmd.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(lastName) ? (object)DBNull.Value : lastName);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@PaymentMethod", string.IsNullOrEmpty(paymentMethod) ? (object)DBNull.Value : paymentMethod);
                insertCmd.Parameters.AddWithValue("@Password", password);

                int rows = insertCmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Registration successful!" : "Registration failed.");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
            }
        }

        public static bool LoginUser()
        {
            Console.Clear();
            Console.WriteLine("--- Login ---");

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                conn.Open();

                string loginQuery = @"SELECT * FROM [USER] WHERE EMAIL = @email AND PASSWORD = @pass;";
                SqlCommand loginCmd = new SqlCommand(loginQuery, conn);
                loginCmd.Parameters.AddWithValue("@email", email);
                loginCmd.Parameters.AddWithValue("@pass", password);

                SqlDataReader reader = loginCmd.ExecuteReader();
                IDataRecord userRecord = (IDataRecord)reader;

                if (reader.Read())
                {
                    int id = (int)userRecord[0];
                    string firstName = (string)userRecord[1];
                    string lastName = (string)userRecord[2];
                    string email2 = (string)userRecord[3];
                    string defaultPaymentMethod = (string)userRecord[4];
                    string password2 = (string)userRecord[5];
                    Program.currentLoggedInUser = new User(id, firstName, lastName, email2, defaultPaymentMethod, password2);
                    return true;
                }
                else
                {
                    Console.Write("Invalid credentials.Press Enter To Back....");
                    Console.ReadLine();
                    return false;
                }
            }
        }
        public static void UpdateUser()
        {
            Console.WriteLine("\n--- Update Profile Menu ---");
            Console.WriteLine("1. Update First Name");
            Console.WriteLine("2. Update Last Name");
            Console.WriteLine("3. Update Email");
            Console.WriteLine("4. Update Password");
            Console.WriteLine("5. Add Phone Number");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            string field = "";
            string value = "";

            switch (choice)
            {
                case "1":
                    field = "FIRSTNAME";
                    Console.Write("Enter new first name: ");
                    value = Console.ReadLine();
                    Program.currentLoggedInUser.firstName = value;
                    break;
                case "2":
                    field = "LASTNAME";
                    Console.Write("Enter new last name: ");
                    value = Console.ReadLine();
                    Program.currentLoggedInUser.lastName = value;
                    break;
                case "3":
                    Console.Write("Enter new email: ");
                    value = Console.ReadLine();
                    if (!UserValidation.ValidateEmail(value))
                    {
                        Console.Write("Invalid email format.Press Enter To Back...");
                        Console.ReadKey();
                        return;

                    }
                    field = "EMAIL";
                    Program.currentLoggedInUser.email = value;
                    break;
                case "4":
                    field = "PASSWORD";
                    Console.Write("Enter new Password: ");
                    value = Console.ReadLine();
                    Program.currentLoggedInUser.password = value;
                    break;
                case "5":
                    Console.Write("Enter new phone number: ");
                    value = Console.ReadLine();
                    if (!UserValidation.ValidatePhone(value))
                    {
                        Console.Write("Invalid phone number format.Press Enter To Back...");
                        Console.ReadKey();
                        return;

                    }
                    field = "PHONENUMBER";
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }

            if (field != "PHONENUMBER" && UpdateField(Program.currentLoggedInUser.id, field, value))
                Console.WriteLine($"{field} updated successfully.");
            else if (field == "PHONENUMBER" && AddPhoneNumber(Program.currentLoggedInUser.id, value))
                Console.WriteLine($"{field} Added successfully.");
            else
                Console.WriteLine("Update failed.");
            Console.Write("Press Enter To Back...");
            Console.ReadKey();
        }
        public static bool UpdateField(int userId, string fieldName, string value)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                connection.Open();

                string query = $"UPDATE [USER] SET {fieldName} = @value WHERE USERID = @userId";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        static bool AddPhoneNumber(int userId, string value)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                connection.Open();

                string query = @"insert into [USERPHONE] values(@value,@userId)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }

}
