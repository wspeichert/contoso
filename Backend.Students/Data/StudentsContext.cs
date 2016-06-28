using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using Backend.Students.Data.Entities;
using StudentsData.Data.Entities;

namespace StudentsData.Data
{
    public class StudentsContext : DbContext, IDataContext
    {
        public IDbSet<Enrollment> Enrollments { get; set; }
        public IDbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        Task IDataContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}