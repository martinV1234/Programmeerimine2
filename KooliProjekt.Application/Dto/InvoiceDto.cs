using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDto : InvoiceListItemDto
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public decimal Total { get; set; }
        public IList<InvoiceLineListItemDto> Lines { get; set; } = new List<InvoiceLineListItemDto>();
    }
}

