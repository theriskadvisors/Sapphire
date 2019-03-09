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
    public class TotalRevenuesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: TotalRevenues
        public ActionResult Index()
        {
            var totalRevenues = db.TotalRevenues.Include(t => t.WeekNumber);
            return View(totalRevenues.ToList().OrderBy(x=>x.Id));
        }

        // GET: TotalRevenues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalRevenue totalRevenue = db.TotalRevenues.Find(id);
            if (totalRevenue == null)
            {
                return HttpNotFound();
            }
            return View(totalRevenue);
        }

        // GET: TotalRevenues/Create
        public ActionResult Create()
        {
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }

        // POST: TotalRevenues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Less,Add,PreviousBalance,CurrentBalance,WeekId,Date")] TotalRevenue totalRevenue)
        {
            if (ModelState.IsValid)
            {
                db.TotalRevenues.Add(totalRevenue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", totalRevenue.WeekId);
            return View(totalRevenue);
        }

        // GET: TotalRevenues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalRevenue totalRevenue = db.TotalRevenues.Find(id);
            if (totalRevenue == null)
            {
                return HttpNotFound();
            }
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", totalRevenue.WeekId);
            return View(totalRevenue);
        }

        // POST: TotalRevenues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Less,Add,PreviousBalance,CurrentBalance,WeekId,Date")] TotalRevenue totalRevenue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(totalRevenue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", totalRevenue.WeekId);
            return View(totalRevenue);
        }

        // GET: TotalRevenues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotalRevenue totalRevenue = db.TotalRevenues.Find(id);
            if (totalRevenue == null)
            {
                return HttpNotFound();
            }
            return View(totalRevenue);
        }

        // POST: TotalRevenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TotalRevenue totalRevenue = db.TotalRevenues.Find(id);
            db.TotalRevenues.Remove(totalRevenue);
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
