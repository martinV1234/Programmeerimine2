using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class Booking
    {
        public int Id { get; set; }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public int StartKm { get; set; }

        public int FinishKm { get; set; }

        [MaxLength(20)]
        public int KmRate { get; set; }

        public int HourlyRate { get; set; }

        [Required]
        public int CarId { get; set; }
        [Required]
        public int UserId { get; set; }

        public InvoiceLine InvoiceLine { get; set; }
    }
}
