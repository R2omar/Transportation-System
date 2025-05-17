using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using UserInfo;
using DriverInfo;
using RideInfo;
using Tripinfo;
using PromoCodeInfo;
using VehicleInfo;
using System.IO;
namespace Trasnportation_System
{
    class Program
    {
        public static string connectionString = @"Server=ABDALLAH;Database=Transportation_system;Trusted_Connection=True;";
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
                            Console.WriteLine("4. View Ride History");
                            Console.WriteLine("5. Search Rides");
                            Console.WriteLine("6. Generate Ride Report");
                            Console.WriteLine("7. View trip details");
                            Console.WriteLine("8. Add Promo Code");
                            Console.WriteLine("9. View All Promo Codes");
                            Console.WriteLine("10. View Vehicle Type");
                            Console.WriteLine("11. Log Out");
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
                                    Console.Clear();
                                    var trips = RideOperations.GetUserRideHistory(currentLoggedInUser.id);
                                    if (trips.Count == 0)
                                    {
                                        Console.WriteLine("No ride history found.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("=== Your Ride History ===");
                                        foreach (var trip in trips)
                                        {
                                            Console.WriteLine("\n" + trip.ToString());
                                            Console.WriteLine("------------------------");
                                        }
                                    }
                                    Console.WriteLine("\nPress Enter to continue...");
                                    Console.ReadLine();
                                    break;
                                case "5":
                                    Console.Clear();
                                    Console.WriteLine("=== Search Ride History ===");
                                    Console.WriteLine("1. Search by Date");
                                    Console.WriteLine("2. Search by Vehicle Type");
                                    Console.WriteLine("3. Search by Both");
                                    Console.WriteLine("4. Back to Main Menu");
                                    Console.Write("Choose an option: ");
                                    string searchOption = Console.ReadLine();

                                    DateTime? rideDate = null;
                                    string vehicleType = null;

                                    if (searchOption == "1" || searchOption == "3")
                                    {
                                        Console.Write("Enter date (yyyy-MM-dd): ");
                                        string dateInputStr = Console.ReadLine();

                                        if (DateTime.TryParseExact(dateInputStr, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateInput))
                                        {
                                            rideDate = dateInput;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                                            Console.WriteLine("\nPress Enter to continue...");
                                            Console.ReadLine();
                                            return;
                                        }
                                    }

                                    if (searchOption == "2" || searchOption == "3")
                                    {
                                        Console.Write("Enter vehicle type: ");
                                        vehicleType = Console.ReadLine();

                                        if (string.IsNullOrWhiteSpace(vehicleType))
                                        {
                                            Console.WriteLine("Vehicle type cannot be empty.");
                                            Console.WriteLine("\nPress Enter to continue...");
                                            Console.ReadLine();
                                            return;
                                        }
                                    }

                                    if (searchOption == "4")
                                    {
                                        break;
                                    }

                                    var filteredRids = RideOperations.GetRidesByVehicleOrDate(currentLoggedInUser.id, vehicleType, rideDate);

                                    if (filteredRids.Count == 0)
                                    {
                                        Console.WriteLine("No rides found for the selected criteria.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("=== Filtered Ride History ===");
                                        foreach (var trip in filteredRids)
                                        {
                                            Console.WriteLine("\n" + trip.ToString());
                                            Console.WriteLine("------------------------");
                                        }
                                    }

                                    Console.WriteLine("\nPress Enter to continue...");
                                    Console.ReadLine();
                                    break;
                                case "6":
                                    Console.Clear();
                                    UserReportGenerator.reportGenerator();
                                    break;
                                case "7":
                                    Console.Clear();
                                    TripDetails.DisplayTripDetailsMenu(currentLoggedInUser.id);
                                    break;
                                case "8":
                                    Console.Clear();
                                    PromoCodeOperations.AddPromoCode();
                                    Console.WriteLine("\nPress Enter to continue...");
                                    Console.ReadLine();
                                    break;
                                case "9":
                                    Console.Clear();
                                    var promoCodes = PromoCodeOperations.GetAllPromoCodes();
                                    if (promoCodes.Count == 0)
                                    {
                                        Console.WriteLine("No promo codes found.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("=== All Promo Codes ===");
                                        foreach (var pc in promoCodes)
                                        {
                                            Console.WriteLine(pc);
                                        }
                                    }
                                    Console.WriteLine("\nPress Enter to continue...");
                                    Console.ReadLine();
                                    break;
                                case "10":
                                    Console.Clear();
                                    VehicleOperations.DisplayAvailableVehicleTypes(currentLoggedInUser.id);
                                    break;
                                case "11":
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
