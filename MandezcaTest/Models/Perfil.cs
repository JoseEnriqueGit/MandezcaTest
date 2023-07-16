using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandezcaTest.Models
{
    public class Perfil
    {
        [Key]
        [Column("perfil_id")]
        public int PerfilId { get; set; }

        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("perfil_title")]
        [MaxLength(50)]
        public string PerfilTitle { get; set; }

        [Column("perfil_description")]
        [MaxLength(100)]
        public string PerfilDescription { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
