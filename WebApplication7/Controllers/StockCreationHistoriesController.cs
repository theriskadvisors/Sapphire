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
    public class StockCreationHistoriesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: StockCreationHistories
        public ActionResult Index()
        {
            ViewBag.Order = new SelectList(db.Orders, "Id", "OrderNo");
            var stockCreationHistories = db.StockCreationHistories.Include(s => s.Inventory).Include(s=>s.Order);
            return View(stockCreationHistories.ToList());
        }
        public ActionResult GetOrderedList(int orderno)
        {
            var result = db.StockCreationHistories.Where(x => x.OrderId == orderno).ToList();
            List<_stockCreation> stockcreation = new List<_stockCreation>();
            foreach (var item in result)
            {
                _stockCreation st = new _stockCreation();
                st.Id = item.Id;
                st.InventoryId = item.Inventory.Name;
                st.OrderId = item.Order.OrderNo;
                st.QuantityOfInventory = item.QuantityOfInventory;
                st.Date = item.Date;
                stockcreation.Add(st);

            }
            return Json(stockcreation,JsonRequestBehavior.AllowGet);
        }
        public class _stockCreation
        {
           
            public DateTime? Date { get; set; }
            public int? QuantityOfInventory { get; set; }
            public int Id { get; set; }
            public int? OrderId { get; set; }
            public string InventoryId { get; set; }

        }
        // GET: StockCreationHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockCreationHistory stockCreationHistory = db.StockCreationHistories.Find(id);
            if (stockCreationHistory == null)
            {
                return HttpNotFound();
            }
            return View(stockCreationHistory);
        }

        // GET: StockCreationHistories/Create
        public ActionResult Create()
        {
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name");
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "OrderNo");
            return View();
        }

        // POST: StockCreationHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderId,InventoryId,QuantityOfInventory,Date")] StockCreationHistory stockCreationHistory)
        {
            if (ModelState.IsValid)
            {
                db.StockCreationHistories.Add(stockCreationHistory);
                db.SaveChanges();

                //Less from Total Inventory//

                TotalInventory ti = db.TotalInventories.Where(x => x.InventoryId == stockCreationHistory.InventoryId).FirstOrDefault();
                ti.Quantity -= stockCreationHistory.QuantityOfInventory;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name", stockCreationHistory.InventoryId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "OrderNo", stockCreationHistory.OrderId);
            return View(stockCreationHistory);
        }

        // GET: StockCreationHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockCreationHistory stockCreationHistory = db.StockCreationHistories.Find(id);
            if (stockCreationHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name", stockCreationHistory.InventoryId);
            return View(stockCreationHistory);
        }

        // POST: StockCreationHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderId,InventoryId,QuantityOfInventory,Date")] StockCreationHistory stockCreationHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockCreationHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name", stockCreationHistory.InventoryId);
            return View(stockCreationHistory);
        }

        // GET: StockCreationHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockCreationHistory stockCreationHistory = db.StockCreationHistories.Find(id);
            if (stockCreationHistory == null)
            {
                return HttpNotFound();
            }
            return View(stockCreationHistory);
        }

        // POST: StockCreationHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockCreationHistory stockCreationHistory = db.StockCreationHistories.Find(id);
            db.StockCreationHistories.Remove(stockCreationHistory);
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
