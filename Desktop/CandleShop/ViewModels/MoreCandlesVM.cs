using CandleShop.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CandleShop.ViewModels
{
    public class MoreCandlesVM
    {
        public User? user { get; set; }
        public IEnumerable<int>? selectedCandles { get; set; }
        public IEnumerable<SelectListItem>? candleList { get; set; }
        [Required]
        public String location { get; set; }

    }
}
