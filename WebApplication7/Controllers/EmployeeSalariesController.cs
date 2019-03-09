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
    public class EmployeeSalariesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: EmployeeSalaries
        public ActionResult Index()
        {
            var employeeSalaries = db.EmployeeSalaries.Include(e => e.Employee);
            return View(employeeSalaries.ToList());
        }
        public JsonResult GetEmployees()
        {
            var employeelist = db.Employees.Select(x => x.Name).ToList();
            var weekno = db.WeekNumbers.Select(x => x.WeekNo).OrderByDescending(y => y).FirstOrDefault();
            var result = new { employeelist, weekno };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveSalary(SalaryDetail SalaryDetail)
        {
            var emp = db.Employees.Where(x => x.Name == SalaryDetail.Name).Select(x=>x.Id).FirstOrDefault();
            var wk = db.WeekNumbers.Where(x => x.WeekNo == SalaryDetail.Week).Select(x=>x.Id).FirstOrDefault();
            var sal = db.EmployeeSalaries.Where(x => x.EmployeeId == emp && x.WeekId == wk).FirstOrDefault();
            var result = "True";
            if(sal==null)
            {
                EmployeeSalary employeeSalary = new EmployeeSalary();
                employeeSalary.EmployeeId = emp;
                employeeSalary.WeekId = wk;
                employeeSalary.Hours = SalaryDetail.Hours;
                employeeSalary.HourlyRate = SalaryDetail.HourlyRate;
                employeeSalary.Date = SalaryDetail.Date;
                employeeSalary.Total = SalaryDetail.Total;
                db.EmployeeSalaries.Add(employeeSalary);
                db.SaveChanges();
            }
            else
            {
                result = "False";
            }
            return Json(result,JsonRequestBehavior.AllowGet);
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
        // GET: EmployeeSalaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            if (employeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(employeeSalary);
        }

        // GET: EmployeeSalaries/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        // POST: EmployeeSalaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Hours,HourlyRate,EmployeeId,WeekNo,Date,Total")] EmployeeSalary employeeSalary)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeSalaries.Add(employeeSalary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", employeeSalary.EmployeeId);
            return View(employeeSalary);
        }

        // GET: EmployeeSalaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            if (employeeSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", employeeSalary.EmployeeId);
            return View(employeeSalary);
        }

        // POST: EmployeeSalaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Hours,HourlyRate,EmployeeId,WeekNo,Date,Total")] EmployeeSalary employeeSalary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeSalary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "Name", employeeSalary.EmployeeId);
            return View(employeeSalary);
        }

        // GET: EmployeeSalaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            if (employeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(employeeSalary);
        }

        // POST: EmployeeSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeSalary employeeSalary = db.EmployeeSalaries.Find(id);
            db.EmployeeSalaries.Remove(employeeSalary);
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
