using ChatApp.Models;
using System.Security.Claims;

namespace ChatApp.Utility;

public static class IdentityHandler
{
    public static ClaimsIdentity UserToIdentity(Identity user)
    {
        return new ClaimsIdentity(new List<Claim>() { 
        
        new Claim(ClaimTypes.NameIdentifier,user.User.Id.ToString()),
        new Claim(ClaimTypes.Name,user.User.Username),
        new Claim(ClaimTypes.Email,user.Email),
        
        },"cookie");
    }
}
