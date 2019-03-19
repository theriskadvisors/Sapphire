using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class ThekedarsController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: Thekedars
        public ActionResult Index()
        {
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }
        public ActionResult GetAllEmployees()
        {
            var thekedarlist = db.Thekedars.Select(x => new { x.Id, x.Name, x.PhoneNumber, x.CNIC, x.Address }).ToList();
            var Employeelist = db.Employees.Select(x => new { x.Id, x.Name, x.PhoneNumber, x.CNIC, x.Address }).ToList();
            var Salarylist = db.EmployeeSalaries.Select(x => new { x.Id, x.Employee.Name, x.Total, x.Date, x.WeekNumber.WeekNo }).ToList();
            /////EmployeeSalary List
            var employeenames = db.Employees.Select(x => x.Name).ToList();
            var weekno = db.WeekNumbers.Select(x => x.WeekNo).OrderByDescending(y => y).FirstOrDefault();
            var result = new { thekedarlist, Employeelist, Salarylist, employeenames, weekno };
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        // GET: Thekedars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thekedar thekedar = db.Thekedars.Find(id);
            if (thekedar == null)
            {
                return HttpNotFound();
            }
            return View(thekedar);
        }

        // GET: Thekedars/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployees()
        {
            var name = Request.Form["FullName"];
            var phone = Request.Form["Phone"];
            var cnic = Request.Form["CNIC"];
            var address = Request.Form["Address"];

            Employee employee = new Employee();
            employee.Name = name;
            employee.PhoneNumber = phone;
            employee.CNIC = cnic;
            employee.Address = address;
            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("Index", "Thekedars");
        }
        [HttpPost]
        public ActionResult SaveThekedars()
        {
            var name = Request.Form["FullName"];
            var phone = Request.Form["Phone"];
            var cnic = Request.Form["CNIC"];
            var address = Request.Form["Address"];

            Thekedar thekedar = new Thekedar();
            thekedar.Name = name;
            thekedar.PhoneNumber = phone;
            thekedar.CNIC = cnic;
            thekedar.Address = address;
            db.Thekedars.Add(thekedar);
            db.SaveChanges();
            return RedirectToAction("Index", "Thekedars");
        }
        public JsonResult SaveSalary(SalaryDetail SalaryDetail)
        {
            var emp = db.Employees.Where(x => x.Name == SalaryDetail.Name).Select(x => x.Id).FirstOrDefault();
            var sal = db.EmployeeSalaries.Where(x => x.EmployeeId == emp && x.WeekId == SalaryDetail.Week).FirstOrDefault();
            var result = "True";
            if (sal == null)
            {
                EmployeeSalary employeeSalary = new EmployeeSalary();
                employeeSalary.EmployeeId = emp;
                employeeSalary.WeekId = SalaryDetail.Week;
                employeeSalary.Hours = SalaryDetail.Hours;
                employeeSalary.HourlyRate = SalaryDetail.HourlyRate;
                employeeSalary.Date = SalaryDetail.Date;
                employeeSalary.Total = SalaryDetail.Total;
                db.EmployeeSalaries.Add(employeeSalary);
                db.SaveChanges();

                var flag = db.TotalSalaries.Where(x => x.WeekId == SalaryDetail.Week).FirstOrDefault();
                if(flag==null)
                {
                    TotalSalary TotalSalary = new TotalSalary();
                    TotalSalary.SalarySum = SalaryDetail.Total;
                    TotalSalary.WeekId = SalaryDetail.Week;
                    db.TotalSalaries.Add(TotalSalary);
                    db.SaveChanges();
                }
                else
                {
                    TotalSalary TotalSalary = db.TotalSalaries.Where(x => x.WeekId == SalaryDetail.Week).FirstOrDefault();
                    TotalSalary.SalarySum += SalaryDetail.Total;
                    db.SaveChanges();
                }
               

            }
            else
            {
                result = "False";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public class SalaryDetail
        {
            public string Name { get; set; }
            public int Hours { get; set; }
            public string HourlyRate { get; set; }
            public DateTime Date { get; set; }
            public int Week { get; set; }
            public int Total { get; set; }

        }
        // POST: Thekedars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PhoneNumber,Address,CNIC")] Thekedar thekedar)
        {
            if (ModelState.IsValid)
            {
                db.Thekedars.Add(thekedar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thekedar);
        }

        // GET: Thekedars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thekedar thekedar = db.Thekedars.Find(id);
            if (thekedar == null)
            {
                return HttpNotFound();
            }
            return View(thekedar);
        }

        // POST: Thekedars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhoneNumber,Address,CNIC")] Thekedar thekedar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thekedar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thekedar);
        }

        // GET: Thekedars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thekedar thekedar = db.Thekedars.Find(id);
            if (thekedar == null)
            {
                return HttpNotFound();
            }
            return View(thekedar);
        }

        // POST: Thekedars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thekedar thekedar = db.Thekedars.Find(id);
            db.Thekedars.Remove(thekedar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
