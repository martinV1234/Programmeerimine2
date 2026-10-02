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

        public string LineItem { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [MaxLength(10)]
        public int Unit { get; set; }
        [Required]
        [MaxLength(10)]
        public int Quantity { get; set; }
        [Required]
        public int Total { get; set; }
        [Required]
        public int BookingId { get; set; }
    }
}
