using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class UserDto : UserListItemDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}

