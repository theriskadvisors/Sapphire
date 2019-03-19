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
    public class RevenuesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: Revenues
        public ActionResult Index()
        {
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            ViewBag.ExpenseId = new SelectList(db.ExpenseTypes, "Id", "Name");
            return View();
        }
        public ActionResult GetAllRevenues()
        {
            var revenue = db.Revenues.Select(x => new { x.Id, x.Name,x.Amount,x.WeekNumber.WeekNo }).ToList();
            var totalrevenue = db.TotalRevenues.Select(x => new { x.Id, x.Name, x.PreviousBalance, x.Less, x.Add, x.CurrentBalance, x.WeekNumber.WeekNo, x.Date }).ToList();
            var ExpenseType = db.ExpenseTypes.Select(x => new { x.Id, x.Name }).ToList();
            var ExpenseDetial = db.ExpenseDetails.Select(x => new { x.Id, x.ExpenseType.Name, x.Description, x.Amount, x.Date, x.WeekNumber.WeekNo }).ToList();
            var result = new { ExpenseDetial,ExpenseType,revenue, totalrevenue };
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveRevenue()
        {
            var dbTransaction = db.Database.BeginTransaction();
            var name = Request.Form["Name"];
            var amount = Request.Form["Amount"];
            var date = Request.Form["Date"];
            var weekid = Request.Form["WeekId"];

            //Revenue Type//
            Revenue rev = new Revenue();
            rev.Name = name;
            rev.Amount =Int32.Parse(amount);
            rev.Date = Convert.ToDateTime(date);
            rev.WeekId =Int32.Parse(weekid);
            db.Revenues.Add(rev);
            db.SaveChanges();

            //Total Revenue//

            TotalRevenue tr = new TotalRevenue();
            var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
            tr.Name =name;
            tr.Add = Int32.Parse(amount);
            tr.Less = 0;
            if (totalrev == null)
            {
                tr.PreviousBalance = 0;
                tr.CurrentBalance = 0;
            }
            else
            {
                tr.PreviousBalance = totalrev.CurrentBalance;
                tr.CurrentBalance = totalrev.CurrentBalance;
            }
            tr.CurrentBalance += Int32.Parse(amount);
            tr.Date = Convert.ToDateTime(date);
            tr.WeekId = Int32.Parse(weekid);
            db.TotalRevenues.Add(tr);
            db.SaveChanges();
            dbTransaction.Commit();

            return RedirectToAction("Index", "Revenues");
        }
        public ActionResult SaveExpenseType()
        {
            var name = Request.Form["Name"];
            ExpenseType exp = new  ExpenseType();
            exp.Name = name;
            db.ExpenseTypes.Add(exp);
            db.SaveChanges();
            return RedirectToAction("Index", "Revenues");
        }
        public ActionResult SaveExpenseDetails()
        {
            var dbTransaction = db.Database.BeginTransaction();
            var expenseId = Request.Form["ExpenseId"];
            var amount = Request.Form["Amount"];
            var Description = Request.Form["Description"];
            var date = Request.Form["Date"];
            var weekid = Request.Form["WeekId"];

            //Expense Details//
            var wid = Int32.Parse(weekid);
            var expdet = db.ExpenseDetails.Where(x => x.ExpenseType.Name == "Salary" && x.WeekId == wid).FirstOrDefault();
            if(expdet==null)
            {
                ExpenseDetail rev = new ExpenseDetail();
                rev.ExpenseTypeId = Int32.Parse(expenseId);
                rev.Amount = Int32.Parse(amount);
                rev.Date = Convert.ToDateTime(date);
                rev.WeekId = Int32.Parse(weekid);
                db.ExpenseDetails.Add(rev);
                db.SaveChanges();

                //Total Revenue//

                TotalRevenue tr = new TotalRevenue();
                var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
                var eid = Int32.Parse(expenseId);
                var expType = db.ExpenseTypes.Where(x => x.Id == eid).Select(x => x.Name).FirstOrDefault();
                tr.Name = expType;
                tr.Add = 0;
                tr.Less = Int32.Parse(amount); ;
                if (totalrev == null)
                {
                    tr.PreviousBalance = 0;
                    tr.CurrentBalance = 0;
                }
                else
                {
                    tr.PreviousBalance = totalrev.CurrentBalance;
                    tr.CurrentBalance = totalrev.CurrentBalance;
                }
                tr.CurrentBalance -= Int32.Parse(amount);
                tr.Date = Convert.ToDateTime(date);
                tr.WeekId = Int32.Parse(weekid);
                db.TotalRevenues.Add(tr);
                db.SaveChanges();
            }
            else
            {
                ExpenseDetail rev = db.ExpenseDetails.Where(x => x.ExpenseType.Name == "Salary" && x.WeekId == wid).FirstOrDefault();
                rev.Amount = Int32.Parse(amount);
                db.SaveChanges();

                //Total Revenue//
                TotalRevenue tr = db.TotalRevenues.Where(x => x.WeekId == wid && x.Name == "Salary").FirstOrDefault();
                tr.Less = Int32.Parse(amount); 
                tr.CurrentBalance -= Int32.Parse(amount);
                db.SaveChanges();
            }
           

           
            dbTransaction.Commit();


            return RedirectToAction("Index", "Revenues");
        }
        public ActionResult GetSalary(int week_id,int exp_type)
        {
            var expname = db.ExpenseTypes.Where(x => x.Id == exp_type).Select(x => x.Name).FirstOrDefault();
            if(expname=="Salary")
            {
                var t_sal = db.TotalSalaries.Where(x =>x.WeekId==week_id).Select(x => x.SalarySum).FirstOrDefault();
                if(t_sal==null)
                {
                    var t_res = "No";
                    return Json(t_res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(t_sal, JsonRequestBehavior.AllowGet);
                }
             
            }
            else
            {
                var result = "No";
                return Json(result, JsonRequestBehavior.AllowGet);
            }    
        }
        // GET: Revenues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revenue revenue = db.Revenues.Find(id);
            if (revenue == null)
            {
                return HttpNotFound();
            }
            return View(revenue);
        }

        // GET: Revenues/Create
        public ActionResult Create()
        {
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }

        // POST: Revenues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Amount,Date,WeekId")] Revenue revenue)
        {
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                db.Revenues.Add(revenue);
                db.SaveChanges();

                //Total Revenue//

                TotalRevenue tr = new TotalRevenue();
                var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
                tr.Name = revenue.Name;
                tr.Add = revenue.Amount;
                tr.Less = 0;
                if (totalrev == null)
                {
                    tr.PreviousBalance = 0;
                    tr.CurrentBalance = 0;
                }
                else
                {
                    tr.PreviousBalance = totalrev.CurrentBalance;
                    tr.CurrentBalance = totalrev.CurrentBalance;
                }
                tr.CurrentBalance +=revenue.Amount;
                tr.Date = revenue.Date;
                tr.WeekId = revenue.WeekId;
                db.TotalRevenues.Add(tr);
                db.SaveChanges();

                dbTransaction.Commit();
                return RedirectToAction("Index");
            }

            return View(revenue);
        }

        // GET: Revenues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revenue revenue = db.Revenues.Find(id);
            if (revenue == null)
            {
                return HttpNotFound();
            }
            return View(revenue);
        }

        // POST: Revenues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Amount,Date")] Revenue revenue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revenue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(revenue);
        }

        // GET: Revenues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revenue revenue = db.Revenues.Find(id);
            if (revenue == null)
            {
                return HttpNotFound();
            }
            return View(revenue);
        }

        // POST: Revenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Revenue revenue = db.Revenues.Find(id);
            db.Revenues.Remove(revenue);
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
