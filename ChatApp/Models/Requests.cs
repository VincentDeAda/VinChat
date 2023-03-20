namespace ChatApp.Models;

public record SignInRequest(string Username,string Password);
public record SignUpRequest(string Username, string Password);
public record MessageRequest(string Message);