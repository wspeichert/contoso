using System.Collections.Generic;
using Backend.Students.Data.Entities;
using SchoolData.Data.Entities;

namespace ContosoUniversity.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}

