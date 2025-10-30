using Microsoft.AspNetCore.Identity;

namespace Sis_Inmobiliaria.Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? ProfileImage { get; set; }
    }
}
