﻿using Backend.Students.Data.Entities;
using ContosoUniversity.Controllers;
using CourseControllerTests.Infrastructure.Fakes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StudentsData.Data.Entities;

namespace CourseControllerTests.Controllers
{
    public static class StudentControllerTests
    {
        [TestFixture]
        public class WhenGettingIndexWithEmptySortOrder
        {
            private StudentController sut;
            private ViewResult result;

            [SetUp]
            public void Given()
            {
                var dataFake = new FakeSchoolContext {
                    Students = new FakeDbSet<Student>
                    {
                        new Student { LastName = "3" },
                        new Student { LastName = "1" },
                        new Student { LastName = "2" }
                    }
                };
                sut = new StudentController(dataFake);

                result = sut.Index(null, null, null, null);
            }

            [Test]
            public void ReturnsStudensSortedByLastNameAscending()
            {
                var model = (IEnumerable<Student>)result.Model;
                Assert.That(model.Select(x => x.LastName).ToList(), Is.EqualTo(new List<string> {"1", "2","3" }));
            }
        }

        [TestFixture]
        public class WhenGettingIndexWithNameDescendingSortOrder
        {
            private StudentController sut;
            private ViewResult result;

            [SetUp]
            public void Given()
            {
                var dataFake = new FakeSchoolContext
                {
                    Students = new FakeDbSet<Student>
                    {
                        new Student { LastName = "3" },
                        new Student { LastName = "1" },
                        new Student { LastName = "2" }
                    }
                };
                sut = new StudentController(dataFake);

                result = sut.Index("name_desc", null, null, null);
            }

            [Test]
            public void ReturnsStudensSortedByLastNameAscending()
            {
                var model = (IEnumerable<Student>)result.Model;
                Assert.That(model.Select(x => x.LastName).ToList(), Is.EqualTo(new List<string> { "3", "2", "1" }));
            }
        }
    }
}
