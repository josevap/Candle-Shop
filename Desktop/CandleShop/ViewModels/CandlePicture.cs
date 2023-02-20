using CandleShop.Models;
using System.ComponentModel.DataAnnotations;

namespace CandleShop.ViewModels
{
    public class CandlePicture
    {
        public Candle? candle { get; set; }

        [Display(Name = "Upload picture")]
        public IFormFile? pictureFile { get; set; }

        [Display(Name = "Picture")]
        public string? pictureName { get; set; }
    }
}
