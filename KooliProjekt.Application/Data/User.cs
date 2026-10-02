using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public int PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public InVoice InVoice { get; set; }

        public Booking Booking { get; set; }
    }
}
