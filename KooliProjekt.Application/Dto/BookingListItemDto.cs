using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class BookingListItemDto
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string RegistrationNo { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public string UserName { get; set; }
    }
}

