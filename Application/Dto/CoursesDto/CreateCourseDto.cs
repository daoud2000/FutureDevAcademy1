using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dto.CoursesDto
{
    public  class CreateCourseDto
    {
        [Required]
        public string Ttile { get; set; }
        [Required]
        public string Describtion { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }

    }
}
