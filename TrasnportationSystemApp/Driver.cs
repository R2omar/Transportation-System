using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Trasnportation_System;
using DriverInfo;
using VehicleInfo;


namespace DriverInfo
{
  class Driver
  {
    public int DriverId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string LicenceNumber { get; }
    public string Status { get; }
    public List<string> PhoneNumbers { get; }

    public double? AverageRating { get; }

    public Driver(int driverId, string firstName, string lastName, string licenceNumber, string status, List<string> phoneNumbers, double? averageRating = null)
{
      DriverId = driverId;
      FirstName = firstName;
      LastName = lastName;
      LicenceNumber = licenceNumber;
      Status = status;
      PhoneNumbers = phoneNumbers ?? new List<string>();
      AverageRating = averageRating;
}
    public override string ToString()
    {
      string phones = string.Join(", ", PhoneNumbers);
      string rating = AverageRating.HasValue ? AverageRating.Value.ToString("0.00") : "N/A";
      return $"Driver: {FirstName} {LastName}, License: {LicenceNumber}, Status: {Status}, Phones: [{phones}], Avg Rating: {rating}";
    }
  }
}

class DriverOperations
{
  public static Driver GetDriverById(int driverId)
  {
    List<string> phones = new List<string>();
    using (SqlConnection conn = new SqlConnection(Program.connectionString))
    {
      conn.Open();
      string query = "SELECT DRIVERID, FIRSTNAME, LASTNAME, LICENCENUMBER, STATUS FROM DRIVER WHERE DRIVERID = @DriverId";
      using (SqlCommand cmd = new SqlCommand(query, conn))
      {
        cmd.Parameters.AddWithValue("@DriverId", driverId);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
          if (!reader.Read())
            return null;
          int id = reader.GetInt32(0);
          string firstName = reader.GetString(1);
          string lastName = reader.IsDBNull(2) ? null : reader.GetString(2);
          string licenceNumber = reader.GetString(3);
          string status = reader.GetString(4);
          reader.Close();
          string phoneQuery = "SELECT PHONENUMBER FROM DRIVERPHONE WHERE DRIVERID = @DriverId";
          using (SqlCommand phoneCmd = new SqlCommand(phoneQuery, conn))
          {
            phoneCmd.Parameters.AddWithValue("@DriverId", driverId);
            using (SqlDataReader phoneReader = phoneCmd.ExecuteReader())
            {
              while (phoneReader.Read())
              {
                phones.Add(phoneReader.GetString(0));
              }
            }
          }
          return new Driver(id, firstName, lastName, licenceNumber, status, phones);
        }
      }
    }
  }

  public static List<Driver> GetAllDrivers()
  {
    var drivers = new List<Driver>();
    using (SqlConnection conn = new SqlConnection(Program.connectionString))
    {
      conn.Open();
      var tempDriverList = new List<(int DriverId, string FirstName, string LastName, string LicenceNumber, string Status)>();
      string query = "SELECT DRIVERID, FIRSTNAME, LASTNAME, LICENCENUMBER, STATUS FROM DRIVER";
      using (SqlCommand cmd = new SqlCommand(query, conn))
      using (SqlDataReader reader = cmd.ExecuteReader())
      {
        while (reader.Read())
        {
          tempDriverList.Add((
              DriverId: reader.GetInt32(0),
              FirstName: reader.GetString(1),
              LastName: reader.IsDBNull(2) ? null : reader.GetString(2),
              LicenceNumber: reader.GetString(3),
              Status: reader.GetString(4)
          ));
        }
      }
      foreach (var d in tempDriverList)
        {
          var phones = GetPhonesByDriverId(d.DriverId, conn);
          double? avgRating = GetAverageRatingByDriverId(d.DriverId, conn);
          drivers.Add(new Driver(d.DriverId, d.FirstName, d.LastName, d.LicenceNumber, d.Status, phones, avgRating));
        }
    }
    return drivers;
  }

  private static List<string> GetPhonesByDriverId(int driverId, SqlConnection conn)
  {
    var phones = new List<string>();
    string query = "SELECT PHONENUMBER FROM DRIVERPHONE WHERE DRIVERID = @DriverId";
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
      cmd.Parameters.AddWithValue("@DriverId", driverId);
      using (SqlDataReader reader = cmd.ExecuteReader())
      {
        while (reader.Read())
        {
          phones.Add(reader.GetString(0));
        }
      }
    }
    return phones;
  }
    private static double? GetAverageRatingByDriverId(int driverId, SqlConnection conn)
    {
        string query = @"
        SELECT AVG(CAST(F.RATING AS FLOAT))
        FROM FEEDBACK F
        INNER JOIN TRIP T ON F.TRIP_ID = T.TRIP_ID
        WHERE T.DRIVERID = @DriverId";

        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@DriverId", driverId);
            object result = cmd.ExecuteScalar();
            return result != DBNull.Value ? (double?)Convert.ToDouble(result) : null;
        }
    }
    public static List<Driver> GetAvailableDriversByVehicleType(string vehicleType)
    {
        var availableDrivers = new List<Driver>();

        using (SqlConnection conn = new SqlConnection(Program.connectionString))
        {
            conn.Open();

            string query = @"
            SELECT DISTINCT D.DRIVERID, D.FIRSTNAME, D.LASTNAME, D.LICENCENUMBER, D.STATUS
            FROM DRIVER D
            INNER JOIN VEHICLE V ON D.DRIVERID = V.DRIVERID
            WHERE D.STATUS = 'Available' AND V.VEHICLETYPE = @VehicleType";

            var tempDrivers = new List<(int DriverId, string FirstName, string LastName, string LicenceNumber, string Status)>();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tempDrivers.Add((
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? null : reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        ));
                    }
                }
            }

            foreach (var d in tempDrivers)
            {
                var phones = GetPhonesByDriverId(d.DriverId, conn);
                double? avgRating = GetAverageRatingByDriverId(d.DriverId, conn);
                availableDrivers.Add(new Driver(d.DriverId, d.FirstName, d.LastName, d.LicenceNumber, d.Status, phones, avgRating));
            }
        }

        return availableDrivers;
    }

}
