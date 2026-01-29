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
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.Service;
using wskh.WebEssentials.DateAndTime;

namespace OrdinaryWorkProgram.anly
{
    /// <summary>
    /// متدهای عملیاتی در این بخش میباشند که از سایر متدهای بخش آنالیزر استفاده میکنند
    /// </summary>
    public partial class Analyzer
    {

        /// <summary>
        /// بروزرسانی یک گزارش
        /// </summary>
        /// <param name="secondSelectedLogList"></param>
        /// <param name="reportList"></param>
        private static void UpdateAnalyzedReport(List<Log> secondSelectedLogList, List<AnalyzedReport> reportList, string userId, int calendarDayId, DateTime dt, List<WorkProgramDayModel> workProgramDayList, List<SpecialDay> holidayList)
        {
            var firstReport = reportList.FirstOrDefault();

            wskhContext context = new wskhContext();


            try
            {
                var selectedReportList = context.AnalyzedReports.ToList();
                selectedReportList = selectedReportList.Where(x => x.UserId == userId).ToList();
                selectedReportList = selectedReportList.Where(x => x.CalendarDayId == calendarDayId).ToList();
                selectedReportList = selectedReportList.Where(x => x.Remove != true).ToList();
                selectedReportList = selectedReportList.Where(x => x.Date.Date == dt.Date).ToList();

                AnalyzedReport report = selectedReportList.FirstOrDefault();
                report.WorkProgramDayId = workProgramDayList.First(x => x.DateTime.Date == dt.Date).WorkProgramDayId;

                context.AnalyzedReports.AddOrUpdate(report);
                context.SaveChanges();

                ///در این بخش لاگ های غیر تکراری را شناسایی میکنیم
                List<Log> listFinal = DistinctLogs(secondSelectedLogList, context, report);


                ///تهیه تعطیلی و ایام خاص
                PrepareHolidayAndSpecialDay(dt, holidayList, report);


                ///افزودن لاگ ها به گزارش
                CreateReportLogs(secondSelectedLogList, report, listFinal);

            }
            catch (Exception e)
            {
            }
            context.Dispose();
        }

        /// <summary>
        /// ایجاد گزارش انالیز شده
        /// </summary>
        /// <param name="secondSelectedLogList"></param>
        /// <param name="userId"></param>
        /// <param name="calendarDayId"></param>
        /// <param name="dt"></param>
        private static void CreateAnalyzedReport(List<Log> secondSelectedLogList, string userId, int calendarDayId, DateTime dt, List<WorkProgramDayModel> workProgramDayList, List<SpecialDay> holidayList)
        {

            wskhContext context = new wskhContext();

            try
            {
                AnalyzedReport report = new AnalyzedReport();

                report.State = wskh.Core.Enumerator.AnalyzedReportState.WaitingForAnalyzing;
                report.Type = wskh.Core.Enumerator.AnalyzedReportType.Ordinary;
                report.UserId = userId;
                report.CalendarDayId = calendarDayId;
                report.CreateDateTime = DateTime.Now;

                report.PersianDayName = DateTimeHelper.GetPersianDayName(dt);
                report.PersianDate = DateTimeHelper.TopersianDate(dt);
                report.ReportDate = dt;
                report.Date = dt;

                report.WorkProgramDayId = workProgramDayList.First(x => x.DateTime.Date == dt.Date).WorkProgramDayId;

                secondSelectedLogList = secondSelectedLogList.Distinct().ToList();



                context.AnalyzedReports.Add(report);
                context.SaveChanges();
                context.Dispose();


                ///در این بخش لاگ های غیر تکراری را شناسایی میکنیم
                List<Log> listFinal = DistinctLogs(secondSelectedLogList, context, report);


                ///تهیه تعطیلی و ایام خاص
                PrepareHolidayAndSpecialDay(dt, holidayList, report);


                ///افزودن لاگ ها به گزارش
                CreateReportLogs(secondSelectedLogList, report, listFinal);
            }
            catch (Exception e)
            {
            }
            context.Dispose();
        }

        /// <summary>
        /// ایجاد گزارشات لاگ ها
        /// </summary>
        /// <param name="secondSelectedLogList"></param>
        /// <param name="report"></param>
        /// <param name="listFinal"></param>
        private static void CreateReportLogs(List<Log> secondSelectedLogList, AnalyzedReport report, List<Log> listFinal)
        {
            ///فراخوانی سرویس و انتخاب کزارش
            var _analyzedReportService = DependencyResolver.Current.GetService<IAnalyzedReportService>();
            var entity = _analyzedReportService.FindById(report.Id);

            int max = listFinal.Count();

            /// پیمایش کلیه لاگ ها
            for (int i = 1; i <= max; i++)
            {
                AnalyzedReportLog analyzedReportLog = new AnalyzedReportLog();

                ///اگر تنها یک تردد ثبت شده باشد از حلقه خارج میشود
                if (secondSelectedLogList.Count() % 2 == 0 && i == secondSelectedLogList.Count())
                    break;


                var firstLog = secondSelectedLogList[i - 1];
                var secondLog = new Log();

                try
                {
                    secondLog = secondSelectedLogList[i];
                }
                catch (Exception e)
                {
                }


                if (firstLog != null)
                {
                    analyzedReportLog.FirstLogId = firstLog.Id;
                    analyzedReportLog.FirstLogTime = firstLog.LogTime;
                }

                if (secondLog != null && secondLog.Id > 0)
                {
                    analyzedReportLog.SecondLogId = secondLog.Id;
                    analyzedReportLog.SecondLogTime = secondLog.LogTime;
                    analyzedReportLog.Duration = TimeHelper.Duration(firstLog.LogTime, secondLog.LogTime);
                }
                analyzedReportLog.TimeType = wskh.Core.Enumerator.TimeType.Other;


                entity.AnalyzedReportLogs.Add(analyzedReportLog);
            }


            _analyzedReportService.Update(entity);
            _analyzedReportService.Dispose();
        }

        /// <summary>
        /// میزان کسر کار/غیبت تاخیر ورود را براساس قانون کاری مشخص میکند
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        private static void EnteranceAbsence(int reportId)
        {
            wskhContext context = new wskhContext();


            try
            {
                var report = context.AnalyzedReports.FirstOrDefault(x => x.Id == reportId);

                ///پارامترها
                bool hasMonthlyDelay = false;
                bool hasDailyDelay = false;
                int monthlyDelayMin = 0;
                int dailyDelayMin = 0;
                DateTime startWorkTime = new DateTime(1990, 01, 01, 00, 00, 00);
                DateTime startWorkTimeWithDelay = new DateTime(1990, 01, 01, 00, 00, 00);
                DateTime enteranceWorkTime = new DateTime(1990, 01, 01, 00, 00, 00);
                DelayAction delyAction;
                bool isHoliday = false;


                isHoliday = report.WorkProgramDay.WorkType == WorkType.Holiday || report.WorkProgramDay.WorkType == WorkType.RestDay ? true : false;

                ///زمان شروع کار در روز گزارش را بدست میاورد
                string workTimeStart = report
                                             .WorkProgramDay
                                             .WorkProgramTimes
                                             .OrderBy(x => TimeSpan.Parse(x.StartTime)).FirstOrDefault(x => x.TimeType == TimeType.WorkTime)
                                             .StartTime;
                var firstLog = report
                                     .AnalyzedReportLogs
                                     .OrderBy(x => TimeSpan.Parse(x.FirstLogTime))
                                     .FirstOrDefault()
                                     .FirstLogTime;


               

                /// شناسایی قوانین تردد
                var requestRule = report.CalendarDay.Calendar.RequestRule;


                ///محسابه سقف مجاز ماهانه
                if (requestRule.DelyMonthMin != "0")
                {
                    hasMonthlyDelay = true;
                    monthlyDelayMin = int.Parse(requestRule.DelyMonthMin);
                }

                ///محسابه سقف مجاز روزانه
                if (requestRule.DelayDayMin != "0")
                {
                    hasDailyDelay = true;
                    dailyDelayMin = int.Parse(requestRule.DelayDayMin);
                }


                if (!string.IsNullOrEmpty(workTimeStart) && !string.IsNullOrEmpty(firstLog))
                {
                    ///زمان مجاز ورود با تاخیر را محاسبه میکند
                    startWorkTime = new DateTime(1990, 01, 01, int.Parse(workTimeStart.Split(':')[0]), int.Parse(workTimeStart.Split(':')[1]), 0);
                    startWorkTimeWithDelay = startWorkTime;
                    startWorkTimeWithDelay = startWorkTimeWithDelay.AddMinutes(dailyDelayMin);

                    ///زمان ورود را شناسایی میکند
                    enteranceWorkTime = new DateTime(1990, 01, 01, int.Parse(firstLog.Split(':')[0]), int.Parse(firstLog.Split(':')[1]), 0);
                }

                ///عملیات تاخیر ورود
                delyAction = requestRule.DelayAction;


             

                if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0)
                {
                    DelayEnteranceCompile(report, startWorkTimeWithDelay, enteranceWorkTime, delyAction, isHoliday);
                }

            }
            catch (Exception e)
            {

            }

            context.SaveChanges();

            context.Dispose();
        }

        /// <summary>
        /// مشخص میکند در صورت تاخیر ورد چه رویدادی اتفاق بیافتد
        /// </summary>
        /// <param name="report"></param>
        /// <param name="startWorkTimeWithDelay"></param>
        /// <param name="enteranceWorkTime"></param>
        /// <param name="delyAction"></param>
        /// <param name="isHoliday"></param>
        /// <returns></returns>
        private static void DelayEnteranceCompile(AnalyzedReport report,
                                                  DateTime startWorkTimeWithDelay,
                                                  DateTime enteranceWorkTime,
                                                  DelayAction delyAction,
                                                  bool isHoliday)
        {
            int res1 = DateTimeHelper.Comapre($"{startWorkTimeWithDelay.Hour}:{startWorkTimeWithDelay.Minute}:0",
                                               $"{enteranceWorkTime.Hour}:{enteranceWorkTime.Minute}:0");

            if (res1 == -1)
            {
                switch (delyAction)
                {
                    case DelayAction.Nothing:
                        break;
                    case DelayAction.DelayFromStart:
                        break;
                    case DelayAction.DelayRemainTime:
                        break;
                    case DelayAction.Absence:

                        if (isHoliday)
                            report.State = AnalyzedReportState.Holiday_Absence_Systemic;
                        else
                            report.State = AnalyzedReportState.Absence_Systemic;

                        break;
                    case DelayAction.Disapprove:
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
