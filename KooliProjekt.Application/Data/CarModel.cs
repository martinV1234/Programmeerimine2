using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class CarModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public int CarManufacturerId { get; set; }
        [MaxLength(20)]
        public int KmRate { get; set; }
        [MaxLength(40)]
        public int HourlyRate { get; set; }

        public Car Car { get; set; }
    }
}
