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
    public class ExpenseDetailsController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: ExpenseDetails
        public ActionResult Index()
        {
            var expenseDetails = db.ExpenseDetails.Include(e => e.ExpenseType).Include(e => e.WeekNumber);
            return View(expenseDetails.ToList());
        }

        // GET: ExpenseDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseDetail expenseDetail = db.ExpenseDetails.Find(id);
            if (expenseDetail == null)
            {
                return HttpNotFound();
            }
            return View(expenseDetail);
        }

        // GET: ExpenseDetails/Create
        public ActionResult Create()
        {
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "Id", "Name");
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id");
            return View();
        }

        // POST: ExpenseDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ExpenseTypeId,Description,Amount,WeekId,Date")] ExpenseDetail expenseDetail)
        {
            if (ModelState.IsValid)
            {
                db.ExpenseDetails.Add(expenseDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "Id", "Name", expenseDetail.ExpenseTypeId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", expenseDetail.WeekId);
            return View(expenseDetail);
        }

        // GET: ExpenseDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseDetail expenseDetail = db.ExpenseDetails.Find(id);
            if (expenseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "Id", "Name", expenseDetail.ExpenseTypeId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", expenseDetail.WeekId);
            return View(expenseDetail);
        }

        // POST: ExpenseDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ExpenseTypeId,Description,Amount,WeekId,Date")] ExpenseDetail expenseDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes, "Id", "Name", expenseDetail.ExpenseTypeId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", expenseDetail.WeekId);
            return View(expenseDetail);
        }

        // GET: ExpenseDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseDetail expenseDetail = db.ExpenseDetails.Find(id);
            if (expenseDetail == null)
            {
                return HttpNotFound();
            }
            return View(expenseDetail);
        }

        // POST: ExpenseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpenseDetail expenseDetail = db.ExpenseDetails.Find(id);
            db.ExpenseDetails.Remove(expenseDetail);
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
