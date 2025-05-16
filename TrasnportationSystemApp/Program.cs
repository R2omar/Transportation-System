using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using UserInfo;
using DriverInfo;
using System.IO;
namespace Trasnportation_System
{
  class Program
  {
    public static string connectionString = @"Server=DESKTOP-PU6LD79;Database=Transportation_System;Trusted_Connection=True;";
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
              Console.WriteLine("2. View All Drivers");
              Console.WriteLine("3. Search Driver by ID");
              Console.WriteLine("4. Log Out");
              Console.Write("Choose an option: ");
              input = Console.ReadLine();
              switch (input)
              {
                case "1":
                  Console.Clear();
                  UserOperations.UpdateUser();
                  break;
                case "2":
                  Console.Clear();
                  var drivers = DriverOperations.GetAllDrivers();
                  if (drivers.Count == 0)
                  {
                    Console.WriteLine("No drivers found.");
                  }
                  else
                  {
                    Console.WriteLine("=== All Drivers ===");
                    foreach (var driver in drivers)
                    {
                      Console.WriteLine(driver);
                    }
                  }
                  Console.WriteLine("\nPress Enter to continue...");
                  Console.ReadLine();
                  break;
                case "3":
                  Console.Clear();
                  Console.Write("Enter Driver ID: ");
                  if (int.TryParse(Console.ReadLine(), out int driverId))
                  {
                    var driver = DriverOperations.GetDriverById(driverId);
                    if (driver != null)
                    {
                      Console.WriteLine("\nDriver Found:");
                      Console.WriteLine(driver);
                    }
                    else
                    {
                      Console.WriteLine("Driver not found.");
                    }
                  }
                  else
                  {
                    Console.WriteLine("Invalid ID format.");
                  }
                  Console.WriteLine("\nPress Enter to continue...");
                  Console.ReadLine();
                  break;
                case "4":
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