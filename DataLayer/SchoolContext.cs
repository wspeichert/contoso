using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer
{
    public class SchoolContext : DbContext, IDataContext
    {
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Department> Departments { get; set; }
        public IDbSet<Enrollment> Enrollments { get; set; }
        public IDbSet<Instructor> Instructors { get; set; }
        public IDbSet<Student> Students { get; set; }
        public IDbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public IDbSet<Person> People { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                    .MapRightKey("InstructorID")
                    .ToTable("CourseInstructor"));

            modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        Task IDataContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}