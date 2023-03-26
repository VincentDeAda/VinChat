using Bcrypt = BCrypt.Net.BCrypt;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Utility;

public class BcryptPasswordHasher : IPasswordHasher<Identity>
{
    public string HashPassword(Identity user, string password)
    {
        string salt = Bcrypt.GenerateSalt();
        string hashedPassword = Bcrypt.HashPassword(password, salt);
        user.HashedPassword = hashedPassword;
        user.Salt = salt;
        return hashedPassword;
    }

    public PasswordVerificationResult VerifyHashedPassword(Identity user, string hashedPassword, string providedPassword)
    {
        bool verfied = Bcrypt.Verify(providedPassword, hashedPassword);
        if (!verfied)
            return PasswordVerificationResult.Failed;
        return PasswordVerificationResult.Success;
    }
}
