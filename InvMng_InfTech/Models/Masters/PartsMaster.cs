using System.ComponentModel.DataAnnotations;

namespace InvMng_InfTech.Models.Masters
{
    public class PartsMaster
    {
        [Key]
        public Guid PartID { get; set; }
        public string? Brand { get; set; }
        public string? PartNumber { get; set; }
        public string? PartName { get; set; }
        public string? Description { get; set; }
        public int? MinNew { get; set; }
        public int? MinUsed { get; set; }
        public string? Bin { get; set; }
        public DateTime? Modified { get; set; }
    }
}
