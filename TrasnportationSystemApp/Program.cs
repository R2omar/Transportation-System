using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using UserInfo;
namespace Trasnportation_System
{
    class Program
    {
        public static string connectionString = @"Server=DESKTOP-6H50C7D;Database=Transportation_System;Trusted_Connection=True;";
        public static User currentLoggedInUser;
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== User Authentication ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        UserOperations.RegisterUser();
                        break;
                    case "2":
                        bool isLogin = UserOperations.LoginUser();
                        while (isLogin)
                        {
                            Console.Clear();
                            Console.WriteLine($"    <<Welcome {currentLoggedInUser.firstName}>>");
                            Console.WriteLine("=== User Operations ===");
                            Console.WriteLine("1. Update Profile");
                            Console.WriteLine("2. Log Out");
                            Console.Write("Choose an option: ");
                            input = Console.ReadLine();
                            switch (input)
                            {
                                case "1":
                                    Console.Clear();
                                    UserOperations.UpdateUser();
                                    break;
                                case "2":
                                    isLogin = false;
                                    break;
                            }
                        }
                        break;
                    case "3":
                        Console.WriteLine("Thanks For Using Our Program");
                        return;
                    default:
                        Console.WriteLine("Invalid input. Press Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}