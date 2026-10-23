using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class CarManufacturerDto : CarManufacturerListItemDto
    {
        public IList<CarModelListItemDto> Models { get; set; } = new List<CarModelListItemDto>();
    }
}

