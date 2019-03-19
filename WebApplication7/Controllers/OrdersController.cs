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
    public class OrdersController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Stock).Include(o => o.WeekNumber);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name");
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }
        public JsonResult FindOrderNo()
        {
            int No;
            int Order_No;
            int thekedarOrder_No;
            try
            {
                Order_No = (int)db.Orders.Select(x => x.OrderNo).Max();
                thekedarOrder_No = (int)db.ThekedarOrders.Select(x => x.OrderNo).Max();

                if (Order_No > thekedarOrder_No)
                {
                    Order_No++;
                    No = Order_No;
                }
                else
                {
                    thekedarOrder_No++;
                    No = thekedarOrder_No;
                }

            }
            catch
            {
                No = 1;
            }

            return Json(No, JsonRequestBehavior.AllowGet);

        }
        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderNo,CustomerName,StockId,TotalPrice,Quantity,OrderDate,DeliveryDate,Status,IsDelivered,Advance,RemainingBalance,WeekId")] Order order)
        {
           
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                var _status = Request.Form["status"];
                var _isdel = Request.Form["IsDelivered"];
                order.Status = _status;
                order.IsDelivered = _isdel;
                db.Orders.Add(order);
                db.SaveChanges();

                //Total Revenue//
              
                var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
                TotalRevenue tr = new TotalRevenue();
                tr.Name = order.CustomerName;
                tr.Add = order.Advance;
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
                tr.CurrentBalance += order.Advance;
                tr.Date = DateTime.Now;
                tr.WeekId = order.WeekId;
                db.TotalRevenues.Add(tr);
                db.SaveChanges();
                dbTransaction.Commit();
                return RedirectToAction("Index");
            }

                ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name", order.StockId);
                ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", order.WeekId);
                return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name", order.StockId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", order.WeekId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderNo,CustomerName,StockId,TotalPrice,Quantity,OrderDate,DeliveryDate,Status,IsDelivered,Advance,RemainingBalance,WeekId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "Name", order.StockId);
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "Id", order.WeekId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
