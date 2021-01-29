using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace SourceCode.Custom
{
    public static class SeedUsers
    {
        // We created this class to add admin user in to database,
        // Identity Users can't be added using normal way like we used in ModelBuilder Extenstion class. 
        public static void AddUsers(UserManager<IdentityUser> _userManager , IConfiguration _config)
        {
            try
            {
                string AdminEmail = "webmaster@admin.com";

                string AdminPassword = "12345";


                if (_userManager.FindByEmailAsync(AdminEmail).Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = AdminEmail,
                        Email = AdminEmail
                    };

                    IdentityResult result = _userManager.CreateAsync(user, AdminPassword).Result;

                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, "Webmaster").Wait();
                    }
                }
            }
            catch (Exception ex)
            {
               string message=ex.Message;
            }

        }
    }
}
