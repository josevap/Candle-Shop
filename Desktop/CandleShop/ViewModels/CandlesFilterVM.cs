using CandleShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CandleShop.ViewModels
{
    public class CandlesFilterVM
    {
        public IList<Candle> candles { get; set; }
        public SelectList Categories { get; set; }
        public String candleCategory { get; set; }
        public SelectList Brands { get; set; }
        public String candleBrand { get; set; }
        public String SearchString { get; set; }

    }
}
