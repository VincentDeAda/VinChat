using ChatApp.Models;
using ChatApp.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace ChatApp.Controllers;
[Route("api/")]
[ApiController]
public class CredentialController : ControllerBase
{
    private readonly ChatContext _db;
    private readonly IPasswordHasher<User> _hasher;

    public CredentialController(ChatContext db, IPasswordHasher<User> hasher)
    {
        _db = db;
        _hasher = hasher;
    }
    [HttpGet("Verify"), Authorize]
    public IActionResult IsAuthenticated()
    {
        return Ok();
    }
    [HttpPost("SignIn"), AllowAnonymous]
    public async Task<IActionResult> Login(SignInRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
        if (user is default(User))
            return BadRequest("Invalid Credentials.");

        var passwordVerificationResult = _hasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return BadRequest("Invalid Credentials.");

        var claimIdentity = IdentityHandler.UserToIdentity(user);
        var principle = new ClaimsPrincipal(claimIdentity);
        return SignIn(principle, "cookie");
    }


    [HttpPost("SignUp"), AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
        if (user is not default(User))
            return BadRequest("User Exist.");



        user = new User()
        {
            Username = request.Username,
        };
        _hasher.HashPassword(user, request.Password);
        await _db.AddAsync(user);
        await _db.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("SignOut"), Authorize]
    public Task<IActionResult> LogOut() => Task.FromResult<IActionResult>(SignOut("cookie"));


    [HttpGet("FetchUserInfo"), Authorize]
    public Task<IActionResult> FetchUserInfo()
    {
        var id = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)!;
        var userinfo = new UserInfoResponse(id, username);
        return Task.FromResult<IActionResult>(Ok(userinfo));
    }

    [HttpGet("FetchMessages"), Authorize]
    public async Task<IActionResult> FetchMessages([FromQuery] int skip = 0)
    {
        var msgs = await _db.Messages.Include(x => x.Author).OrderByDescending(x => x.Date).Where(x => !x.Removed).Skip(skip).Take(50).Select(x => new MessageResponse(x.Id, x.MessageContent, x.Date, new AuthorResponse(x.Author.Id, x.Author.Username))).Reverse().ToListAsync();
        if (msgs.Count == 0)
            return NoContent();
        return Ok(msgs);
    }


}
