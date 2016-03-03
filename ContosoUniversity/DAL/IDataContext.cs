﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
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
        void Dispose();

        Database Database { get; }
    }
}