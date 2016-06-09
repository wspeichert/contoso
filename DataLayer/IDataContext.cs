using System.Data.Entity;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer
{
    public interface IDataContext
    {
        IDbSet<Course> Courses { get; set; }
        IDbSet<Department> Departments { get; set; }
        IDbSet<Enrollment>  Enrollments { get; set; }
        IDbSet<Instructor> Instructors { get; set; }
        IDbSet<Student> Students { get; set; }
        IDbSet<OfficeAssignment> OfficeAssignments { get; set; }
        IDbSet<Person> People { get; set; }

        int SaveChanges();
        Task SaveChangesAsync();
        void Dispose();

        Database Database { get; }
     }
}