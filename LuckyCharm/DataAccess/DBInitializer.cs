using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuckyCharm.DataAccess
{
    public class DBInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SxResultsContext>
    {
        protected override void Seed(SxResultsContext context)
        {
            //enter test data here!
            base.Seed(context);
        }
    }
}