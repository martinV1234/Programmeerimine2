using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class CarModelDto : CarModelListItemDto
    {
        public int CarManufacturerId { get; set; }
        public int CarCount { get; set; }
    }
}

