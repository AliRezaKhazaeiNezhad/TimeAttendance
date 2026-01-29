using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Service;

namespace TimeAttendance.WebEssentials.ReportDayPart
{
    public static class ReportDayHelper
    {
        public static void Create()
        {
            try
            {
                var _userService = DependencyResolver.Current.GetService<IUserService>();

                var _enrollService = DependencyResolver.Current.GetService<IEnrollService>();

                var _logService = DependencyResolver.Current.GetService<ILogService>();

                var _reportDayService = DependencyResolver.Current.GetService<IReportDayService>();



                try
                {
                    var userList = _userService.GetList;

                    var enrollList = _enrollService.GetList;

                    var logList = _logService.GetList;

                    var reportDayList = _reportDayService.GetList;


                    foreach (var user in userList)
                    {
                        if (user.Enrolls != null && user.Enrolls.Count() > 0)
                        {
                            foreach (var userEnroll in user.Enrolls)
                            {

                            }
                        }
                    }


                }
                catch (Exception e)
                {

                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
