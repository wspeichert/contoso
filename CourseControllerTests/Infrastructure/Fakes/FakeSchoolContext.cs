using System.Data.Entity;
using System.Threading.Tasks;
using DataLayer.Data;
using DataLayer.Data.Entities;

namespace CourseControllerTests.Infrastructure.Fakes
{
    public class FakeSchoolContext : IDataContext
    {
        public FakeSchoolContext()
        {
            Courses = new FakeDbSet<Course>();
            Departments = new FakeDbSet<Department>();
            Enrollments = new FakeDbSet<Enrollment>();
            Instructors = new FakeDbSet<Instructor>();
            Students = new FakeDbSet<Student>();
            OfficeAssignments = new FakeDbSet<OfficeAssignment>();
            People = new FakeDbSet<Person>();
        }
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Department> Departments { get; set; }
        public IDbSet<Enrollment> Enrollments { get; set; }
        public IDbSet<Instructor> Instructors { get; set; }
        public IDbSet<Student> Students { get; set; }
        public IDbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public IDbSet<Person> People { get; set; }
        public int SaveChanges()
        {
            SaveChangesWasCalled = true;
            return 1;
        }

        public Task SaveChangesAsync()
        {
            SaveChangesAsyncWasCalled = true;
            return null;
        }

        public void Dispose()
        {
            //do nothing
        }

        public Database Database { get; set; }

        public bool SaveChangesWasCalled { get; private set; }
        public bool SaveChangesAsyncWasCalled { get; private set; }
    }
}