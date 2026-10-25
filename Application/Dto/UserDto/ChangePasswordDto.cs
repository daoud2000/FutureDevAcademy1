using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.UserDto
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
