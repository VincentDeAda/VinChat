﻿namespace ChatApp.Models;

public record MessageResponse (Guid Id, string Message, DateTime Date, AuthorResponse Author,DateTime? LastEditDate, bool IsEdited);
public record AuthorResponse(Guid Id, string Username);
public record UserInfoResponse(Guid Id, string Username);

