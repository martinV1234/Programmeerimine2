using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class InvoiceLine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string LineItem { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "10000000")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(10)]
        public string Unit { get; set; }
        [Required]
        [Range(1, 1000000)]
        public int Quantity { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "10000000")]
        public decimal Total { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int BookingId { get; set; }
    }
}
