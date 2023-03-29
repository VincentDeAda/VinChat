namespace ChatApp.Models;

public record SignInRequest(string Username, string Password);
public record SignUpRequest(string Email, string Username, string Password);
public record MessageRequest(string Message);
public record ConfirmEmailRequest(Guid Id, string Secretkey);
