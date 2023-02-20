﻿using System.ComponentModel.DataAnnotations;

namespace CandleShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name ="Order Status")]
        [StringLength(30)]
        public string? Status { get; set; }
        [Display(Name = "Location")]
        [StringLength(30)]
        public string? Location { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public int CandleId { get; set; }
        public Candle? Candle { get; set; }

    }
}
