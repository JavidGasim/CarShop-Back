﻿using System.ComponentModel.DataAnnotations;

namespace CarShop.WepApi.DTOS
{
    public class LoginDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
