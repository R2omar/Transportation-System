using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PromoCodeInfo
{
    class PromoCode
    {
        public int PromoCodeId { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public string Description { get; }
        public string Code { get; }
        public decimal DiscountAmount { get; }
        public bool IsActive { get; }
        public int UsageLimit { get; }

        public PromoCode(int promoCodeId, DateTime startDate, DateTime? endDate, string description, string code, decimal discountAmount, bool isActive, int usageLimit)
        {
            PromoCodeId = promoCodeId;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Code = code;
            DiscountAmount = discountAmount;
            IsActive = isActive;
            UsageLimit = usageLimit;
        }

        public override string ToString()
        {
            return $"Code: {Code}, Discount: {DiscountAmount:C}, Active: {IsActive}, Start: {StartDate.ToShortDateString()}, End: {(EndDate.HasValue ? EndDate.Value.ToShortDateString() : "N/A")}, Limit: {UsageLimit}, Desc: {Description}";
        }
    }

    class PromoCodeOperations
    {
        public static void AddPromoCode()
        {
            try
            {
                Console.Write("Enter Promo Code ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter Code: ");
                string code = Console.ReadLine();

                Console.Write("Enter Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter End Date (yyyy-MM-dd or leave empty): ");
                string endInput = Console.ReadLine();
                DateTime? endDate = string.IsNullOrWhiteSpace(endInput) ? (DateTime?)null : DateTime.Parse(endInput);

                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Console.Write("Enter Discount Amount: ");
                decimal discount = decimal.Parse(Console.ReadLine());

                Console.Write("Is Active (true/false): ");
                bool isActive = bool.Parse(Console.ReadLine());

                Console.Write("Enter Usage Limit: ");
                int usageLimit = int.Parse(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(Trasnportation_System.Program.connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO PROMOCODE (PROMOCODEID, CODE, STARTDATE, ENDDATE, DESCRIPTION, DISCOUNTAMOUNT, ISACTIVE, USAGELIMIT) " +
                                   "VALUES (@Id, @Code, @StartDate, @EndDate, @Desc, @Discount, @IsActive, @Limit)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Code", code);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Desc", (object)description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Discount", discount);
                        cmd.Parameters.AddWithValue("@IsActive", isActive);
                        cmd.Parameters.AddWithValue("@Limit", usageLimit);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Promo code added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding promo code: {ex.Message}");
            }
        }

        public static List<PromoCode> GetAllPromoCodes()
        {
            var promoCodes = new List<PromoCode>();
            using (SqlConnection conn = new SqlConnection(Trasnportation_System.Program.connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM PROMOCODE";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        promoCodes.Add(new PromoCode(
                            reader.GetInt32(0),
                            reader.GetDateTime(1),
                            reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            reader.IsDBNull(3) ? null : reader.GetString(3),
                            reader.GetString(4),
                            reader.GetDecimal(5),
                            reader.GetBoolean(6),
                            reader.GetInt32(7)
                        ));
                    }
                }
            }
            return promoCodes;
        }
    }
}
