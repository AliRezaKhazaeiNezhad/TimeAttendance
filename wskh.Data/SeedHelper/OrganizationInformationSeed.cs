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
    public static class OrganizationInformationSeed
    {
        public static void Initial()
        {
            wskhContext context = new wskhContext();
            #region اطلاعات سازمان
            if (context.OrganizationInformation == null || context.OrganizationInformation.Count() == 0)
            {
                context.OrganizationInformation.Add(new OrganizationInformation()
                {
                    Title = "نام سازمان",
                    Category = "زمینه فعالیت",
                    Address = "آدرس",
                    Phone = "05...",
                    LogoPath = "/wwwroot/dashboard/img/128.png",
                    Completed = false,
                    Remove = false,
                });
              
            }
            #endregion
            context.SaveChanges();
        }
    }
}
