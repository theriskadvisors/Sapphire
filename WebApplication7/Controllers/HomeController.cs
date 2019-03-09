using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        private SapphireDataBaseEntities db = new SapphireDataBaseEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SaveWeek(string weekno)
        {
            var wno= Int32.Parse(weekno);
            var _week = db.WeekNumbers.Where(x => x.WeekNo == wno).FirstOrDefault();
            var result = "True";
            if(_week==null)
            {
                WeekNumber wn = new WeekNumber();
                wn.WeekNo = wno;
                db.WeekNumbers.Add(wn);
                db.SaveChanges();
            }
            else
            {
                result = "False";
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWeek()
        {
            var weekno = db.WeekNumbers.Select(x=>x.WeekNo).OrderByDescending(y=>y).FirstOrDefault();
            return Json(weekno,JsonRequestBehavior.AllowGet);
        }
    }
}