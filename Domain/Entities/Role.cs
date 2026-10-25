using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public  class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleEnum Code { get; set; }


    }
}
