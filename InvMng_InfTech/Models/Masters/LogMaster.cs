using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvMng_InfTech.Models.Masters
{
    public class LogMaster
    {
        [Key]
        public Guid ID { get; set; }

        
        // User Input
        public string? PartNumber { get; set; }
        public string? PartName { get; set; }
        public string? Location { get; set; }
        public string? SubLocation { get; set; }
        public string? StockInOut { get; set; }
        public string? Supplier { get; set; }
        public string? Remark { get; set; }
        


        // Controller Generates
        public DateTime LogDate { get; set; }
        public Guid? PartID { get; set; }       
        public Guid? SupplierID { get; set; }               
        public int? ExistingStockNew { get; set; }
        public int? ExistingStockUsed { get; set; }
        public int? StockNew { get; set; }
        public int? StockUsed { get; set; }
        public Guid? LocationID { get; set; }       
        public Guid? SubLocationID { get; set; }       
        public string? IssuedBy { get; set; }
        
    }
}
