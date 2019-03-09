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
    public class ThekedarsController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: Thekedars
        public ActionResult Index()
        {
            return View(db.Thekedars.ToList());
        }

        // GET: Thekedars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thekedar thekedar = db.Thekedars.Find(id);
            if (thekedar == null)
            {
                return HttpNotFound();
            }
            return View(thekedar);
        }

        // GET: Thekedars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thekedars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PhoneNumber,Address,CNIC")] Thekedar thekedar)
        {
            if (ModelState.IsValid)
            {
                db.Thekedars.Add(thekedar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thekedar);
        }

        // GET: Thekedars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thekedar thekedar = db.Thekedars.Find(id);
            if (thekedar == null)
            {
                return HttpNotFound();
            }
            return View(thekedar);
        }

        // POST: Thekedars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PhoneNumber,Address,CNIC")] Thekedar thekedar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thekedar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thekedar);
        }

        // GET: Thekedars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thekedar thekedar = db.Thekedars.Find(id);
            if (thekedar == null)
            {
                return HttpNotFound();
            }
            return View(thekedar);
        }

        // POST: Thekedars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thekedar thekedar = db.Thekedars.Find(id);
            db.Thekedars.Remove(thekedar);
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
