using System;
using System.Collections.Generic;
using SchoolData.Data;
using SchoolData.Data.Entities;

namespace DataLayer.Data
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var courses = new List<Course>
            {
            new Course{CourseId=1050,Title="Chemistry",Credits=3,},
            new Course{CourseId=4022,Title="Microeconomics",Credits=3,},
            new Course{CourseId=4041,Title="Macroeconomics",Credits=3,},
            new Course{CourseId=1045,Title="Calculus",Credits=4,},
            new Course{CourseId=3141,Title="Trigonometry",Credits=4,},
            new Course{CourseId=2021,Title="Composition",Credits=3,},
            new Course{CourseId=2042,Title="Literature",Credits=4,}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();
        }
    }
}