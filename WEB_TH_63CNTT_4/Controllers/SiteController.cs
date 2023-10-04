using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextBD.Model;


namespace WEB_TH_63CNTT_4.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext(); // taoj mowis Database
            
            int sodong = db.Products.Count();
            ViewBag.sodong = sodong;
            return View();
        }
        public ActionResult Text()
        {
            Mycontext myContext = new Mycontext(); // Tạo mới database
            int st = myContext.Menus.Count();  
            ViewBag.st = st;
            return View();
        }
    }
}