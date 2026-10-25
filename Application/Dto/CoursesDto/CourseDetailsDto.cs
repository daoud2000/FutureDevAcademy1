using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.CoursesDto
{
    public  class CourseDetailsDto
    {
        public int Id { get; set; }
        public string Ttile { get; set; }
        public string Describtion { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string category { get; set; }
        public int TotalEnrollments { get; set; }
    }
}
