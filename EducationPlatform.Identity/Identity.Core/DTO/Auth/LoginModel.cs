﻿namespace Identity.Core.DTO.Auth
{
    public class LoginModel
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}