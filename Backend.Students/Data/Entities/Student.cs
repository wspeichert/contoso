using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Students.Data.Entities;
using ITOps.Shared;

namespace StudentsData.Data.Entities
{
    public class Student : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}