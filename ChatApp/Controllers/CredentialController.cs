﻿using ChatApp.Models;
using ChatApp.Services;
using ChatApp.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ChatApp.Controllers;
[Route("api/")]
[ApiController]
public class CredentialController : ControllerBase
{
    private readonly ChatContext _db;
    private readonly IPasswordHasher<Identity> _hasher;
    private readonly IEmailingService _emailing;
    private readonly IMapper _mapper;

    public CredentialController(ChatContext db, IPasswordHasher<Identity> hasher, IEmailingService emailing, IMapper mapper)
    {
        _db = db;
        _hasher = hasher;
        _emailing = emailing;
        _mapper = mapper;
    }
    [HttpGet("Verify"), Authorize]
    public IActionResult IsAuthenticated()
    {
        return Ok();
    }


    [HttpPost("SignIn"), AllowAnonymous]
    public async Task<IActionResult> Login(SignInRequest request)
    {
        var isEmail = new EmailAddressAttribute().IsValid(request.Username);
        Identity? identity;

        if (isEmail)
            identity = await _db.Identities.Include(x => x.User).FirstOrDefaultAsync(x => x.Email == request.Username.ToLower());
        else
        {
            var user = await _db.Users.Include(x => x.Identity).FirstOrDefaultAsync(x => x.Username == request.Username);
            identity = user?.Identity;
        }

        if (identity is default(Identity))
            return BadRequest("Invalid Credentials.");

        var passwordVerificationResult = _hasher.VerifyHashedPassword(identity, identity.HashedPassword, request.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return BadRequest("Invalid Credentials.");

        var claimIdentity = IdentityHandler.UserToIdentity(identity);
        var principle = new ClaimsPrincipal(claimIdentity);
        return SignIn(principle, "cookie");

    }


    [HttpPost("SignUp"), AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var searchedUsername = await _db.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

        if (searchedUsername is not default(User))
            return BadRequest("User Exist.");


        var mail = request.Email.ToLower();
        var isValidEmail = new EmailAddressAttribute().IsValid(mail);

        if (!isValidEmail)
            return BadRequest("Invalid Email Address");

        var searchedMail = await _db.Identities.FirstOrDefaultAsync(x => x.Email == mail);

        if (searchedMail is not default(Identity))
            return BadRequest("Email Already Used");

        var user = new User()
        {
            Username = request.Username,
            Identity = new Identity()
            {
                Email = mail
            }
        };
        _hasher.HashPassword(user.Identity, request.Password);
        var confirmEmail = new EmailConfirmation()
        {
            ConfirmationKey =IdentityHandler.GenerateConfirmationSecretKey(),
            IdentityUser = user.Identity,
            Email = mail,
        };

        await _db.AddAsync(user);
        await _db.AddAsync(confirmEmail);
        await _db.SaveChangesAsync();
        await _emailing.SendConfirmationEmail(confirmEmail);


        return Ok();
    }
    [HttpPost("SignOut"), Authorize]
    public Task<IActionResult> LogOut() => Task.FromResult<IActionResult>(SignOut("cookie"));

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
    {
        var confirmation = await _db.EmailConfirmations.Include(x => x.IdentityUser).FirstOrDefaultAsync(x => x.Id == request.Id);
        if (confirmation is default(EmailConfirmation)
            || confirmation.ExpirationDate < DateTime.UtcNow
            || confirmation.ConfirmationKey != request.Secretkey
            || confirmation.Success == true)
            return BadRequest("Invalid Request.");

        confirmation.Success = true;
        confirmation.IdentityUser.Email = confirmation.Email;
        confirmation.IdentityUser.EmailConfirmed = true;
        _db.Update(confirmation);
        await _db.SaveChangesAsync();
        return Ok();

    }
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
        var msgs = await _db.Messages.Include(x => x.Author).OrderByDescending(x => x.Date).Skip(skip).Take(50).Reverse().ToListAsync();
        if (msgs.Count == 0)
            return NoContent();
        return Ok(_mapper.MapMessages(msgs));
    }


}
