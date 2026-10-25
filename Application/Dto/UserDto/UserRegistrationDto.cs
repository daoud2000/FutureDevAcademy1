using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dto.UserDto
{
    public class UserRegistrationDto
    {

        [Required]
        public string FullName { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UniversityName { get; set; }
    }
}
