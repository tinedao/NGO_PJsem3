using NGO_PJsem3.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGO_PJsem3.Models
{
    public class Querys
    {
        [Key]
        public int QueryId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string QuerySubject { get; set; }

        [Required]
        [StringLength(1000)]
        public string QueryMessage { get; set; }

        [DataType(DataType.Date)]
        public DateTime QueryDate { get; set; }

        public virtual Users Users { get; set; }
    }
}
