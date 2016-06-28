using System.Data.Entity;
using System.Threading.Tasks;
using Backend.Students.Data.Entities;
using StudentsData.Data.Entities;

namespace StudentsData.Data
{
    public interface IDataContext
    {
        IDbSet<Enrollment>  Enrollments { get; set; }
        IDbSet<Student> Students { get; set; }

        int SaveChanges();
        Task SaveChangesAsync();
        void Dispose();

        Database Database { get; }
     }
}