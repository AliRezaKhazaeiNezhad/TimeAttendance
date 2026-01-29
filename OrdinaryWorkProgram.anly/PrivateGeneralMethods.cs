using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.WebEssentials.DateAndTime;
using wskh.Core;
using wskh.Data;
using wskh.Service;
using wskh.WebEssentials.DateAndTime;

namespace OrdinaryWorkProgram.anly
{
    /// <summary>
    /// متدهای خصوصی در این بخش نمایش داده میشود
    /// </summary>
    public partial class Analyzer
    {
        /// <summary>
        /// این تابع لیست تقویم هایی را برمیگرداند که حذف نشده اند
        /// </summary>
        /// <returns></returns>
        private List<TimeAttendance.Core.Calendar> GetCalendars()
        {
            wskhContext context = new wskhContext();

            List<TimeAttendance.Core.Calendar> enttiyList = new List<TimeAttendance.Core.Calendar>();

            try
            {
                var list = context.Calendars.ToList();

                if (list != null && list.Count() > 0)
                    list = list.Where(x => x.Remove == false || x.Remove == null).ToList();

                enttiyList = list;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// این تابغ لیست افردادی را میگیرد که دارای قرارداد با تقویم رودی میباشند
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        private List<wskhUser> GetUserList(int calendarId)
        {
            wskhContext context = new wskhContext();

            List<wskhUser> enttiyList = new List<wskhUser>();

            try
            {
                var list = context.Users.ToList();

                if (list != null && list.Count() > 0)
                    list = list
                        .Where(x =>
                        x.UserGroup != null &&
                        x.UserGroup.UserGroupCalendares != null &&
                        x.UserGroup.UserGroupCalendares.Where(f => f.CalendarId == calendarId).Count() > 0)
                        .ToList();

                enttiyList = list;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// لیست انرول های یک کاربر را برمیگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<Enroll> EnrollList(string userId)
        {
            wskhContext context = new wskhContext();

            List<Enroll> enttiyList = new List<Enroll>();

            try
            {
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                if (user.Enrolls != null && user.Enrolls.Count() > 0)
                {
                    foreach (var enroll in user.Enrolls)
                    {
                        enttiyList.Add(enroll);
                    }
                }
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// بدست اوردن لیست بازه های تقویم
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        private List<CalendarDay> GetCalendarDays(int calendarId)
        {
            wskhContext context = new wskhContext();

            List<CalendarDay> enttiyList = new List<CalendarDay>();

            try
            {
                var list = context.CalendarDays.Where(x => x.CalendarId == calendarId).ToList();

                if (list != null && list.Count() > 0)
                    list = list.OrderBy(x => x.StartDate.Date).ToList();

                enttiyList = list;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// لیست لاگ هایی را میگیرد که برای تقویم ورودی انالیز نشده است
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        private List<Log> GetLogList(int calendarDayId)
        {
            wskhContext context = new wskhContext();

            List<Log> enttiyList = new List<Log>();

            try
            {
                var logs = context.Logs.ToList();

                var analyzedLogs = context.AnalyzedReportLogs.ToList();

                var secondLogList = new List<Log>();

                if (analyzedLogs != null && analyzedLogs.Count() > 0)
                    foreach (var item in analyzedLogs)
                    {
                        if (item.FirstLog != null && item.FirstLogId > 0)
                            secondLogList.Add(item.FirstLog);

                        if (item.SecondLog != null && item.SecondLogId > 0)
                            secondLogList.Add(item.SecondLog);
                    }

                enttiyList = logs.Where(x => secondLogList.All(x2 => x.Id != x2.Id)).ToList();
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// لیست گزارش های آنالیز شده برای بازه مذکور 
        /// </summary>
        /// <param name="calendarDayId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<AnalyzedReport> GetAnalyzedReports(int calendarDayId, string userId)
        {
            wskhContext context = new wskhContext();

            List<AnalyzedReport> enttiyList = new List<AnalyzedReport>();

            try
            {
                var reports = context.AnalyzedReports.ToList();

                if (reports != null && reports.Count() > 0)
                {
                    reports = reports
                        .Where(x =>
                        x.CalendarDayId == calendarDayId &&
                        x.UserId == userId)
                        .ToList();
                }

                enttiyList = reports;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// لیست تعطیلات
        /// </summary>
        /// <param name="specialDayGroupingId"></param>
        /// <returns></returns>
        private List<SpecialDay> GetHolidayList(int specialDayGroupingId)
        {
            wskhContext context = new wskhContext();

            List<SpecialDay> enttiyList = new List<SpecialDay>();

            try
            {
                var reports = context.SpecialDays.ToList();

                if (reports != null && reports.Count() > 0)
                {
                    reports = reports
                        .Where(x =>
                        x.SpecialDayGroupingId == specialDayGroupingId)
                        .ToList();
                }

                enttiyList = reports;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// لیست روزهای کاری را بر میگرداند
        /// </summary>
        /// <param name="workProgramId"></param>
        /// <returns></returns>
        private List<WorkProgramDay> GetCalendarDayList(int workProgramId)
        {
            wskhContext context = new wskhContext();

            List<WorkProgramDay> enttiyList = new List<WorkProgramDay>();

            try
            {
                var reports = context.WorkProgramDays.ToList();

                if (reports != null && reports.Count() > 0)
                    reports = reports.Where(x => x.WorkProgramId == workProgramId).ToList();

                enttiyList = reports;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return enttiyList;
        }


        /// <summary>
        /// جداسازی لاگ ها
        /// </summary>
        /// <param name="secondSelectedLogList"></param>
        /// <param name="context"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        private static List<Log> DistinctLogs(List<Log> secondSelectedLogList, wskhContext context, AnalyzedReport report)
        {
            List<Log> listOne = new List<Log>();
            foreach (var item in report.AnalyzedReportLogs)
            {
                listOne.Add(context.Logs.FirstOrDefault(x => x.Id == item.Id));
            }
            listOne.AddRange(secondSelectedLogList);
            List<Log> listFinal = new List<Log>();
            listFinal = listOne.Distinct().ToList();
            return listFinal;
        }


        /// <summary>
        /// لیست روزهای 
        /// </summary>
        /// <param name="calendarDay"></param>
        /// <param name="secondStDate"></param>
        /// <param name="edDate"></param>
        /// <returns></returns>
        private List<WorkProgramDayModel> GetWorkProgramDayModelList(CalendarDay calendarDay, DateTime secondStDate, DateTime edDate)
        {
            ///لیست روزهای برنامه کاری را میگیرد
            var workProgramDayList = GetCalendarDayList(calendarDay.WorkProgramId);

            DateTime listSecondStDate = calendarDay.StartDate;

            List<WorkProgramDayModel> modelList = new List<WorkProgramDayModel>();

            bool start = true;
            int index = 0;
            do
            {

                if (start)
                {
                    var dayIndex = DateTimeHelper.GetDayIndex(listSecondStDate);
                    var findItem = workProgramDayList.First(x => x.DayIndex == dayIndex);
                    index = workProgramDayList.FindIndex(x => x.Id == findItem.Id);

                    modelList.Add(new WorkProgramDayModel()
                    {
                        DateTime = secondStDate,
                        DayIndex = dayIndex,
                        WorkProgramDayId = findItem.Id
                    });

                    index = index + 1;
                    start = false;
                }
                else
                {
                    if (index >= workProgramDayList.Count())
                        index = 0;

                    var dayIndex = DateTimeHelper.GetDayIndex(secondStDate);
                    var findItem = workProgramDayList[index];

                    modelList.Add(new WorkProgramDayModel()
                    {
                        DateTime = secondStDate,
                        DayIndex = dayIndex,
                        WorkProgramDayId = findItem.Id
                    });

                    index = index + 1;

                }

                secondStDate = secondStDate.AddDays(1);
            } while (secondStDate.Date <= edDate.Date);
            return modelList;
        }


        /// <summary>
        /// ایجاد تعطیلی و روز خاص برای یک گزارش انالیز شده
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="holidayList"></param>
        /// <param name="report"></param>
        private static void PrepareHolidayAndSpecialDay(DateTime dt, List<SpecialDay> holidayList, AnalyzedReport report)
        {
            ///تعطیلی را مشخص میکند
            if (holidayList != null && holidayList.Count() > 0 && holidayList.Where(x => x.StartDate.Date == dt.Date && x.Type == wskh.Core.Enumerator.SpecialDayType.Holiday).Count() > 0)
            {
                var _holidayService = DependencyResolver.Current.GetService<ISpecialDayService>();
                var _analyzedReportService = DependencyResolver.Current.GetService<IAnalyzedReportService>();


                var holidayEntity = _holidayService.FindById(holidayList.FirstOrDefault(x => x.StartDate.Date == dt.Date).Id);

                var reportEntity = _analyzedReportService.FindById(report.Id);
                reportEntity.IsHoliday = true;
                reportEntity.HolidayId = holidayEntity.Id;

                _analyzedReportService.Update(reportEntity);


                _analyzedReportService.Dispose();
                _holidayService.Dispose();
            }

            ///ایام خاص را مشخص میکند
            if (holidayList != null &&
                holidayList.Count() > 0 && holidayList.Where(x => x.Type == wskh.Core.Enumerator.SpecialDayType.SpecialDay && x.StartDate.Date <= dt.Date && x.EndDate.GetValueOrDefault().Date >= dt.Date).Count() > 0)
            {
                var _holidayService = DependencyResolver.Current.GetService<ISpecialDayService>();
                var _analyzedReportService = DependencyResolver.Current.GetService<IAnalyzedReportService>();


                var holidayEntity = _holidayService.FindById(holidayList.FirstOrDefault(x => x.Type == wskh.Core.Enumerator.SpecialDayType.SpecialDay && x.StartDate.Date <= dt.Date && x.EndDate.GetValueOrDefault().Date >= dt.Date).Id);

                var reportEntity = _analyzedReportService.FindById(report.Id);
                reportEntity.IsSpecialDay = true;
                reportEntity.SpecialDayId = holidayEntity.Id;

                _analyzedReportService.Update(reportEntity);

                _analyzedReportService.Dispose();
                _holidayService.Dispose();
            }
        }


        /// <summary>
        /// کلاس اختصاصی
        /// </summary>
        private class WorkProgramDayModel
        {
            public WorkProgramDayModel()
            {

            }

            public int DayIndex { get; set; }
            public DateTime DateTime { get; set; }
            public int WorkProgramDayId { get; set; }
        }
    }
}
