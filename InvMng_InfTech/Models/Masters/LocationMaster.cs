using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace InvMng_InfTech.Models.Location
{
    public class LocationMaster
    {
        [Key] // Primary key
        [Column("Location ID")]
        public Guid? LocationID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Location { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }


        [Column("Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
