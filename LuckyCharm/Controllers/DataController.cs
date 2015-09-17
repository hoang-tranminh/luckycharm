using LuckyCharm.Busisness;
using LuckyCharm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuckyCharm.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            var m = new DataAdminModel();
            var f = new DataFetcherBase();
            var a = new AnalysisTwoLastNumber();

            m.LatestAnalyzedDate = a.LatestAnalyzedDate;
            m.NumberOfRecords = f.NumberOfRecord;
            m.LatestRecordedDate = f.LatestRecordedDate;
            m.OldestRecordedDate = f.OldestRecordedDate;

            return View(m);
        }

        public ActionResult FetchData(DateTime date, int count)
        {
            ViewBag.Title = "Data Admin Page";
            //var date1 = new DateTime(2015, 12, 14);
            //new DataFetcher1().FetchData(date.AddDays(-200).Date, date);
            //new DataFetcher2().FetchData(date.AddDays(-200).Date, date);
            new DataFetcherBase().FetchData(date.AddDays(-count).Date, date);

            //new DataFetcherBase().FetchData(date1.AddDays(-320).Date, date1);

            return Redirect(string.Format("/{0}/Index?fetchedDate{1}&dateCount={2}", ControllerContext.RouteData.Values["controller"], date, count));
        }

        public ActionResult AnalyseData()
        {
            ViewBag.Title = "Home Page";

            var a = new AnalysisTwoLastNumber();
            a.BuildSourceToAnalyse();
            a.AnalyseData();

            return Redirect(string.Format("/{0}/Index?analyzed={1}", ControllerContext.RouteData.Values["controller"], true));
        }
    }
}