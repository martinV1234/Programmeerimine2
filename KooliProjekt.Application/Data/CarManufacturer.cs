using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class CarManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
    }
}
