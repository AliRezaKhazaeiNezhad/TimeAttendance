using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.Service;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.WebEssentials.ReportPart
{
    public static class ReportDayDuplicatorHelper
    {
        public static async void Initial()
        {

            try
            {
                bool addReportDay = await AddReportDay();
                bool removeReportDay = await RemoveReportDay();
            }
            catch (Exception e)
            {
            }

        }

        #region این بخش اولین و آخرین لاگ هر کاربر را میگیرد و دربین آن برای روزهای بدون تردد مثل جمعه یا تعطیلی، گزارش روزانه تشکیل میدهد
        private static async Task<bool> AddReportDay()
        {
            wskhContext context = new wskhContext();
            bool result = true;

            try
            {
                var reportDays = context.ReportDays.ToList();
                var logs = context.Logs.ToList();
                var users = context.Users.ToList();

                foreach (var user in users)
                {
                    try
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = DateTime.Now;
                        List<ReportDay> customeReportDays = new List<ReportDay>();

                        customeReportDays = reportDays.Where(x => x.UserId == user.Id).ToList();

                        if (customeReportDays != null && customeReportDays.Count() > 0)
                        {
                            customeReportDays = customeReportDays.OrderBy(x => x.ReportDate.Date).ThenBy(x => x.ReportDate.TimeOfDay).ToList();
                            DateTime customeStartDateTime = customeReportDays.FirstOrDefault().ReportDate;
                            DateTime customeEndDateTime = customeReportDays.LastOrDefault().ReportDate;


                            do
                            {
                                if (customeReportDays.Where(x => x.ReportDate.Date == customeStartDateTime.Date).Count() <= 0)
                                {
                                    ReportDay entity = new ReportDay();

                                    entity.ReportDate = customeStartDateTime;
                                    entity.Remove = false;

                                    /////در این بخش باید اطلاعات یک گزارش کامل شود
                                    string persianDate = DateTimeHelper.TopersianDate(customeStartDateTime);
                                    entity.PersianDate = persianDate;
                                    entity.PersianDay = int.Parse(persianDate.Split('/')[2]);
                                    entity.PersianDayName = DateTimeHelper.GetPersianDayName(customeStartDateTime);
                                    entity.PersianMonth = int.Parse(persianDate.Split('/')[1]);
                                    entity.PersianYear = int.Parse(persianDate.Split('/')[0]);
                                    entity.ReportDate = customeStartDateTime.Date;
                                    entity.State = ReportState.Analyzing;
                                    entity.TradeType = TradeType.UnKnown;
                                    entity.UserId = user.Id;


                                    context.ReportDays.Add(entity);
                                    context.SaveChanges();
                                }

                                customeStartDateTime = customeStartDateTime.AddDays(1);
                            }
                            while (customeStartDateTime.Date <= customeEndDateTime.Date);
                         
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            context.Dispose();
            return result;
        }
        #endregion


        #region حذف گزارشات روزانه تکراری یک کاربر
        private static async Task<bool> RemoveReportDay()
        {
            wskhContext context = new wskhContext();
            bool result = true;

            try
            {
                var reportDays = context.ReportDays.ToList();
                var logs = context.Logs.ToList();
                var users = context.Users.ToList();

                foreach (var user in users)
                {
                    try
                    {
                        var customeReportDays = reportDays.Where(x => x.UserId == user.Id).ToList();
                        customeReportDays = customeReportDays.OrderBy(x => x.ReportDate.Date).ToList();
                        DateTime customeStartDateTime = customeReportDays.FirstOrDefault().ReportDate;
                        DateTime customeEndDateTime = customeReportDays.LastOrDefault().ReportDate;

                        do
                        {
                            var selectedList = customeReportDays.Where(x => x.ReportDate.Date == customeStartDateTime.Date).ToList();

                            if (selectedList.Count() > 1)
                            {
                                ReportDay entity = new ReportDay();

                                for (int i = 0; i < selectedList.Count(); i++)
                                {
                                    if (i > 0)
                                    {
                                        context.ReportDays.Remove(selectedList[i]);
                                        context.SaveChanges();
                                    }
                                }
                               
                            }

                            customeStartDateTime = customeStartDateTime.AddDays(1);
                        }
                        while (customeStartDateTime.Date <= customeEndDateTime.Date);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            context.Dispose();
            return result;
        }
        #endregion
    }


}

