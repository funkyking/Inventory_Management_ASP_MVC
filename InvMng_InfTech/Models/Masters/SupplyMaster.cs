using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace InvMng_InfTech.Models.Masters
{
    public class SupplyMaster
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [MaxLength(30)]
        public string? Representative { get; set; }

        [MaxLength(30)]
        public string? Department { get; set; }

        [MaxLength(30)]
        public string? Position { get; set; }

        [MaxLength(30)]
        public string? ContactNo { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? State { get; set; }

        [MaxLength(30)]
        public string? City { get; set; }

        [MaxLength(30)]
        public string? Postcode { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? Website { get; set; }

        [MaxLength(50)]
        public string? Industry { get; set; }

        public DateTime? Modified { get; set; }
    }
}
