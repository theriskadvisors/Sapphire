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
    public class ProductionCategoriesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: ProductionCategories
        public ActionResult Index()
        {
            return View(db.ProductionCategories.ToList());
        }

        // GET: ProductionCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionCategory productionCategory = db.ProductionCategories.Find(id);
            if (productionCategory == null)
            {
                return HttpNotFound();
            }
            return View(productionCategory);
        }

        // GET: ProductionCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ProductionCategory productionCategory)
        {
            if (ModelState.IsValid)
            {
                db.ProductionCategories.Add(productionCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productionCategory);
        }

        // GET: ProductionCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionCategory productionCategory = db.ProductionCategories.Find(id);
            if (productionCategory == null)
            {
                return HttpNotFound();
            }
            return View(productionCategory);
        }

        // POST: ProductionCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ProductionCategory productionCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productionCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productionCategory);
        }

        // GET: ProductionCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionCategory productionCategory = db.ProductionCategories.Find(id);
            if (productionCategory == null)
            {
                return HttpNotFound();
            }
            return View(productionCategory);
        }

        // POST: ProductionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductionCategory productionCategory = db.ProductionCategories.Find(id);
            db.ProductionCategories.Remove(productionCategory);
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
