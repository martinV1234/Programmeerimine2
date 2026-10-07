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

        [Range(typeof(decimal), "0", "10000000")]
        public decimal StartKm { get; set; }

        [Range(typeof(decimal), "0", "10000000")]
        public decimal FinishKm { get; set; }

        [Range(typeof(decimal), "0", "1000000")]
        public decimal KmRate { get; set; }

        [Range(typeof(decimal), "0", "1000000")]
        public decimal HourlyRate { get; set; }

        [Required]
        public int CarId { get; set; }
        [Required]
        public int UserId { get; set; }

        public InvoiceLine InvoiceLine { get; set; }
    }
}
