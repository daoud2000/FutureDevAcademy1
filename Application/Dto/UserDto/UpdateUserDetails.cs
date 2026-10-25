using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.UserDto
{
    public class UpdateUserDetails
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string UniversityName { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
