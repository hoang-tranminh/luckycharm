using LuckyCharm.DataAccess;
using LuckyCharm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyCharm.Busisness
{
    public class AnalysisTwoLastNumber
    {
        private Dictionary<DateTime, string> _dataToAnalyse = new Dictionary<DateTime, string>();

        private SxResultsContext context = null;

        public SxResultsContext Context
        {
            get
            {
                if (context == null)
                    context = new SxResultsContext();
                return context;
            }
        }

        public void BuildSourceToAnalyse()
        {
            var skip = 0;
            var items = Context.DailyResults.OrderBy(k => k.Date).ToList().Select(item => new KeyValuePair<DateTime, int>(item.Date, item.Special));
            while (items.Count() > 0)
            {
                foreach (var i in items)
                {
                    var s = i.Value.ToString("D5");
                    var number = s.Substring(s.Length - 2, 2);
                    _dataToAnalyse.Add(i.Key, number);
                }
                skip += items.Count();
                items = Context.DailyResults.OrderBy(k => k.Date).Skip(skip).ToList().Select(item => new KeyValuePair<DateTime, int>(item.Date, item.Special));
            }
        }

        public IEnumerable<VirtualAnalysisItem> GetNumbersWithLongestLength(int count)
        {
            var allItems = Context.AnalysisItems.SqlQuery("SELECT * FROM AnalysisItems WHERE ID in "+
                                                            "(Select B.ID FROM " +
                                                                "(SELECT DISTINCT TOP 100 PrimaryNumber, [Date], A.RN, A.ID FROM "+
                                                                    "(SELECT PrimaryNumber, [Date], ID, ROW_NUMBER() over(partition by PrimaryNumber order by[Date] desc) RN "+
                                                                        "FROM AnalysisItems "+
                                                                    ") A where A.RN = 1 "+
                                                                    "ORDER BY PrimaryNumber desc "+
                                                                 ") B "+
                                                             ")").ToList();

            var allNumbers = allItems.OrderBy(i => i.Date).Take(count).Select(i => i.PrimaryNumber).ToArray();

            List<VirtualAnalysisItem> results = new List<VirtualAnalysisItem>();
            Dictionary<string, List<AnalysisItem>> collections = new Dictionary<string, List<AnalysisItem>>();
            foreach (var s in allNumbers)
            {
                var existingItems =  Context.AnalysisItems.Where(a => a.PrimaryNumber == s).OrderBy(a => a.Date).ToList();
                if (!collections.ContainsKey(s))
                    collections.Add(s, existingItems);
                var item = new VirtualAnalysisItem();
                item.Date = DateTime.Now.Date;
                item.PrimaryNumber = s;

                BuildAnalysisItem(collections, item);

                results.Add(item);
            }
            return results;
        }

        public void AnalyseData()
        {
            Context.Database.ExecuteSqlCommand("delete from AnalysisItems");
            List<AnalysisItem> itemsToSaveToDb = new List<AnalysisItem>();

            Dictionary<string, List<AnalysisItem>> collections = new Dictionary<string, List<AnalysisItem>>();
            for (int i = 0; i < _dataToAnalyse.Count; i++)
            {
                var data = _dataToAnalyse.ElementAt(i);
                var analysisItem = new AnalysisItem();
                analysisItem.Date = data.Key;
                analysisItem.PrimaryNumber = data.Value;
                itemsToSaveToDb.Add(analysisItem);

                BuildAnalysisItem(collections, analysisItem);

                if (i > 0 && i % 100 == 0)
                {
                    Context.AnalysisItems.AddRange(itemsToSaveToDb);
                    Context.SaveChanges();
                    itemsToSaveToDb = new List<AnalysisItem>();
                }
            }

            if (itemsToSaveToDb.Count > 0)
            {
                Context.AnalysisItems.AddRange(itemsToSaveToDb);
                Context.SaveChanges();
                itemsToSaveToDb = new List<AnalysisItem>();
            }
        }

        private void BuildAnalysisItem(Dictionary<string, List<AnalysisItem>> collections, AnalysisItem analysisItem)
        {
            if (!collections.ContainsKey(analysisItem.PrimaryNumber))
                collections.Add(analysisItem.PrimaryNumber, new List<AnalysisItem>());

            var listOfAnalysisItems = collections[analysisItem.PrimaryNumber];
            listOfAnalysisItems.Sort((a1, a2) => a1.Date.CompareTo(a2.Date));

            if (listOfAnalysisItems.Count > 0)
            {
                var last = listOfAnalysisItems.Last();
                analysisItem.LengthToPrevOccur = (int)(analysisItem.Date - last.Date).TotalDays;

                if (listOfAnalysisItems.Count >= 2) //only have this data after two occurrences
                    analysisItem.AvrOfPrevLengths = last.CurrentAvrOfLengths;

                if (listOfAnalysisItems.Count >= 1) //only have this data after one occurrence
                {
                    if (listOfAnalysisItems.Any(a => a.LengthToPrevOccur > 0))
                    {
                        analysisItem.MinLengthToPrevOccur = (int)Math.Min(analysisItem.LengthToPrevOccur, listOfAnalysisItems.Where(a => a.LengthToPrevOccur > 0).Min(a => a.LengthToPrevOccur));
                        analysisItem.MaxLengthToPrevOccur = (int)Math.Max(analysisItem.LengthToPrevOccur, listOfAnalysisItems.Max(a => a.LengthToPrevOccur));
                    }
                    analysisItem.CurrentAvrOfLengths = (listOfAnalysisItems.Select(a => a.LengthToPrevOccur).Sum() + analysisItem.LengthToPrevOccur) / (listOfAnalysisItems.Count);
                }

                if (listOfAnalysisItems.Count >= 2) //only have this data after two occurrences
                {
                    //delta to previous length
                    analysisItem.DeltaToPreviousLength = analysisItem.LengthToPrevOccur - last.LengthToPrevOccur;

                    analysisItem.MinDeltaToPreviousLength = Math.Min(analysisItem.DeltaToPreviousLength, listOfAnalysisItems.Min(a => a.DeltaToPreviousLength));

                    analysisItem.MaxDeltaToPreviousLength = Math.Max(analysisItem.DeltaToPreviousLength, listOfAnalysisItems.Max(a => a.DeltaToPreviousLength));

                    analysisItem.AvrDeltaToPreviousLength = (listOfAnalysisItems.Select(a => a.DeltaToPreviousLength).Sum() + analysisItem.DeltaToPreviousLength) / (listOfAnalysisItems.Count - 1);

                    analysisItem.AvrOfAbsDeltaToAvrOfPrevLengths = (listOfAnalysisItems.Select(a => Math.Abs(a.DeltaToPreviousLength)).Sum() + Math.Abs(analysisItem.DeltaToPreviousLength)) / (listOfAnalysisItems.Count - 1);
                }


                if (listOfAnalysisItems.Count >= 2) //only have this data after two occurrences
                {
                    //delta to previous average lengths
                    analysisItem.DeltaToAvrOfPrevLengths = analysisItem.LengthToPrevOccur - last.CurrentAvrOfLengths;
                    analysisItem.AvrOfDeltaToAvrOfPrevLengths = (listOfAnalysisItems.Select(a => a.DeltaToAvrOfPrevLengths).Sum() + analysisItem.DeltaToAvrOfPrevLengths) / (listOfAnalysisItems.Count);

                }

                if (!listOfAnalysisItems.Any(a => a.Date == analysisItem.Date))
                {
                    listOfAnalysisItems.Add(analysisItem);
                    listOfAnalysisItems.Sort((a1, a2) => a1.Date.CompareTo(a2.Date));
                }
                else
                {
                    throw new NotSupportedException();
                }

                if (listOfAnalysisItems.Count >= 2) //only have this data after two occurrences
                {
                    var minLength = (float)listOfAnalysisItems.Where(a => a.LengthToPrevOccur > 0).Min(a => a.LengthToPrevOccur);
                    var maxLength = (float)listOfAnalysisItems.Where(a => a.LengthToPrevOccur > 0).Min(a => a.LengthToPrevOccur);
                    var midPoint = (minLength + maxLength) / 2;
                    var delta = (maxLength - minLength) / 4;
                    //low
                    analysisItem.NumberOfLows = listOfAnalysisItems.Where(a => a.LengthToPrevOccur > 0).Count(a => (float)a.LengthToPrevOccur <= midPoint);
                    if (analysisItem.NumberOfLows > 0)
                    {
                        analysisItem.PercentageOfLows = (int)Math.Round(((float)analysisItem.NumberOfLows / listOfAnalysisItems.Count) * 100);
                        analysisItem.AvrOfLows = (float)listOfAnalysisItems.Where(a => a.LengthToPrevOccur > 0).Where(a => (float)a.LengthToPrevOccur <= midPoint).Select(a => a.LengthToPrevOccur).Average();
                    }

                    //medium
                    analysisItem.NumberOfMediums = listOfAnalysisItems.Count(a => ((float)a.LengthToPrevOccur >= midPoint - delta) && ((float)a.LengthToPrevOccur <= midPoint + delta));
                    if (analysisItem.NumberOfMediums > 0)
                    {
                        analysisItem.PercentageOfMediums = (int)Math.Round(((float)analysisItem.NumberOfMediums / listOfAnalysisItems.Count) * 100);
                        analysisItem.AvrOfMediums = (float)listOfAnalysisItems.Where(a => ((float)a.LengthToPrevOccur >= midPoint - delta) && ((float)a.LengthToPrevOccur <= midPoint + delta)).Select(a => a.LengthToPrevOccur).Average();
                    }

                    //high
                    analysisItem.NumberOfHighs = listOfAnalysisItems.Count(a => (float)a.LengthToPrevOccur > midPoint);
                    if (analysisItem.NumberOfHighs > 0)
                    {
                        analysisItem.PercentageOfHighs = (int)Math.Round(((float)analysisItem.NumberOfHighs / listOfAnalysisItems.Count) * 100);
                        analysisItem.AvrOfHighs = (float)listOfAnalysisItems.Where(a => (float)a.LengthToPrevOccur > midPoint).Select(a => a.LengthToPrevOccur).Average();
                    }
                }

            }
            else
            {
                //this is the first analysis item, it doesn't have much analysis data really
                listOfAnalysisItems.Add(analysisItem);
            }
        }
    }
}