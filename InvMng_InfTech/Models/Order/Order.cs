using InvMng_InfTech.Models.Order.Customers;

namespace InvMng_InfTech.Models.Order
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

       
        // Foreign key to link the order to a customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        
        // Navigation property for order items (initially empty)
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
