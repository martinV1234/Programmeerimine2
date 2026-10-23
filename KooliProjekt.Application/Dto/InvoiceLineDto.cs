using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceLineDto : InvoiceLineListItemDto
    {
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
    }
}

