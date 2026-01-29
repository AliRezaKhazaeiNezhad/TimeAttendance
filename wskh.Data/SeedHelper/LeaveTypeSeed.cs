using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core;
using wskh.Data;

namespace TimeAttendance.Data.SeedHelper
{
    public static class LeaveTypeSeed
    {
        public static void Initial()
        {
            wskhContext context = new wskhContext();
            #region نوع مرخصی
            if (context.LeaveTypes == null || context.LeaveTypes.Count() == 0)
            {
                context.LeaveTypes.Add(new LeaveType()
                {
                    Title = "استحقاقی",
                    Remove = false,
                    AllowRemove = false
                });
                context.LeaveTypes.Add(new LeaveType()
                {
                    Title = "استعلاجی",
                    Remove = false,
                    AllowRemove = false
                });
                context.LeaveTypes.Add(new LeaveType()
                {
                    Title = "بدون حقوق",
                    Remove = false,
                    AllowRemove = false
                });
            }
            #endregion
            context.SaveChanges();
        }
    }
}
