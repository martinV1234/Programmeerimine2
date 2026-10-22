using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class CarModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CarManufacturerId { get; set; }

        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }

        public CarManufacturer CarManufacturer { get; set; } = null!;
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
