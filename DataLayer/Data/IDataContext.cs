using System.Data.Entity;
using System.Threading.Tasks;
using ITOps.Shared;
using SchoolData.Data.Entities;

namespace SchoolData.Data
{
    public interface IDataContext
    {
        IDbSet<Course> Courses { get; set; }
        IDbSet<Department> Departments { get; set; }
        IDbSet<Instructor> Instructors { get; set; }
        IDbSet<OfficeAssignment> OfficeAssignments { get; set; }
        IDbSet<Person> People { get; set; }
        IDbSet<InstructorCourse> InstructorCourses { get; set; }

        int SaveChanges();
        Task SaveChangesAsync();
        void Dispose();

        Database Database { get; }
     }
}