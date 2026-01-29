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
    public static class AboutSoftwareSeed
    {
        public static void Initial()
        {
            wskhContext context = new wskhContext();
            #region درباره نرم افزار
            if (context.AboutSoftwares == null || context.AboutSoftwares.Count() == 0)
            {
                context.AboutSoftwares.Add(new AboutSoftware()
                {
                    PublishDateTime = new DateTime(2020, 10, 29),
                    Version = "V1.0.0",
                    Description = "1- پابلیش اولین نسخه نرم افزار حضور و غیاب###2- ارتباط با سخت افزار های شرکت فینگرتک"
                });
              
            }
            #endregion
            context.SaveChanges();
        }
    }
}
