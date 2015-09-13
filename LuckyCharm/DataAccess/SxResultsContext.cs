using LuckyCharm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LuckyCharm.DataAccess
{
    public class SxResultsContext : DbContext
    {
        public SxResultsContext() : base("DefaultConnection")
        { }

        public DbSet<DailyResult> DailyResults { get; set; }

        public DbSet<AnalysisItem> AnalysisItems { get; set; }
    }
}