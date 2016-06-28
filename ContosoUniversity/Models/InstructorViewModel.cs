using System.Collections.Generic;
using Backend.Students.Data.Entities;
using ContosoUniversity.ViewModels;
using SchoolData.Data.Entities;

namespace ContosoUniversity.Models
{
    public class InstructorViewModel
    {
        public IEnumerable<InstructorData> Instructors { get; set; }
        public IEnumerable<CourseData> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}