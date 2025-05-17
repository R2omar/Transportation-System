using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Trasnportation_System;
using UserInfo;
using System.Security.Cryptography;

namespace RideInfo
{
  public class Ride
  {
    public int RideId { get; set; }
    public int UserId { get; set; }
    public int DriverId { get; set; }
    public int VehicleId { get; set; }
    public string DropOff_Street { get; set; }
    public string DropOff_City { get; set; }
    public string PickUp_Street { get; set; }
    public string PickUp_City { get; set; }
    public DateTime? RequestedTime { get; set; }
    public decimal FinalFare { get; set; }
    public decimal EstimatedFare { get; set; }
    public string DriverFirstName { get; set; }
    public string DriverLastName { get; set; }
    public string VehicleType { get; set; }
    public string Comment { get; set; }
    public int? Rating { get; set; }

    public Ride(int rideId, int userId, int driverId, int vehicleId,
                string dropOffStreet, string dropOffCity,
                string pickUpStreet, string pickUpCity,
                DateTime? requestedTime, decimal estimatedFare, decimal finalFare,
                string driverFirstName, string driverLastName,
                string vehicleType, string comment, int? rating)
    {
      RideId = rideId;
      UserId = userId;
      DriverId = driverId;
      VehicleId = vehicleId;
      DropOff_Street = dropOffStreet;
      DropOff_City = dropOffCity;
      PickUp_Street = pickUpStreet;
      PickUp_City = pickUpCity;
      RequestedTime = requestedTime;
      EstimatedFare = estimatedFare;
      FinalFare = finalFare;
      DriverFirstName = driverFirstName;
      DriverLastName = driverLastName;
      VehicleType = vehicleType;
      Comment = comment;
      Rating = rating;
    }

    public override string ToString()
    {
      return $"Ride ID: {RideId}\n" +
             $"Driver: {DriverFirstName} {DriverLastName}\n" +
             $"Vehicle: {VehicleType}\n" +
             $"Pickup: {PickUp_Street}, {PickUp_City}\n" +
             $"Dropoff: {DropOff_Street}, {DropOff_City}\n" +
             $"Requested Time: {RequestedTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}\n" +
             $"Estimated Fare: ${EstimatedFare:F2}\n" +
             $"Final Fare: ${FinalFare:F2}\n" +
             $"Rating: {(Rating.HasValue ? Rating.ToString() : "N/A")}\n" +
             $"Comment: {Comment ?? "No comment"}";
    }
  }

  public class RideOperations
  {
    public static List<Ride> GetUserRideHistory(int userId, DateTime? startDate = null)
    {
      var rides = new List<Ride>();

      using (SqlConnection conn = new SqlConnection(Program.connectionString))
      {
        conn.Open();

        string query = @"
            SELECT 
                r.RIDEID, r.USERID, r.DRIVERID, r.VEHICLEID, r.DEOPOFF_STREET, 
                r.DROPOFF_CITY, r.PICKUP_STREET, r.PICKUP_CITY, r.REQUESTEDTIME,
                r.FINALFARE, r.ESTIMATEDFARE, d.FIRSTNAME, d.LASTNAME, v.VEHICLE_TYPE, 
                f.COMMENTS, f.RATING
            FROM ride r
            JOIN driver d ON r.DRIVERID = d.DRIVERID
            JOIN vehicle v ON r.VEHICLEID = v.VEHICLEID
            JOIN trip t ON t.rideid = r.rideid
            LEFT JOIN feedback f ON f.trip_id = t.trip_id
            WHERE r.USERID = @UserId";

        if (startDate.HasValue)
        {
          query += " AND CONVERT(date, r.REQUESTEDTIME) >= @StartDate";
        }

        query += " ORDER BY r.REQUESTEDTIME DESC";

        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@UserId", userId);

          if (startDate.HasValue)
          {
            cmd.Parameters.AddWithValue("@StartDate", startDate.Value.Date);
          }

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              rides.Add(new Ride(
                  rideId: (int)reader["RIDEID"],
                  userId: (int)reader["USERID"],
                  driverId: (int)reader["DRIVERID"],
                  vehicleId: (int)reader["VEHICLEID"],
                  dropOffStreet: reader["DEOPOFF_STREET"].ToString(),
                  dropOffCity: reader["DROPOFF_CITY"].ToString(),
                  pickUpStreet: reader["PICKUP_STREET"].ToString(),
                  pickUpCity: reader["PICKUP_CITY"].ToString(),
                  requestedTime: reader["REQUESTEDTIME"] == DBNull.Value ? null : (DateTime?)reader["REQUESTEDTIME"],
                  estimatedFare: (decimal)reader["ESTIMATEDFARE"],
                  finalFare: (decimal)reader["FINALFARE"],
                  driverFirstName: reader["FIRSTNAME"].ToString(),
                  driverLastName: reader["LASTNAME"].ToString(),
                  vehicleType: reader["VEHICLE_TYPE"].ToString(),
                  comment: reader["COMMENTS"] == DBNull.Value ? null : reader["COMMENTS"].ToString(),
                  rating: reader["RATING"] == DBNull.Value ? null : (int?)reader["RATING"]
              ));
            }
          }
        }
      }

      return rides;
    }

    public static List<Ride> GetRidesByVehicleOrDate(int userId, string vehicleType = null, DateTime? rideDate = null)
    {
      var rides = new List<Ride>();

      using (SqlConnection conn = new SqlConnection(Program.connectionString))
      {
        conn.Open();
        string query = @"
            SELECT 
                r.RIDEID, r.USERID, r.DRIVERID, r.VEHICLEID, r.DEOPOFF_STREET, 
                r.DROPOFF_CITY, r.PICKUP_STREET, r.PICKUP_CITY, r.REQUESTEDTIME,
                r.FINALFARE, r.ESTIMATEDFARE, d.FIRSTNAME, d.LASTNAME, v.VEHICLE_TYPE, 
                f.COMMENTS, f.RATING,
                t.ride_time
            FROM ride r
            JOIN driver d ON r.DRIVERID = d.DRIVERID
            JOIN vehicle v ON r.VEHICLEID = v.VEHICLEID
            JOIN trip t ON t.rideid = r.rideid
            LEFT JOIN feedback f ON f.trip_id = t.trip_id
            WHERE r.USERID = @UserId";

        if (!string.IsNullOrEmpty(vehicleType))
          query += " AND v.vehicle_type = @VehicleType";
        if (rideDate.HasValue)
          query += " AND CONVERT(date, t.ride_time) = @RideDate";

        query += " ORDER BY t.ride_time DESC";

        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@UserId", userId);
          if (!string.IsNullOrEmpty(vehicleType))
            cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
          if (rideDate.HasValue)
            cmd.Parameters.AddWithValue("@RideDate", rideDate.Value.Date);

          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              rides.Add(new Ride(
                  rideId: (int)reader["RIDEID"],
                  userId: (int)reader["USERID"],
                  driverId: (int)reader["DRIVERID"],
                  vehicleId: (int)reader["VEHICLEID"],
                  dropOffStreet: reader["DEOPOFF_STREET"].ToString(),
                  dropOffCity: reader["DROPOFF_CITY"].ToString(),
                  pickUpStreet: reader["PICKUP_STREET"].ToString(),
                  pickUpCity: reader["PICKUP_CITY"].ToString(),
                  requestedTime: reader["REQUESTEDTIME"] == DBNull.Value ? null : (DateTime?)reader["REQUESTEDTIME"],
                  estimatedFare: (decimal)reader["ESTIMATEDFARE"],
                  finalFare: (decimal)reader["FINALFARE"],
                  driverFirstName: reader["FIRSTNAME"].ToString(),
                  driverLastName: reader["LASTNAME"].ToString(),
                  vehicleType: reader["VEHICLE_TYPE"].ToString(),
                  comment: reader["COMMENTS"] == DBNull.Value ? null : reader["COMMENTS"].ToString(),
                  rating: reader["RATING"] == DBNull.Value ? null : (int?)reader["RATING"]
              ));
            }
          }
        }
      }

      return rides;
    }
  }
}
