using NGO_PJsem3.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGO_PJsem3.Models
{
    public class Donations
    {
        [Key]
        public int DonationId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Causes")]
        public int CauseId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DonationAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime DonationDate { get; set; }

        public virtual Users Users { get; set; }
        public virtual Causes Causes { get; set; }
    }
}
