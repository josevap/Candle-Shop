using CandleShop.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace CandleShop.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string ? FirstName  { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name ="Last Name")]
        public string ? LastName  { get; set; }
        public string? ProfilePicture { get; set; }
        public string ? FullName 
        {
            get { return FirstName + " " + LastName; }
        }
        public CandleShopUser? user { get; set; }
        public ICollection<Order>? Candles { get; set; }
    }
}
