﻿using System.ComponentModel.DataAnnotations;

namespace AbhCare.Identity.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
