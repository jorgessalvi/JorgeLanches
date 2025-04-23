using Microsoft.AspNetCore.Identity;

namespace JorgeLanches.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }
    }
}
