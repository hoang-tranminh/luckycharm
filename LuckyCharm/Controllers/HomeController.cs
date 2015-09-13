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

        public ActionResult FetchData()
        {
            ViewBag.Title = "Home Page";

            var date = new DateTime(2005, 5, 17);
            //new DataFetcher1().FetchData(date.AddDays(-200).Date, date);
            //new DataFetcher2().FetchData(date.AddDays(-200).Date, date);
            new DataFetcherBase().FetchData(date.AddDays(-300).Date, date);

            ViewBag.FetchedData = true;
            return View("Index");
        }
    }
}
