using System.ComponentModel.DataAnnotations;

namespace InvMng_InfTech.Models.Order
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        [Display(Name = "Part ID")]
        public string PartId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }

        // Nullable foreign key to link the order item to the order
        public int? OrderId { get; set; }
        public Order Order { get; set; }
    }
}
