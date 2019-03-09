﻿using System;
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
    public class ExpenseTypesController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();

        // GET: ExpenseTypes
        public ActionResult Index()
        {
            return View(db.ExpenseTypes.ToList());
        }

        // GET: ExpenseTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
            if (expenseType == null)
            {
                return HttpNotFound();
            }
            return View(expenseType);
        }

        // GET: ExpenseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ExpenseType expenseType)
        {
            if (ModelState.IsValid)
            {
                db.ExpenseTypes.Add(expenseType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expenseType);
        }

        // GET: ExpenseTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
            if (expenseType == null)
            {
                return HttpNotFound();
            }
            return View(expenseType);
        }

        // POST: ExpenseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ExpenseType expenseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expenseType);
        }

        // GET: ExpenseTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
            if (expenseType == null)
            {
                return HttpNotFound();
            }
            return View(expenseType);
        }

        // POST: ExpenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
            db.ExpenseTypes.Remove(expenseType);
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
