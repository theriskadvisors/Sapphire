using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class TotalInventoryController : Controller
    {
        private SapphireDataBaseEntities db = new  SapphireDataBaseEntities();
        // GET: TotalInventory
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTotalInventory()
        {
            var list = db.TotalInventories.Select(x=>new {x.Id,x.Inventory.Name,x.Quantity,x.Price }).ToList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
    }
}