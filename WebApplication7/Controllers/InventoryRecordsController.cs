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
    public class InventoryRecordsController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: InventoryRecords
        public ActionResult Index()
        {
            var inventoryRecords = db.InventoryRecords.Include(i => i.Inventory);
            return View(inventoryRecords.ToList());
        }

        // GET: InventoryRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryRecord inventoryRecord = db.InventoryRecords.Find(id);
            if (inventoryRecord == null)
            {
                return HttpNotFound();
            }
            return View(inventoryRecord);
        }

        // GET: InventoryRecords/Create
        public ActionResult Create()
        {
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name");
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }

        // POST: InventoryRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InventoryId,Quantity,Price,Date,WeekId")] InventoryRecord inventoryRecord)
        {
            if (ModelState.IsValid)
            {
                var dbTransaction = db.Database.BeginTransaction();
                db.InventoryRecords.Add(inventoryRecord);
                db.SaveChanges();
                var totalInventoryId = db.TotalInventories.Where(x => x.InventoryId == inventoryRecord.InventoryId).Select(x => x.InventoryId).FirstOrDefault();
                if(totalInventoryId != null)
                {
                    TotalInventory total_Inventory = db.TotalInventories.Where(x => x.InventoryId == totalInventoryId).FirstOrDefault();
                    total_Inventory.Quantity += inventoryRecord.Quantity;
                    total_Inventory.Price += inventoryRecord.Price;
                    db.SaveChanges();
                }
                else
                {
                    TotalInventory totalInventory = new TotalInventory();
                    totalInventory.InventoryId = inventoryRecord.InventoryId;
                    totalInventory.Quantity = inventoryRecord.Quantity;
                    totalInventory.Price = inventoryRecord.Price;
                    db.TotalInventories.Add(totalInventory);
                    db.SaveChanges();
                }

                var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
                TotalRevenue tr = new TotalRevenue();
                var inventorytype = db.Inventories.Where(x => x.Id == inventoryRecord.InventoryId).Select(x=>x.Name).FirstOrDefault();
                tr.Name = inventorytype;
                tr.Add = 0;
                tr.Less = inventoryRecord.Price;
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
                tr.CurrentBalance -= inventoryRecord.Price;
                tr.Date = DateTime.Now;
                tr.WeekId = inventoryRecord.WeekId;
                db.TotalRevenues.Add(tr);
                db.SaveChanges();


                dbTransaction.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name", inventoryRecord.InventoryId);
            return View(inventoryRecord);
        }

        // GET: InventoryRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryRecord inventoryRecord = db.InventoryRecords.Find(id);
            if (inventoryRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name", inventoryRecord.InventoryId);
            return View(inventoryRecord);
        }

        // POST: InventoryRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InventoryId,Quantity,Price,Date")] InventoryRecord inventoryRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventoryRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name", inventoryRecord.InventoryId);
            return View(inventoryRecord);
        }

        // GET: InventoryRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryRecord inventoryRecord = db.InventoryRecords.Find(id);
            if (inventoryRecord == null)
            {
                return HttpNotFound();
            }
            return View(inventoryRecord);
        }

        // POST: InventoryRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryRecord inventoryRecord = db.InventoryRecords.Find(id);
            db.InventoryRecords.Remove(inventoryRecord);
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
