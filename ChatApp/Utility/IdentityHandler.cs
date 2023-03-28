using ChatApp.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Utility;

public static class IdentityHandler
{
    public static ClaimsIdentity UserToIdentity(Identity user)
    {
        return new ClaimsIdentity(new List<Claim>() { 
        new Claim(ClaimTypes.NameIdentifier,user.User.Id.ToString()),
        new Claim(ClaimTypes.Name,user.User.Username),
        new Claim(ClaimTypes.Email,user.Email),

        }, "cookie");
    }
    public static string GenerateConfirmationSecretKey()
    {
        var noise = RandomNumberGenerator.GetBytes(64);
        StringBuilder base64 = new StringBuilder(Convert.ToBase64String(noise));
        var cleanBase64 = base64.Replace('/', '.').Replace('+', '$');
        return cleanBase64.ToString();
    }
}
