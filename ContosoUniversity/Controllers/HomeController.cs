using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.ViewModels;
using DataLayer;


namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        #region DiConstructors        
        //Using Unity IoC to inject data context at runtime!
        private readonly IDataContext db;       
        public HomeController(IDataContext db)
        {
            this.db = db;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            // Commenting out LINQ to show how to do the same thing in SQL.
            //IQueryable<EnrollmentDateGroup> = from student in db.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };

            // SQL version of the above LINQ code.
            const string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                                 + "FROM Person "
                                 + "WHERE Discriminator = 'Student' "
                                 + "GROUP BY EnrollmentDate";
            IEnumerable<EnrollmentDateGroup> data = db.Database.SqlQuery<EnrollmentDateGroup>(query);

            return View(data.ToList());
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}