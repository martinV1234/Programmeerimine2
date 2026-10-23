using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class CarDto : CarListItemDto
    {
        public int CarModelId { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
    }
}

