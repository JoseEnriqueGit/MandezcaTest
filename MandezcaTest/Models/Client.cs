using System.ComponentModel.DataAnnotations;

namespace MandezcaTest.Models
{
    public class Client
    {
        [Key]
        public int client_id { get; set; }
        public string? client_name { get; set; }
        public string? client_email { get; set; }
        public string? client_phone { get; set; }
    }
}
