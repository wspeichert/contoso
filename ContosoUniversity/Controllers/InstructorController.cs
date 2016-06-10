﻿using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using SchoolData.Data;
using SchoolData.Data.Entities;
using WebGrease.Css.Extensions;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        #region DiConstructors        
        //Using Unity IoC to inject data context at runtime!
        private readonly IDataContext schoolContext;
        private readonly StudentsData.Data.IDataContext studentsContext;
        
        public InstructorController(IDataContext schoolContext, StudentsData.Data.IDataContext studentsContext)
        {
            this.schoolContext = schoolContext;
            this.studentsContext = studentsContext;
        }
        #endregion

        // GET: Instructor
        public ActionResult Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData
            {
                Instructors = schoolContext.Instructors
                    .OrderBy(i => i.LastName)
            };

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = schoolContext.Courses.Where(x => x.InstructorId == id);
            }

            if (courseId == null) return View(viewModel);

            ViewBag.CourseID = courseId.Value;
            // Lazy loading
            //viewModel.Enrollments = viewModel.Courses.Where(
            //    x => x.CourseID == courseID).Single().Enrollments;
            // Explicit loading
            var selectedCourse = viewModel.Courses.Single(x => x.CourseId == courseId);

            viewModel.Enrollments = studentsContext.Enrollments.Where(x => x.CourseId == selectedCourse.CourseId);

            return View(viewModel);
        }


        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructor = schoolContext.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        public ActionResult Create()
        {
            var model = new InstructorViewModel {Courses = new List<Course>()};
            PopulateAssignedCourseData(model);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstructorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            schoolContext.Instructors.Add(model.Instructor);
            schoolContext.SaveChanges();

            if (model.Courses == null) return RedirectToAction("Index");

            var instructorCourses =
                model.Courses.Select(x => new InstructorCourse {CourseId = x.CourseId, InstructorId = model.Instructor.Id}).ToList();
            instructorCourses.ForEach(x => schoolContext.InstructorCourses.Add(x));

            return RedirectToAction("Index");
        }


        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructor = schoolContext.Instructors.Single(i => i.Id == id);
            var model = new InstructorViewModel {Instructor = instructor};

            PopulateAssignedCourseData(model);
            if (model.Instructor == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        private void PopulateAssignedCourseData(InstructorViewModel model)
        {
            var allCourses = schoolContext.Courses;
            var instructorCourses = new HashSet<int>(model.Courses.Select(c => c.CourseId));
            var viewModel = allCourses.Select(course => new AssignedCourseData
            {
                CourseID = course.CourseId, Title = course.Title, Assigned = instructorCourses.Contains(course.CourseId)
            }).ToList();

            ViewBag.Courses = viewModel;
        }
        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstructorViewModel model, string[] selectedCourses)
        {
            schoolContext.Instructors.AddOrUpdate(model.Instructor);
            
            var existingCourses = schoolContext.Courses.Where(x => x.InstructorId == model.Instructor.Id).ToList();
            existingCourses.ForEach(x => x.InstructorId = 0);

            var courseHashes = new HashSet<string> (selectedCourses);
            var coursesToAssign = schoolContext.Courses.Where(x => courseHashes.Contains(x.CourseId.ToString()));
            coursesToAssign.ForEach(x => x.InstructorId = model.Instructor.Id);

            schoolContext.SaveChanges();

            return View(model);
        }
        
        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructor = schoolContext.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var instructor = schoolContext.Instructors.Single(i => i.Id == id);

            var officeAssignment = schoolContext.OfficeAssignments.Single(x => x.InstructorId == id);
            schoolContext.OfficeAssignments.Remove(officeAssignment);

            var instructorCourses = schoolContext.InstructorCourses.Where(x => x.InstructorId == id).ToList();
            instructorCourses.ForEach(x => schoolContext.InstructorCourses.Remove(x));
            
            schoolContext.Instructors.Remove(instructor);

            var department = schoolContext.Departments.SingleOrDefault(d => d.AdministratorInstructorId == id);

            if (department != null)
            {
                department.AdministratorInstructorId = null;
            }

            schoolContext.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                schoolContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
