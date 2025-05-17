using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Trasnportation_System;

namespace Tripinfo
{
    public class TripDetails
    {
        public int TripId { get; set; }
        public string DriverName { get; set; }
        public string LicensePlateNumber { get; set; }
        public string VehicleType { get; set; }
        public string DriverPhoneNumber { get; set; }
        public DateTime RideTime { get; set; }
        public decimal RideFees { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string RideStatus { get; set; }

        public static List<TripDetails> GetUserTrips(int userId)
        {
            List<TripDetails> trips = new List<TripDetails>();

            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        t.TRIP_ID,
                        t.DRIVER_NAME,
                        v.PLATE_NUMBER,
                        v.VEHICLE_TYPE,
                        dp.PHONENUMBER,
                        t.RIDE_TIME,
                        t.RIDE_FEES,
                        r.PICKUP_STREET + ', ' + r.PICKUP_CITY as PickupLocation,
                        r.DEOPOFF_STREET + ', ' + r.DROPOFF_CITY as DropoffLocation,
                        r.RIDESTATUS
                    FROM TRIP t
                    JOIN RIDE r ON t.RIDEID = r.RIDEID AND t.USERID = r.USERID AND t.DRIVERID = r.DRIVERID
                    JOIN DRIVER d ON t.DRIVERID = d.DRIVERID
                    LEFT JOIN VEHICLE v ON d.DRIVERID = v.DRIVERID
                    LEFT JOIN DRIVERPHONE dp ON d.DRIVERID = dp.DRIVERID
                    WHERE t.USERID = @UserId
                    ORDER BY t.RIDE_TIME DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        trips.Add(new TripDetails
                        {
                            TripId = (int)reader["TRIP_ID"],
                            DriverName = reader["DRIVER_NAME"].ToString(),
                            LicensePlateNumber = reader["PLATE_NUMBER"]?.ToString() ?? "N/A",
                            VehicleType = reader["VEHICLE_TYPE"]?.ToString() ?? "N/A",
                            DriverPhoneNumber = reader["PHONENUMBER"]?.ToString() ?? "N/A",
                            RideTime = Convert.ToDateTime(reader["RIDE_TIME"]),
                            RideFees = Convert.ToDecimal(reader["RIDE_FEES"]),
                            PickupLocation = reader["PickupLocation"].ToString(),
                            DropoffLocation = reader["DropoffLocation"].ToString(),
                            RideStatus = reader["RIDESTATUS"].ToString()
                        });
                    }
                }
            }

            return trips;
        }

        public static void DisplayTripDetailsMenu(int userId)
        {
            Console.Clear();
            Console.WriteLine("=== My Trips ===");

            var trips = GetUserTrips(userId);

            if (trips.Count == 0)
            {
                Console.WriteLine("No trips found for this user.");
                Console.WriteLine("\nPress Enter to return...");
                Console.ReadLine();
                return;
            }

            // Display list of trips
            Console.WriteLine("\nYour Recent Trips:");
            for (int i = 0; i < trips.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Trip ID: {trips[i].TripId} | Date: {trips[i].RideTime.ToShortDateString()} | From: {trips[i].PickupLocation} | To: {trips[i].DropoffLocation}  | Fare: {trips[i].RideFees:C}");
            }

            Console.Write("\nSelect a trip to view details (or 0 to return): ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= trips.Count)
            {
                DisplayTripDetails(trips[choice - 1]);
            }
        }

        private static void DisplayTripDetails(TripDetails trip)
        {
            Console.Clear();
            Console.WriteLine("=== Trip Details ===");
            Console.WriteLine($"Trip ID: {trip.TripId}");
            Console.WriteLine($"Status: {trip.RideStatus}");
            Console.WriteLine($"Date & Time: {trip.RideTime}");
            Console.WriteLine($"From: {trip.PickupLocation}");
            Console.WriteLine($"To: {trip.DropoffLocation}");
            Console.WriteLine($"\nDriver Information:");
            Console.WriteLine($"Name: {trip.DriverName}");
            Console.WriteLine($"Phone: {trip.DriverPhoneNumber}");
            Console.WriteLine($"Vehicle: {trip.VehicleType} ({trip.LicensePlateNumber})");
            Console.WriteLine($"\nFare: {trip.RideFees:C}");

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }
    }
}