using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvMng_InfTech.Models.Masters
{
    public class SubLocationMaster
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public Guid? SubLocationID { get; set; }

        [Required]
        [MaxLength(50)]
        public string? SubLocation { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }

        [Column("Modified Date")]
        public DateTime? ModifiedDate { get; set; }

    }
}
