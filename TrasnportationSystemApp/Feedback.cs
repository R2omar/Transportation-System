using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Trasnportation_System;

namespace FeedbackInfo
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int TripId { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public DateTime SubmittedAtDate { get; set; }
        public TimeSpan SubmittedAtTime { get; set; }

        public override string ToString()
        {
            return $"Feedback ID: {FeedbackId}\n" +
                   $"Trip ID: {TripId}\n" +
                   $"User ID: {UserId}\n" +
                   $"Rating: {Rating} Stars\n" +
                   $"Comments: {Comments}\n" +
                   $"Submitted On: {SubmittedAtDate.ToShortDateString()} at {SubmittedAtTime}";
        }
    }
}
