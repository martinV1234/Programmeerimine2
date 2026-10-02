using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class CarManufacturer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public CarModel CarModel { get; set; }
    }
}
