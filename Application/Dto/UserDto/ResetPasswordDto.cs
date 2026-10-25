using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.UserDto
{
    public class ResetPasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
