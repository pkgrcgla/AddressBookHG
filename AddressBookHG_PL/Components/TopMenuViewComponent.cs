﻿using AddressBookHG_EL.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace AddressBookHG_PL.Components
{
    public class TopMenuViewComponent : ViewComponent
    {
        public readonly UserManager<AppUser> _userManager;

        public TopMenuViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var username = User.Identity.Name;

                if (username != null)
                {
                    var user = _userManager.FindByNameAsync(username).Result;
                    return View(user);
                }
                return View(new AppUser());
            }
            catch (Exception ex)
            {
                return View(new AppUser());
            }
        }
    }
}
