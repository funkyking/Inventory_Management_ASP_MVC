using InvMng_InfTech.Models.Order.Customers;
using Microsoft.AspNetCore.Mvc;

namespace InvMng_InfTech.Models
{
    public class DashboardViewModel
    {

        // Global Stock Information

        public int TotalBrands { get; set; }
        public int TotalParts{ get; set; }
        public string FavouriteBrand { get; set; }
        public string leastFavouriteBrand { get; set; }

        
        //Finance Part
        public decimal TotalSales { get; set; }
        public int TotalCustomers { get; set; }
        

        // This is the Item Storage Indicator
        public Items LowestStock { get; set; }
        public Items HighestStock { get; set; }
        public Items LatestStock { get; set; }
        public Items MostBoughtStock { get; set; }



        
        // Order Information
        public Order.Order LatestCustomerOrder { get; set; }
        public IEnumerable<Order.OrderItem> LatestCustomerItems { get; set; }



        public Order.Order CustomerWithMostItems { get; set; }
        public IEnumerable<Order.OrderItem> CustomerWithMostOrderItems { get; set; }


        

    }
}
