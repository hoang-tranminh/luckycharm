using log4net;
using LuckyCharm.DataAccess;
using LuckyCharm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace LuckyCharm.Busisness
{
    public class DataFetcherBase
    {
        private ILog _logger = LogManager.GetLogger(typeof(DataFetcherBase));

        protected Regex Special = new Regex(@"<td class=""giaidb"">\s+<div>(\d+)<\/div><\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex First = new Regex(@"<td class=""giai1"">\s+<div>(\d+)[^\d]*<\/div>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex Second = new Regex(@"<td class=""giai2"">\s+<div>(\d+)<\/div><div>(\d+)<\/div>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex Third = new Regex(@"<td class=""giai3"">\s+<div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex Fourth = new Regex(@"<td class=""giai4"">\s+<div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex Fifth = new Regex(@"<td class=""giai5"">\s+<div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex Sixth = new Regex(@"<td class=""giai6"">\s+<div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected Regex Seventh = new Regex(@"<td class=""giai7"">\s+<div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div><div>(\d+)<\/div>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected string URL = "http://www.minhngoc.net.vn/ket-qua-xo-so/mien-bac/{0}.html";

        protected string DateFormat = "dd-MM-yyyy";

        public void FetchData(DateTime fromDate, DateTime toDate)
        {
            DateTime now = toDate;

            var dbContext = new SxResultsContext();
            var count = 0;
            while (now >= fromDate)
            {
                count++;
                try
                {
                    var d = now.ToString(DateFormat);
                    WebRequest req = WebRequest.CreateHttp(string.Format(URL, d));   //WebRequest.CreateHttp("http://www.minhngoc.net.vn/ket-qua-xo-so/mien-bac/" + d + ".html");
                    req.Method = "GET";
                    var res = req.GetResponse();
                    var dailyItem = new DailyResult();
                    dailyItem.Date = now;
                    bool found = false;
                    using (var s = new StreamReader(res.GetResponseStream()))
                    {
                        found = ExtractAllValues(dailyItem, found, s);
                    }
                    if (found)
                    {
                        InsertOrUpdateItem(dbContext, dailyItem);
                    }
                    else
                    {
                        _logger.Error("Do not found (special) data on day:" + dailyItem.Date.ToShortDateString());
                    }
                    now = now.AddDays(-1);
                    if (count % 100 == 0)
                        dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            dbContext.SaveChanges();
        }

        private bool ExtractAllValues(DailyResult dailyItem, bool found, StreamReader s)
        {
            var result = s.ReadToEnd();
            var match = Special.Match(result);
            if (match.Success)
            {
                found = true;
                dailyItem.Special = int.Parse(match.Groups[1].Value);
            }
            match = First.Match(result);
            if (match.Success)
            {
                dailyItem.First = int.Parse(match.Groups[1].Value);
            }
            else
                _logger.Error("Do not found first data on day:" + dailyItem.Date.ToShortDateString());
            match = Second.Match(result);
            if (match.Success)
            {
                dailyItem.Second1 = int.Parse(match.Groups[1].Value);
                dailyItem.Second2 = int.Parse(match.Groups[2].Value);
            }
            else
                _logger.Error("Do not found second data on day:" + dailyItem.Date.ToShortDateString());
            match = Third.Match(result);
            if (match.Success)
            {
                dailyItem.Third1 = int.Parse(match.Groups[1].Value);
                dailyItem.Third2 = int.Parse(match.Groups[2].Value);
                dailyItem.Third3 = int.Parse(match.Groups[3].Value);
                dailyItem.Third4 = int.Parse(match.Groups[4].Value);
                dailyItem.Third5 = int.Parse(match.Groups[5].Value);
                dailyItem.Third6 = int.Parse(match.Groups[6].Value);
            }
            else
                _logger.Error("Do not found third data on day:" + dailyItem.Date.ToShortDateString());
            match = Fourth.Match(result);
            if (match.Success)
            {
                dailyItem.Fourth1 = int.Parse(match.Groups[1].Value);
                dailyItem.Fourth2 = int.Parse(match.Groups[2].Value);
                dailyItem.Fourth3 = int.Parse(match.Groups[3].Value);
                dailyItem.Fourth4 = int.Parse(match.Groups[4].Value);
            }
            else
                _logger.Error("Do not found fourth data on day:" + dailyItem.Date.ToShortDateString());
            match = Fifth.Match(result);
            if (match.Success)
            {
                dailyItem.Fifth1 = int.Parse(match.Groups[1].Value);
                dailyItem.Fifth2 = int.Parse(match.Groups[2].Value);
                dailyItem.Fifth3 = int.Parse(match.Groups[3].Value);
                dailyItem.Fifth4 = int.Parse(match.Groups[4].Value);
                dailyItem.Fifth5 = int.Parse(match.Groups[5].Value);
                dailyItem.Fifth6 = int.Parse(match.Groups[6].Value);
            }
            else
                _logger.Error("Do not found fifth data on day:" + dailyItem.Date.ToShortDateString());
            match = Sixth.Match(result);
            if (match.Success)
            {
                dailyItem.Sixth1 = int.Parse(match.Groups[1].Value);
                dailyItem.Sixth2 = int.Parse(match.Groups[2].Value);
                dailyItem.Sixth3 = int.Parse(match.Groups[3].Value);
            }
            else
                _logger.Error("Do not found sixth data on day:" + dailyItem.Date.ToShortDateString());
            match = Seventh.Match(result);
            if (match.Success)
            {
                dailyItem.Seventh1 = int.Parse(match.Groups[1].Value);
                dailyItem.Seventh2 = int.Parse(match.Groups[2].Value);
                dailyItem.Seventh3 = int.Parse(match.Groups[3].Value);
                dailyItem.Seventh4 = int.Parse(match.Groups[4].Value);
            }
            else
                _logger.Error("Do not found seventh data on day:" + dailyItem.Date.ToShortDateString());

            return found;
        }

        private void InsertOrUpdateItem(SxResultsContext dbContext, DailyResult dailyItem)
        {
            var item = dbContext.DailyResults.Where(r => r.Date == dailyItem.Date).FirstOrDefault();
            if (item == null)
            {
                //dont insert if 4 first value is all 0!
                if (dailyItem.Special != 0 || dailyItem.First != 0 || dailyItem.Second1 != 0 || dailyItem.Second2 != 0)
                    dbContext.DailyResults.Add(dailyItem);
                else
                    _logger.Error("All 4 first values are 0 on day:" + dailyItem.Date.ToShortDateString());
            }
            else
            {
                //update
                item.First = dailyItem.First;
                item.Second1 = dailyItem.Second1;
                item.Second2 = dailyItem.Second2;
                item.Sixth1 = dailyItem.Sixth1;
                item.Sixth2 = dailyItem.Sixth2;
                item.Sixth3 = dailyItem.Sixth3;
                item.Seventh1 = dailyItem.Seventh1;
                item.Seventh2 = dailyItem.Seventh2;
                item.Seventh3 = dailyItem.Seventh3;
                item.Seventh4 = dailyItem.Seventh4;
            }
        }
    }
}