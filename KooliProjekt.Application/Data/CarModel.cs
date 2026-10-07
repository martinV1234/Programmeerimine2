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

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int CarManufacturerId { get; set; }
        [Range(typeof(decimal), "0", "1000000")]
        public decimal KmRate { get; set; }
        [Range(typeof(decimal), "0", "1000000")]
        public decimal HourlyRate { get; set; }

        public Car Car { get; set; }
    }
}
