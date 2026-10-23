using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceListItemDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public string UserName { get; set; }
    }
}

