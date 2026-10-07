using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class InVoice
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsPaid { get; set; }

        public InvoiceLine InvoiceLine { get; set; }
    }
}
