using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;


namespace NGO_PJsem3.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool role { get; set; }
        public string imgUser { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Donations> Donations { get; set; }
        public virtual ICollection<Querys> Querys { get; set; }
    }
}
