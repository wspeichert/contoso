using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using ITOps.Shared;
using SchoolData.Data.Entities;

namespace SchoolData.Data
{
    public class SchoolContext : DbContext, IDataContext
    {
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Department> Departments { get; set; }
        public IDbSet<Instructor> Instructors { get; set; }
        public IDbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public IDbSet<Person> People { get; set; }
        public IDbSet<InstructorCourse> InstructorCourses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        Task IDataContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}