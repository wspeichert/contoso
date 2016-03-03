using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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

        [TestFixture]
        public class WhenGetingDetailsWithNullParameter
        {
            //this is an odd example...realisticly that paramater should not be a nullable int so this would never occur
            //but contoso does this....so to keep in line with the tutorial.....
            private CourseController sut;
            private HttpStatusCodeResult result;

            [SetUp]
            public void Given()
            {
                sut = new CourseController();
                //I dont actually need a data fake here, as the action quits before it is ever accessed
                sut.DataContext = () => null;
                result = sut.Details(null) as HttpStatusCodeResult;
            }

            [Test]
            public void ReturnsBadRequest()
            {
                var expectedResult = new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode;
                Assert.That(result.StatusCode, Is.EqualTo(expectedResult));
            }
        }

        [TestFixture]
        public class WhenGetingDetailsWithInvalidId
        {
            private CourseController sut;
            private HttpNotFoundResult result;

            [SetUp]
            public void Given()
            {
                sut = new CourseController();
                var fakeDb = A.Fake<SchoolContext>();
                A.CallTo(() => fakeDb.Courses.Find(A<int>.Ignored)).Returns(null);
                sut.DataContext = () => fakeDb;
                result = sut.Details(1) as HttpNotFoundResult;
            }

            [Test]
            public void ReturnsHttpNotFound()
            {
                var expectedResult = new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode;
                Assert.That(result.StatusCode, Is.EqualTo(expectedResult));
            }
        }

        [TestFixture]
        public class WhenGetingDetailsWithValidId
        {
            private CourseController sut;
            private ViewResult result;
            private IDataContext fakeDb;
            private Course expectedResult = new Course();
            //here we use a set integer for the course id, and use it explicitly to define the behavior of the fake
            //we dont want to actually test the .Find method, so if we set up the fake to work with only an 
            //input of this particular value, we can test that the correct parameter is passed in.
            //IE, if .Find is called with anything other than the courseID below, the test will fail.
            private int courseId = 1;
            [SetUp]
            public void Given()
            {
                sut = new CourseController();
                fakeDb = A.Fake<IDataContext>();
                A.CallTo(() => fakeDb.Courses.Find(courseId)).Returns(expectedResult);
                sut.DataContext = () => fakeDb;
                result = sut.Details(courseId) as ViewResult;
            }

            [Test]
            public void ReturnsViewByMvcNamingConventions()
            {
                Assert.That(result.ViewName, Is.EqualTo(string.Empty));
            }

            [Test]
            public void ReturnsCorrectCourse()
            {
                var model = result.Model;
                Assert.That(model, Is.EqualTo(expectedResult));
            }
        }
    }
}
