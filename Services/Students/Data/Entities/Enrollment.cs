using System.ComponentModel.DataAnnotations;
using StudentsData.Data.Entities;

namespace Backend.Students.Data.Entities
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public virtual Student Student { get; set; }
    }
}

