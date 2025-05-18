
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Trasnportation_System;

namespace VehicleInfo
{
    public class VehicleType
    {
        public string TypeName { get; set; }
        public int AvailableCount { get; set; }

        public VehicleType(string typeName, int availableCount)
        {
            TypeName = typeName;
            AvailableCount = availableCount;
        }

        public override string ToString()
        {
            return $"{TypeName} (Available: {AvailableCount})";
        }
    }

    public class VehicleOperations
    {
        public static List<VehicleType> GetAvailableVehicleTypes()
        {
            var vehicleTypes = new List<VehicleType>();

            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        v.VEHICLE_TYPE,
                        COUNT(v.VEHICLEID) as AvailableCount
                    FROM VEHICLE v
                    JOIN DRIVER d ON v.DRIVERID = d.DRIVERID
                    WHERE d.STATUS = 'Available'
                    GROUP BY v.VEHICLE_TYPE";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vehicleTypes.Add(new VehicleType(
                            typeName: reader["VEHICLE_TYPE"].ToString(),
                            availableCount: (int)reader["AvailableCount"]
                        ));
                    }
                }
            }

            return vehicleTypes;
        }

        public static void DisplayAvailableVehicleTypes(int userId)
        {
            Console.Clear();
            Console.WriteLine("=== Available Vehicle Types ===");

            var vehicleTypes = GetAvailableVehicleTypes();

            if (vehicleTypes.Count == 0)
            {
                Console.WriteLine("No available vehicle types found.");
            }
            else
            {
                Console.WriteLine("\nAvailable Vehicle Types:");
                foreach (var vehicleType in vehicleTypes)
                {
                    Console.WriteLine(vehicleType);
                }
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
        public static int GetVehicleIdByDriverAndType(int driverId, string vehicleType)
        {
            using (SqlConnection conn = new SqlConnection(Program.connectionString))
            {
                conn.Open();

                string query = @"
                SELECT TOP 1 VEHICLEID
                FROM VEHICLE
                WHERE DRIVERID = @DriverId AND VEHICLE_TYPE = @VehicleType";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DriverId", driverId);
                    cmd.Parameters.AddWithValue("@VehicleType", vehicleType);

                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }



    }


}
