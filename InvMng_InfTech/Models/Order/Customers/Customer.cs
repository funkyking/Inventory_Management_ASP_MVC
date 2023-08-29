namespace InvMng_InfTech.Models.Order.Customers
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        // Other customer-related properties

        // One-to-many relationship with Orders
        public ICollection<Order> Orders { get; set; }
    }
}
