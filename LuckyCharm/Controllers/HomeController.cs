using LuckyCharm.Busisness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LuckyCharm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        

        public ActionResult GetNumbersWithLongestLength(int? count=20)
        {
            ViewBag.Title = "Home Page";

            var a = new AnalysisTwoLastNumber();

            ViewBag.NumbersWithLongestLength = a.GetNumbersWithLongestLength(count.HasValue ? count.Value : 20);

            return View("Index");
        }

    }
}
