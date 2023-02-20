using System.ComponentModel.DataAnnotations;

namespace CandleShop.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(int.MaxValue)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
       
        public ICollection<Candle>? Candles { get; set; }
    }
}
