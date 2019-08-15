﻿using System.ComponentModel.DataAnnotations;

namespace AbhCare.Identity.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
