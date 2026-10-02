using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public int RegistrationNo { get; set; }

        public int CarModelId { get; set; }

        public Booking Booking { get; set; }
    }
}
