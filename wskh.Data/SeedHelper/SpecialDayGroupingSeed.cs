using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Data;

namespace TimeAttendance.Data.SeedHelper
{
    public static class SpecialDayGroupingSeed
    {
        public static void Initial()
        {
            wskhContext context = new wskhContext();
            #region مقاطع تحصیلی
            if (context.SpecialDayGroupings == null || context.SpecialDayGroupings.Count() == 0)
            {
                context.SpecialDayGroupings.Add(new SpecialDayGrouping()
                {
                    Title = "گروه پیش فرض",
                    Remove = false
                });
            }
            #endregion
            context.SaveChanges();
        }
    }
}
