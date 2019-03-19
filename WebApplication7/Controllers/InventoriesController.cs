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
    public class InventoriesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: Inventories
        public ActionResult Index()
        {
            ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Name");
            ViewBag.WeekId = new SelectList(db.WeekNumbers, "Id", "WeekNo");
            return View();
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }
        public ActionResult GetAllInventories()
        {
            var inventorylist = db.Inventories.Select(x => new { x.Id, x.Name, x.Description }).ToList();
            var inven_rec_list = db.InventoryRecords.Select(x => new { x.Id, x.Inventory.Name, x.Quantity, x.Price, x.Date, x.WeekNumber.WeekNo, x.VenderName }).ToList();
            var totalinventory = db.TotalInventories.Select(x => new { x.Id, x.Inventory.Name, x.Quantity, x.Price, x.Date,}).ToList();
            var result = new { inventorylist, inven_rec_list, totalinventory };
            return Json(result,JsonRequestBehavior.AllowGet);
        }
     
        // GET: Inventories/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveInventoryType()
        {
            var name = Request.Form["FullName"];
            var desc = Request.Form["Description"];
            Inventory inventory = new Inventory();
            inventory.Name = name;
            inventory.Description = desc;
            db.Inventories.Add(inventory);
            db.SaveChanges();
            return RedirectToAction("Index","Inventories");
        }

        [HttpPost]
        public ActionResult SaveInventoryRecord()
        {
            var dbTransaction = db.Database.BeginTransaction();
            var inventory = Request.Form["InventoryId"];
            var qty = Request.Form["Quantity"];
            var price = Request.Form["Price"];
            var date = Request.Form["Date"];
            var vendername = Request.Form["VenderName"];
            var weekid = Request.Form["WeekId"];
            //Inventory Record//
            InventoryRecord inventoryrec = new  InventoryRecord();
            inventoryrec.InventoryId=Int32.Parse(inventory);
            inventoryrec.Quantity = Int32.Parse(qty);
            inventoryrec.Price = Int32.Parse(price);
            inventoryrec.Date = date;
            inventoryrec.VenderName = vendername;
            inventoryrec.WeekId =Int32.Parse(weekid);
            db.InventoryRecords.Add(inventoryrec);
            db.SaveChanges();
            //Total Inventory//
            var totalInventoryId = db.TotalInventories.Where(x => x.InventoryId == inventoryrec.InventoryId).Select(x => x.InventoryId).FirstOrDefault();
            if (totalInventoryId != null)
            {
                TotalInventory total_Inventory = db.TotalInventories.Where(x => x.InventoryId == totalInventoryId).FirstOrDefault();
                total_Inventory.Quantity += Int32.Parse(qty);
                total_Inventory.Price += Int32.Parse(price);
                db.SaveChanges();
            }
            else
            {
                TotalInventory totalInventory = new TotalInventory();
                totalInventory.InventoryId = Int32.Parse(inventory);
                totalInventory.Quantity = Int32.Parse(qty);
                totalInventory.Price = Int32.Parse(price);
                db.TotalInventories.Add(totalInventory);
                db.SaveChanges();
            }

            //Total Revenue//
            var totalrev = db.TotalRevenues.OrderByDescending(y => y.Id).FirstOrDefault();
            TotalRevenue tr = new TotalRevenue();
            var inventorytype = db.Inventories.Where(x => x.Id == inventoryrec.InventoryId).Select(x => x.Name).FirstOrDefault();
            tr.Name = inventorytype;
            tr.Add = 0;
            tr.Less = Int32.Parse(price);
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
            tr.CurrentBalance -= Int32.Parse(price);
            tr.Date = DateTime.Now;
            tr.WeekId = Int32.Parse(weekid);
            db.TotalRevenues.Add(tr);
            db.SaveChanges();
            dbTransaction.Commit();
            return RedirectToAction("Index", "Inventories");
        }
        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
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
