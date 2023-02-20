using CandleShop.Models;
using System.ComponentModel.DataAnnotations;

namespace CandleShop.ViewModels
{
    public class CandleOrder
    {
        public User? user { get; set; }
        public int? candleId { get; set; }
        [Required]
        public String location { get; set; }
    }
}
