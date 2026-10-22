using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class InvoiceLine
    {
        public int Id { get; set; }

        public string LineItem { get; set; } = string.Empty;
        [Required]
        [Range(0, 100000)]
        [Precision(10, 2)]
        public decimal Price { get; set; }
        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 100000)]
        [Precision(10, 2)]
        public decimal Quantity { get; set; }
        [Required]
        [Range(0, 1000000)]
        [Precision(12, 2)]
        public decimal Total { get; set; }

        public int BookingId { get; set; }

        public Booking Booking { get; set; } = null!;
    }
}
