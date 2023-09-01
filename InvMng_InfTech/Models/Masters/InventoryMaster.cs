using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvMng_InfTech.Models.Masters
{
    public class InventoryMaster
    {
        [Key]
        [Column("Part ID")]
        public Guid ID { get; set; }


        [Required]
        public string? PartNumber { get; set; }

        [Required]
        public string? PartName { get; set; }

        public string? Brand { get; set; }


        public int? StockNew { get; set; } // Stock In

        public int? StockUsed { get; set; } // Stock Out or Used

        public DateTime? Modified { get; set; }

        public string? Location { get; set; }

        public string? SubLocation { get; set; }
    }
}
