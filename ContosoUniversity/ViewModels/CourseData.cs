using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolData.Data.Entities;

namespace ContosoUniversity.ViewModels
{
    public class CourseData
    {
        public Course Course { get; set; }
        public Department Department { get; set; }
    }
}