using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGO_PJsem3.Models
{
    public class NGOs
    {
        [Key]
        public int NgoId { get; set; }

        [Required]
        [StringLength(100)]
        public string NgoName { get; set; }

        [StringLength(500)]
        public string NgoDescription { get; set; }

        public byte[] NgoLogo { get; set; }

        public virtual ICollection<Programs> Programs { get; set; }
    }
}
