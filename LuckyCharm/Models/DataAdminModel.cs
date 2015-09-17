using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyCharm.Models
{
    public class DataAdminModel
    {
        public DateTime LatestRecordedDate { get; set; }

        public DateTime LatestAnalyzedDate { get; set; }

        public DateTime OldestRecordedDate { get; set; }

        public int NumberOfRecords { get; set; }

    }
}