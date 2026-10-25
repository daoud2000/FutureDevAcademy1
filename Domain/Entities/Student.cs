using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        
        public string UniversityName { get; set; }

        public DateTime Birthdate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User userId { get; set; }

       

    }
}
