using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGO_PJsem3.Models
{
    public class Causes
    {
        [Key]
        public int CauseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CauseName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Donations> Donations { get; set; }
    }
}
