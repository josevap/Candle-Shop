using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CandleShop.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CandleShopUser class
public class CandleShopUser : IdentityUser
{
    [PersonalData]
    public int user_ID  { get; set; }
}

