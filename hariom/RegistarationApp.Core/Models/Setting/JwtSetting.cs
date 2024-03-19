﻿namespace RegistarationApp.Core.Models.Setting
{
    public class JwtSetting
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
        public string? ExpiresInMinutes { get; set; }
    }
}
