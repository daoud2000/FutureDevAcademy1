using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public  class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course course { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student student { get; set; }
        public DateTime EnrollmentDate { get; set; }



    }
}
