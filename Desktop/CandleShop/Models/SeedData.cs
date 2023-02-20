using CandleShop.Areas.Identity.Data;
using CandleShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CandleShop.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<CandleShopUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            CandleShopUser user = await UserManager.FindByEmailAsync("admin@candleshop.com");
            if (user == null)
            {
                var User = new CandleShopUser();
                User.Email = "admin@candleshop.com";
                User.UserName = "admin@candleshop.com";
                string userPWD = "admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            /*   var x = await RoleManager.RoleExistsAsync("Guest");
               if (!x)
               {
                   var role = new IdentityRole();
                   role.Name = "Guest";
                   await RoleManager.CreateAsync(role);
               }*/
            // }



            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }
            user = await UserManager.FindByEmailAsync("user@candleshop.com");
            if (user == null)
            {
                var User = new CandleShopUser();
                User.Email = "user@candleshop.com";
                User.UserName = "user@candleshop.com";
                string userPWD = "user123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);

                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "User");
                }
            }
        }
        

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CandleShopContext(
               serviceProvider.GetRequiredService<
                   DbContextOptions<CandleShopContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Brand.Any() || context.Candle.Any() || context.User.Any() || context.Order.Any())
                {
                    return;
                }
                context.Brand.AddRange(
                    new Brand
                    {
                       // Id = 1,
                        Name = "Otherland",
                        Description = "Otherland candles seem to evoke a sense of wanderlust, and not in the nostalgic sense, but to imaginary " +
                        "places we've never been. The kind you'd imagine in a fairytale. Or the Upside Down. They're colorful and quirky, yet " +
                        "still sophisticated."
                    },
                    new Brand
                    {
                      //  Id = 2,
                        Name = "Voluspa",
                        Description = "Voluspa Candles are made with creamy coconut wax which enhances fragrance throw, burns ultra clean, " +
                        "with fragrance and candle light that delights the senses and adds luxe to everyday living.Voluspa Japonica Candle " +
                        "Collection pays homage to beautiful works of Japanese art."
                    },
                    new Brand()
                    {
                      //  Id = 3,
                        Name = "Ani & Co.",
                        Description = "Ani&Co. is a handmade Luxury Coconut Wax candle brand that also offers a small Smudge stick line & small " +
                        "bath collection.We use high end fragrances that are meant to reclaim your own a peace, and evoke positivity with a " +
                        "memorable scent you dare to forget. "
                    },
                    new Brand()
                    {
                      //  Id = 4,
                        Name = "Diptyque",
                        Description = "These Parisian candles are renowned for their exceptional quality and their beautiful designs. Plus, the " +
                        "manufacturers of Diptyque candles are famous for never having used synthetic fragrances in their products. Among luxury " +
                        "scented candles, this places Diptyque candles in a unique category."
                    });
                context.SaveChanges();

                context.Candle.AddRange(
                    new Candle
                    {
                       // Id = 1,
                        Name = "Daybed",
                        Description = "Rosebud / Peony Blossom / Pear Water, 55 Hour Burn Time • Coconut & Soy Wax Blend.",
                        Price = "$36",
                        Size = "3.25'' Tall • 8 Ounces",
                        Picture = "./pictures/otherland-daybed-spring.png",
                        Category = "Spring",
                        BrandId = 1
                    },
                    new Candle
                    {
                      //  Id = 2,
                        Name = "Forest Veil",
                        Description = "Chalet Cedar / Antique Sandalwood / Emerald Vetiver, 50 Hour Burn Time • Coconut & Soy Wax Blend.",
                        Price = "$36",
                        Size = "3.25'' Tall • 8 Ounces",
                        Picture = "./pictures/otherland-forest-veil-fall.png",
                        Category = "Fall",
                        BrandId = 1
                    },
                    new Candle
                    {
                      //  Id = 3,
                        Name = "Crushed Candy Cane",
                        Description = "Notes of crisp peppermint and warm buttery scent of fresh baked cookies.",
                        Price = "$26",
                        Size = "2.50'' Medium • 6 Ounces",
                        Picture = "./pictures/voluspa-candy-cane-winter.png",
                        Category = "Winter",
                        BrandId = 2
                    },
                    new Candle
                    {
                     //   Id = 4,
                        Name = "Kalahari Watermelon",
                        Description = "Notes of Chilled Watermelon, Pressed Lime & Holy Basil.",
                        Price = "$78",
                        Size = "3.25'' Tall • 8 Ounces",
                        Picture = "./pictures/voluspa-kalahari-watermelon-summer.png",
                        Category = "Summer",
                        BrandId = 2
                    },
                    new Candle
                    {
                      //  Id = 5,
                        Name = "Homme",
                        Description = "Feels like the ocean breeze against your skin. Top: Petitgrain, Ozone. " +
                        "Heart: Eucalyptus, Plum, Sea Salt. Base: Sandalwood, Powder",
                        Price = "$30",
                        Size = "2'' Medium • 5 Ounces",
                        Picture = "./pictures/ani-co-homme-summer.jfif",
                        Category = "Summer",
                        BrandId = 3
                    },
                    new Candle
                    {
                       // Id = 6,
                        Name = "Gardenia",
                        Description = "Best enjoyed on every self care Sunday. Top: Gardenia, Lemon Peel. " +
                        "Heart: Green Floral, Jasmine, Tuberose. Base: Powder",
                        Price = "$42",
                        Size = "2.25'' Medium • 5.5 Ounces",
                        Picture = "./pictures/ani-co-gardenia-spring.jfif",
                        Category = "Spring",
                        BrandId = 3
                    },
                    new Candle
                    {
                      //  Id = 7,
                        Name = "Ambre/Amber Candle",
                        Description = "Amber spheres discovered in an ancient treasure box. They now have" +
                        " the heady fragrance of balms and spices, of precious and enveloping woods.",
                        Price = "$56",
                        Size = "3.25'' Tall • 8 Ounces",
                        Picture = "./pictures/diptyque-amber-fall.png",
                        Category = "Fall",
                        BrandId = 4
                    },
                    new Candle
                    {
                      //  Id = 8,
                        Name = "Cannelle/Cinnamon Candle",
                        Description = "Cinnamon bark freshly stripped from the tree branches. It has kept the " +
                        "captivating scents of its land of origin, India, and its accents, at times woody and" +
                        " spicy, at others warm and gourmand.",
                        Price = "$62",
                        Size = "3.25'' Tall • 8 Ounces",
                        Picture = "./pictures/diptyque-cinnamon-winter.png",
                        Category = "Winter",
                        BrandId = 4
                    });
                context.SaveChanges();
                context.User.AddRange(
                    new User
                    {
                        FirstName = "Pavlinka",
                        LastName = "Josheva",
                        ProfilePicture = null
                    },
                    new User
                    {
                        FirstName = "Ina",
                        LastName = "Todorova",
                        ProfilePicture = null
                    },
                    new User
                    {
                        FirstName = "Jovan",
                        LastName = "Pavlov",
                        ProfilePicture = null
                    });
                context.SaveChanges();
                context.Order.AddRange(
                    new Order { Status = "Pending Approval", UserId = 1, CandleId = 2 },
                    new Order { Status = "Pending Approval", UserId = 1, CandleId = 5 },
                    new Order { Status = "Approved", UserId = 1, CandleId = 8 },
                    new Order { Status = "Approved", UserId = 3, CandleId = 4 },
                    new Order { Status = "Approved", UserId = 2, CandleId = 3 });
                context.SaveChanges();
            }
        }
    }
}

