using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace InvMng_InfTech.Models.Masters
{
    public class SupplyPartMaster
    {
        [Key]
        public int ID { get; set; }

        [Column("Part ID")]
        public Guid? PartID { get; set; }

        [MaxLength(50)]
        public string PartNumber { get; set; }

        [MaxLength(50)]
        public string PartName { get; set; }

        [Column("Supplier ID")]
        public Guid? SupplierID { get; set; }

        [MaxLength(50)]
        public string? Supplier { get; set; }

        [MaxLength(50)]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? Remark { get; set; }

        [Column("Modified Date")]
        public DateTime? ModifiedDate { get; set; }

    }
}
