using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Ttile { get; set; }
        public string Describtion { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category category  { get; set; }

    }
}
