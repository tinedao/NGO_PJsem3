using NGO_PJsem3.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGO_PJsem3.Models
{
    public class Programs
    {
        [Key]
        public int ProgramId { get; set; }

        [ForeignKey("NGOs")]
        public int NgoId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProgramName { get; set; }

        [StringLength(500)]
        public string ProgramDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime ProgramDate { get; set; }

        public virtual NGOs NGOs { get; set; }
    }
}
