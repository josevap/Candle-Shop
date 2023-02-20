using System.ComponentModel.DataAnnotations;
//using CandleShop.Models;

namespace CandleShop.Models
{
    public class Candle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(30)]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Display(Name = "Size")]
        public string? Size { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public string? Price { get; set; }

        [StringLength(int.MaxValue)]

        [Display(Name = "Description")]
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public int? BrandId { get; set; }
        [Display(Name = "Brand")]

        public Brand? Brand { get; set; }
        public ICollection<Order>? Users { get; set; }

    }
}
