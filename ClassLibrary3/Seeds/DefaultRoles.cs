using Microsoft.AspNetCore.Identity;
using Sis_Inmobiliaria.Core.Domain.Common.Enums;

namespace Sis_Inmobiliaria.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.GeneralPublic.ToString()));
        }
    }
}
