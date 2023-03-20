using ChatApp.Models;
using System.Security.Claims;

namespace ChatApp.Utility;

public static class IdentityHandler
{
    public static ClaimsIdentity UserToIdentity(User user)
    {
        return new ClaimsIdentity(new List<Claim>() { 
        
        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
        new Claim(ClaimTypes.Name,user.Username),
        
        },"cookie");
    }
}
