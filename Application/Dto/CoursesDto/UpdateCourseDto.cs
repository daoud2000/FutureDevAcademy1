using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.Dto.CoursesDto
{
    public  class UpdateCourseDto
    {
        public string Ttile { get; set; }
        public string Describtion { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       
        public string Category { get; set; }
    }
}
