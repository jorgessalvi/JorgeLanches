﻿using System.ComponentModel.DataAnnotations;

namespace JorgeLanches.DTOs
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage ="Username is required.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage ="Password is required.")]
        public string? Password { get; set; }
    }
}
