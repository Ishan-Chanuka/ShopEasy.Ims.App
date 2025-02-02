﻿using Microsoft.AspNetCore.Identity;

namespace ShopEasy.Ims.Domain.Models.DbModels
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
