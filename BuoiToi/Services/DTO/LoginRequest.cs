﻿namespace BuoiToi.Services;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }
}