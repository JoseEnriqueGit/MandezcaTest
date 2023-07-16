using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandezcaTest.Models
{
    public class Address
    {
        [Key]
        [Column("address_id")]
        public int AddressId { get; set; }

        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("address_line")]
        [MaxLength(100)]
        public string AddressLine { get; set; }

        [Column("city")]
        [MaxLength(50)]
        public string City { get; set; }

        [Column("state")]
        [MaxLength(50)]
        public string State { get; set; }

        [Column("country")]
        [MaxLength(50)]
        public string Country { get; set; }

        [Column("postal_code")]
        [MaxLength(20)]
        public string PostalCode { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
