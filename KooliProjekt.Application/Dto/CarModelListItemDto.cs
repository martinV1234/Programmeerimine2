using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class CarModelListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
        public decimal KmRate { get; set; }
        public decimal HourlyRate { get; set; }
    }
}

