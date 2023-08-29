using System.ComponentModel.DataAnnotations;

namespace InvMng_InfTech.Models
{
    public class Items
    {
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        [Display(Name = "Part Name")]
        public string PartName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }

        //[Required]
        [Display(Name = "Date")]
        public DateTime DateAdded { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserThatAdded { get; set; }
    }
}
