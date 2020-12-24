using System;

namespace service_harness.Models
{
    public class EmailToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
