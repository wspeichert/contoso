using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
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
            _SaveChangesWasCalled = true;
            return 1;
        }

        public Task SaveChangesAsync()
        {
            _SaveChangesAsyncWasCalled = true;
            return null;
        }

        public void Dispose()
        {
            //do nothing
        }

        public Database Database { get; }

        private bool _SaveChangesWasCalled;
        private bool _SaveChangesAsyncWasCalled;
        public bool SaveChangesWasCalled => _SaveChangesWasCalled;
        public bool SaveChangesAsyncWasCalled => _SaveChangesAsyncWasCalled;
    }
}