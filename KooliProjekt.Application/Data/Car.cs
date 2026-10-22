using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class Car
    {
        [Required]
        public int Id { get; set; }

        public string RegistrationNo { get; set; } = string.Empty;

        public int CarModelId { get; set; }

        public CarModel CarModel { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
