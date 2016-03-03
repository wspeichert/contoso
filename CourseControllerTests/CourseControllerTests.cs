using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using FakeItEasy;
using NUnit.Framework;

namespace CourseControllerTests
{
    public static class CourseControllerTests
    {
        [TestFixture]
        public class WhenGettingIndexWithNullParameter
        {
            private CourseController sut;
            private IQueryable<Department> departments;
            private FakeSchoolContext dbFake;
            private ViewResult result;

            [SetUp]
            public void Given()
            {
                sut = new CourseController();

                dbFake = new FakeSchoolContext
                {
                    Departments =
                    {
                        new Department {Name = "TestDept1", DepartmentID = 1},
                        new Department {Name = "TestDept2", DepartmentID = 2},
                        new Department {Name = "TestDept3", DepartmentID = 3}
                    },
                    Courses =
                    {
                        new Course {CourseID = 1, DepartmentID = 1, Title = "Test Course 1"},
                        new Course {CourseID = 2, Title = "Test Course 2" }
                    }
                };

                sut.DataContext = () => dbFake;
                result = sut.Index(null) as ViewResult;
            }

            [Test]
            public void ReturnsAllCourses()
            {
                var model = result.Model;
                Assert.That(model, Is.EquivalentTo(dbFake.Courses.AsQueryable()));
            }

            [Test]
            public void ReturnsViewByMvcNamingConventions()
            {
                Assert.That(result.ViewName, Is.EqualTo(string.Empty));
            }
        }

        [TestFixture]
        public class WhenGettingIndexWithValidDepartmentIdParameter
        {
            private CourseController sut;
            private IQueryable<Department> departments;
            private FakeSchoolContext dbFake;
            private ViewResult result;

            [SetUp]
            public void Given()
            {
                sut = new CourseController();

                dbFake = new FakeSchoolContext
                {
                    Departments =
                    {
                        new Department {Name = "TestDept1", DepartmentID = 1},
                        new Department {Name = "TestDept2", DepartmentID = 2},
                        new Department {Name = "TestDept3", DepartmentID = 3}
                    },
                    Courses =
                    {
                        new Course {CourseID = 1, DepartmentID = 1, Title = "Test Course 1"},
                        new Course {CourseID = 2, Title = "Test Course 2" }
                    }
                };

                sut.DataContext = () => dbFake;
                result = sut.Index(1) as ViewResult;
            }

            [Test]
            public void ReturnsOnlyCourseOne()
            {
                var model = result.Model;
                var expectedCourse = dbFake.Courses.Where(x => x.CourseID == 1);
                Assert.That(model, Is.EquivalentTo(expectedCourse));
            }

            [Test]
            public void ReturnsViewByMvcNamingConventions()
            {
                Assert.That(result.ViewName, Is.EqualTo(string.Empty));
            }
        }

        [TestFixture]
        public class WhenGettingIndexWithInvalidDepartmentIdParameter
        {
            private CourseController sut;
            private IQueryable<Department> departments;
            private FakeSchoolContext dbFake;
            private ViewResult result;

            [SetUp]
            public void Given()
            {
                sut = new CourseController();

                dbFake = new FakeSchoolContext
                {
                    Departments =
                    {
                        new Department {Name = "TestDept1", DepartmentID = 1},
                        new Department {Name = "TestDept2", DepartmentID = 2},
                        new Department {Name = "TestDept3", DepartmentID = 3}
                    },
                    Courses =
                    {
                        new Course {CourseID = 1, DepartmentID = 1, Title = "Test Course 1"},
                        new Course {CourseID = 2, Title = "Test Course 2" }
                    }
                };

                sut.DataContext = () => dbFake;
                result = sut.Index(76543) as ViewResult;
            }

            [Test]
            public void ReturnsOnlyCourseOne()
            {
                var model = result.Model;
                Assert.That(model, Is.EquivalentTo(new List<Course>()));
            }

            [Test]
            public void ReturnsViewByMvcNamingConventions()
            {
                Assert.That(result.ViewName, Is.EqualTo(string.Empty));
            }
        }
    }
}
