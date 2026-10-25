using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.CoursesDto
{
    public class CourseEnrollmentDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime Enrollmentdate { get; set; }
    }
}
