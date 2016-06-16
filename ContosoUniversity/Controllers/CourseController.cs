using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.ViewModels;
using SchoolData.Data;
using SchoolData.Data.Entities;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        #region DiConstructors        
        //Using Unity IoC to inject data context at runtime!
        private readonly IDataContext schoolContext;
        public CourseController(IDataContext schoolContext)
        {
            this.schoolContext = schoolContext;
        }
        #endregion

        // GET: Course
        public ActionResult Index(int? selectedDepartment)
        {
            var departments = schoolContext.Departments.OrderBy(q => q.Name).ToList();

            ViewBag.SelectedDepartment = new SelectList(departments, "DepartmentID", "Name", selectedDepartment);
            var departmentId = selectedDepartment.GetValueOrDefault();

            var courses = schoolContext.Courses
                .Where(c => !selectedDepartment.HasValue || c.DepartmentId == departmentId)
                .GroupJoin(schoolContext.Departments, course => course.DepartmentId, dept => dept.DepartmentId, (course,dept) => new {course,dept})
                .SelectMany(x => x.dept.DefaultIfEmpty(), (x,dept) => new {x.course,dept})
                .ToList()
                .Select(x => new CourseData
                {
                    Course = x.course,
                    Department = x.dept
                })
                .OrderBy(d => d.Course.CourseId);

            return View(courses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = schoolContext.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var department = schoolContext.Departments.Find(course.DepartmentId);
            return View(new CourseData{Course = course, Department = department});
        }


        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")]Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    schoolContext.Courses.Add(course);
                    schoolContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = schoolContext.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = schoolContext.Courses.Find(id);
            if (TryUpdateModel(courseToUpdate, "",
               new[] { "Title", "Credits", "DepartmentID" }))
            {
                try
                {
                    schoolContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentId);
            return View(courseToUpdate);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in schoolContext.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }


        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = schoolContext.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var department = schoolContext.Departments.Find(course.DepartmentId);
            return View(new CourseData{Course = course, Department = department});
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = schoolContext.Courses.Find(id);
            schoolContext.Courses.Remove(course);
            schoolContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCourseCredits(int? multiplier)
        {
            if (multiplier != null)
            {
                ViewBag.RowsAffected = schoolContext.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
            }
            return View();
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
