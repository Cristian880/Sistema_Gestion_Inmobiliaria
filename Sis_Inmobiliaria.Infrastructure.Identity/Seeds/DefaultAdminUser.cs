using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sis_Inmobiliaria.Core.Domain.Common.Enums;
using Sis_Inmobiliaria.Infrastructure.Identity.Entities;


namespace Sis_Inmobiliaria.Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                Name = "Ricardo",
                LastName = "Acosta",
                Email = "addmin@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "admin"
            };

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if (entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }

        }
    }
}
