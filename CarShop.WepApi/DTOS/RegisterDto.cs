﻿namespace CarShop.WepApi.DTOS
{
    public class RegisterDto
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName{ get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? ImagePath { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
