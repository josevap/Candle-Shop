using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CandleShop.Areas.Identity.Data;
using CandleShop.Models;

namespace CandleShop.Data
{
    public class CandleShopContext : IdentityDbContext<CandleShopUser>

    {
        public CandleShopContext (DbContextOptions<CandleShopContext> options)
            : base(options)
        {
        }

        public DbSet<CandleShop.Models.Candle>? Candle { get; set; }

        public DbSet<CandleShop.Models.Brand>? Brand { get; set; }

        public DbSet<CandleShop.Models.User>? User { get; set; }

        public DbSet<CandleShop.Models.Order>? Order { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>()
                .HasOne<User>(d => d.User)
                .WithMany(x => x.Candles)
                .HasForeignKey(d => d.UserId);
            builder.Entity<Order>()
                .HasOne<Candle>(m => m.Candle)
                .WithMany(x => x.Users)
                .HasForeignKey(m => m.CandleId);
            builder.Entity<Candle>()
                .HasOne<Brand>(d => d.Brand)
                .WithMany(x => x.Candles)
                .HasForeignKey(m => m.BrandId);
        }

    }
}
