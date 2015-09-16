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

        public ActionResult FetchData(DateTime date, int count)
        {
            ViewBag.Title = "Home Page";
            //var date1 = new DateTime(2015, 12, 14);
            //new DataFetcher1().FetchData(date.AddDays(-200).Date, date);
            //new DataFetcher2().FetchData(date.AddDays(-200).Date, date);
            new DataFetcherBase().FetchData(date.AddDays(-count).Date, date);

            //new DataFetcherBase().FetchData(date1.AddDays(-320).Date, date1);

            ViewBag.FetchedData = true;
            return View("Index");
        }

        public ActionResult AnalyseData()
        {
            ViewBag.Title = "Home Page";

            var a = new AnalysisTwoLastNumber();
            a.BuildSourceToAnalyse();
            a.AnalyseData();

            ViewBag.AnalysedData = true;
            return View("Index");
        }

        public ActionResult GetNumbersWithLongestLength(int? count=20)
        {
            ViewBag.Title = "Home Page";

            var a = new AnalysisTwoLastNumber();

            ViewBag.Datas = a.GetNumbersWithLongestLength(count.HasValue ? count.Value : 20);
            return View("Index");
        }

    }
}
