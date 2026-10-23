using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class BookingDto : BookingListItemDto
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public decimal StartKm { get; set; }
        public decimal FinishKm { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
    }
}

