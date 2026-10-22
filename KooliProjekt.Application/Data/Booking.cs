using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public DateTime Started { get; set; }
        [Required]
        public DateTime Finished { get; set; }

        [Required]
        public decimal StartKm { get; set; }
        [Required]
        public decimal FinishKm { get; set; }

        [Range(0.01, 10)]
        [Precision(10, 2)]
        public decimal KmRate { get; set; }
        [Range(0.01, 100)]
        [Precision(10, 2)]
        public decimal HourlyRate { get; set; }
        [Required]
        public int CarId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public Car Car { get; set; } = null!;
        [Required]
        public User User { get; set; } = null!;

        public ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
    }
}
