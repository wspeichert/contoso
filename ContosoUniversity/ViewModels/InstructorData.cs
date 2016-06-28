using System.Collections.Generic;
using Backend.Students.Data.Entities;
using SchoolData.Data.Entities;

namespace ContosoUniversity.ViewModels
{
    public class InstructorData
    {
        public Instructor Instructor { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public Enrollment Enrollments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}

