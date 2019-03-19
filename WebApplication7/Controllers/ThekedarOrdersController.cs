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
    public class ThekedarOrdersController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: ThekedarOrders
        public ActionResult Index()
        {
            var thekedarOrders = db.ThekedarOrders.Include(t => t.Stock).Include(t => t.Thekedar).Include(t => t.WeekNumber);
            return View(thekedarOrders.ToList());
        }

        // GET: ThekedarOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThekedarOrder thekedarOrder = db.ThekedarOrders.Find(id);
            if (thekedarOrder == null)
            {
                return HttpNotFound();
            }
            return View(thekedarOrder);
        }

        // GET: ThekedarOrders/Create
        public ActionResult Create()
        {
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name");
            ViewBag.ThekedarId = new SelectList(db.Thekedars, "Id", "Name");
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }

        // POST: ThekedarOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderNo,CustomerName,StockId,ThekedarId,TotalPrice,Quantity,OrderDate,DeliveryDate,Status,IsDelivered,Advance,RemainingBalance,WeekId")] ThekedarOrder thekedarOrder)
        {
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                db.ThekedarOrders.Add(thekedarOrder);
                db.SaveChanges();
                //ThekedarOrder//
                var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
                TotalRevenue tr = new TotalRevenue();
                tr.Name = thekedarOrder.CustomerName;
                tr.Add = thekedarOrder.Advance;
                tr.Less = 0;
                if (totalrev.CurrentBalance == null)
                {
                    tr.PreviousBalance = 0;
                    tr.CurrentBalance = 0;
                }
                else
                {
                    tr.PreviousBalance = totalrev.CurrentBalance;
                    tr.CurrentBalance = totalrev.CurrentBalance;
                }
                tr.CurrentBalance += thekedarOrder.Advance;
                tr.Date = DateTime.Now;
                tr.WeekId = thekedarOrder.WeekId;
                db.TotalRevenues.Add(tr);
                db.SaveChanges();
                dbTransaction.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name", thekedarOrder.StockId);
            ViewBag.ThekedarId = new SelectList(db.Thekedars, "Id", "Name", thekedarOrder.ThekedarId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo", thekedarOrder.WeekId);
            return View(thekedarOrder);
        }

        // GET: ThekedarOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThekedarOrder thekedarOrder = db.ThekedarOrders.Find(id);
            if (thekedarOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name", thekedarOrder.StockId);
            ViewBag.ThekedarId = new SelectList(db.Thekedars, "Id", "Name", thekedarOrder.ThekedarId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo", thekedarOrder.WeekId);
            return View(thekedarOrder);
        }

        // POST: ThekedarOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderNo,CustomerName,StockId,ThekedarId,TotalPrice,Quantity,OrderDate,DeliveryDate,Status,IsDelivered,Advance,RemainingBalance,WeekId")] ThekedarOrder thekedarOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thekedarOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name", thekedarOrder.StockId);
            ViewBag.ThekedarId = new SelectList(db.Thekedars, "Id", "Name", thekedarOrder.ThekedarId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo", thekedarOrder.WeekId);
            return View(thekedarOrder);
        }

        // GET: ThekedarOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThekedarOrder thekedarOrder = db.ThekedarOrders.Find(id);
            if (thekedarOrder == null)
            {
                return HttpNotFound();
            }
            return View(thekedarOrder);
        }

        // POST: ThekedarOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThekedarOrder thekedarOrder = db.ThekedarOrders.Find(id);
            db.ThekedarOrders.Remove(thekedarOrder);
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
