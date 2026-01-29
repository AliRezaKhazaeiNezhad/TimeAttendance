using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.WebEssentials.DateAndTime;

namespace OrdinaryWorkProgram.anly
{
    /// <summary>
    /// متدهای آنالیزگر در این بخش میباشند که به ازای هر گزارش انالیز خاصی را انجام میدهند
    /// </summary>
    public partial class Analyzer
    {

        /// <summary>
        /// لیست گزارش های آنالیز شده را براساس وضعیت گزارش انتخاب میکند
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<AnalyzedReport> GetAnalyzedReports(AnalyzedReportState state)
        {
            List<AnalyzedReport> list = new List<AnalyzedReport>();

            wskhContext context = new wskhContext();

            try
            {
                var entityList = context.AnalyzedReports.ToList();

                if (entityList != null && entityList.Count() > 0)
                    entityList = entityList.Where(x => x.State == state).ToList();

                list = entityList;
            }
            catch (Exception e)
            {
            }

            context.Dispose();

            return list;
        }


        /// <summary>
        /// کل زمان کارکرد در روز را مشخص میکند
        /// بیشتر این تابع برای وضیعت درحال آنالیز میباشد
        /// </summary>
        private static void AddTotalWorkTime()
        {
            wskhContext context = new wskhContext();

            try
            {
                var entityList = context.AnalyzedReports.ToList();

                #region کل لاگ هایی که در حال آنالیز هستند را میگیرد و برای آنها کل ساعت حضور را مشخص میکند
                if (entityList != null && entityList.Count() > 0)
                    entityList = entityList.Where(x => x.State == AnalyzedReportState.WaitingForAnalyzing).ToList();

                if (entityList != null && entityList.Count() > 0)
                {
                    foreach (var entity in entityList)
                    {
                        if (entity.WorkProgramDayId == null || entity.WorkProgramDayId <= 0)
                            entity.TotalWorkTime = "00:00";
                        else
                            entity.TotalWorkTime = DateTimeHelper.MinuteToHoureAndMinute(entity.WorkProgramDay.TotalWorkTimeMinute.ToString());
                    }
                }


                #endregion

                context.SaveChanges();
            }
            catch (Exception e)
            {

            }

            context.SaveChanges();
        }



        /// <summary>
        /// نوع هر زمان را برای لاگ تحلیل شده مشخص میکند
        /// </summary>
        public void ChangeAnalyzedReportLogState()
        {
            wskhContext context = new wskhContext();

            try
            {
                var list = context.AnalyzedReports.ToList();


                if (list != null && list.Count() > 0)
                    list = list
                        .Where(x => x.State == AnalyzedReportState.WaitingForAnalyzing)
                        .ToList();


                if (list != null && list.Count() > 0)
                    foreach (var report in list)
                    {
                        try
                        {
                            int index = 1;
                            foreach (var log in report.AnalyzedReportLogs)
                            {
                                if (log.SecondLogId == null)
                                {
                                    log.TimeType = TimeType.Fraction;
                                    break;
                                }

                                if (index % 2 == 0)
                                {
                                    log.TimeType = TimeType.Absence;
                                }
                                else
                                {
                                    log.TimeType = TimeType.WorkTime;
                                }

                                index = index + 1;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                context.SaveChanges();

            }
            catch (Exception e)
            {

            }



            context.Dispose();
        }


        /// <summary>
        /// تعجیل و تاخیر ورود و خروج را مشخص میکند
        /// برای گزارشاتی که وضعیت آنها "در حال آنالیز" میباشد
        /// بررسی اضافه کار و کسر کار
        /// بررسی اضافه کار اول وقت و اخر وقت
        /// همچنین وضعیت گزارش را بررسی و تغییر میدهد
        /// </summary>
        public void EnteranceAndExitAnalyzing()
        {
            wskhContext context = new wskhContext();

            var list = context.AnalyzedReports.ToList();


            try
            {

                if (list != null && list.Count() > 0)
                    list = list
                        .Where(x => x.State == AnalyzedReportState.WaitingForAnalyzing)
                        .ToList();

                foreach (var report in list)
                {
                    try
                    {

                        report.EarlyEnteranceTime = "0";
                        report.DelayEnterTime = "0";
                        report.DelayExitTime = "0";
                        report.EarlyExit = "0";

                        report.RealTotaloffTime = "00:00";
                        report.RealTotalWorkTime = "00:00";

                        report.TotalEarlyEnterance = "0";
                        report.TotalDelayExit = "0";


                        ///در این قسمت "تاخیر/تعجیل ورود/خروج" را مشخص میکند
                        if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0)
                        {
                            #region متغییرها
                            report.State = AnalyzedReportState.Presence;
                            report.Absence = false;

                            var times = report
                                              .WorkProgramDay
                                              .WorkProgramTimes
                                              .ToList();

                            var analyzedTimes = report
                                                      .AnalyzedReportLogs
                                                      .Where(x => x.TimeType == TimeType.WorkTime)
                                                      .ToList();

                            List<WorkProgramTime> modelList = new List<WorkProgramTime>();


                            ///انتخاب لاگ های کاری 
                            if (times != null && times.Count() > 0)
                                modelList = times.Where(x => x.TimeType == TimeType.WorkTime).ToList();

                            ///مرتب سازی لیست براساس زمان شروع
                            if (modelList != null && modelList.Count() > 0)
                                modelList = modelList.OrderBy(x => DateTimeHelper.ToTimeSpan(x.StartTime)).ToList();

                            ///مرتب سازی لاگ های تحلیل شده
                            if (analyzedTimes != null && analyzedTimes.Count() > 0)
                                analyzedTimes = analyzedTimes.OrderBy(x => DateTimeHelper.ToTimeSpan(x.FirstLogTime)).ToList();



                            ///اولین و آخرین لاگ ارجینال کاری را میگیرد
                            var fistTime = DateTimeHelper
                                                        .ToTimeSpan(times
                                                                         .Where(x =>
                                                                                    x.TimeType == TimeType.WorkTime)
                                                                         .First()
                                                                                 .StartTime);
                            var lastTime = DateTimeHelper
                                                         .ToTimeSpan(times
                                                                          .Where(x =>
                                                                                     x.TimeType == TimeType.WorkTime)
                                                                          .Last()
                                                                                 .EndTime);

                            ///اولین و آخرین لاگهای تحلیل شده را میگیرد
                            var fistAnalyzedTime = DateTimeHelper.ToTimeSpan(analyzedTimes.First().FirstLogTime);
                            var lastAnalyzedTime = DateTimeHelper.ToTimeSpan(analyzedTimes.Last().SecondLogTime);
                            #endregion


                            #region محاسبه زمان تاخیر/تعجیل ورود
                            ///بررسی زمان ورود
                            //اگر شرط برابر منفی یک شد یعنی تعجیل ورود
                            var entTime = DateTimeHelper.Comapre(fistAnalyzedTime.ToString(), fistTime.ToString());
                            if (entTime == -1)
                            {
                                report.EarlyEnteranceTime = DateTimeHelper
                                    .CalculateDuration(fistAnalyzedTime.ToString(), fistTime.ToString());
                            }
                            ///تردد به موقع
                            else if (entTime == 0)
                            {
                            }
                            ///در این حالت تاخیر ورود است
                            else
                            {
                                report.DelayEnterTime = DateTimeHelper
                                    .CalculateDuration(fistTime.ToString(), fistAnalyzedTime.ToString());
                            }
                            #endregion


                            #region محاسبه زمان تاخیر/تعجیل خروج

                            ///بررسی زمان خروج
                            //اگر شرط برابر منفی یک شد یعنی تاخیر خروج
                            var extTime = DateTimeHelper.Comapre(lastAnalyzedTime.ToString(), lastTime.ToString());
                            if (extTime == -1)
                            {
                                report.EarlyExit = DateTimeHelper
                                    .CalculateDuration(lastAnalyzedTime.ToString(), lastTime.ToString());
                            }
                            ///تردد در زمان مقرر
                            else if (extTime == 0)
                            {
                            }
                            ///در این حالت تعجیل خروج است
                            else
                            {
                                report.DelayExitTime = DateTimeHelper
                                    .CalculateDuration(lastTime.ToString(), lastAnalyzedTime.ToString());
                            }
                            #endregion


                            #region محاسبه کل زمان حضور واقعی
                            ///بدست آوردن کل زمان حضور واقعی
                            if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0)
                            {
                                string totalRealWorkTime = "00:00";

                                ///شناسایی لاگ های کاری یک روز کاربر
                                var workTimeList = report
                                                         .AnalyzedReportLogs
                                                         .Where(x =>
                                                                    x.TimeType == TimeType.OverTimeEnd ||
                                                                    x.TimeType == TimeType.OverTimeStart ||
                                                                    x.TimeType == TimeType.WorkTime)
                                                         .ToList();


                                foreach (var item in workTimeList)
                                {
                                    if (item.SecondLogId > 0)
                                    {
                                        int duration = int.Parse(DateTimeHelper.TimeToMinute(item.Duration));
                                        int totalworkTime = int.Parse(DateTimeHelper.TimeToMinute(totalRealWorkTime));

                                        totalRealWorkTime = DateTimeHelper.TotalTimes(totalworkTime + duration);
                                    }
                                }

                                report.RealTotalWorkTime = totalRealWorkTime;
                            }
                            #endregion


                            #region محاسبه کل زمان عدم حضور واقعی
                            ///بدست آوردن کل زمان عدم حضور واقعی
                            if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0)
                            {

                                string totalRealOffTime = "00:00";

                                ///شناسایی لاگ های کاری یک روز کاربر
                                var offTimeList = report
                                             .AnalyzedReportLogs
                                             .Where(x =>
                                                        x.TimeType == TimeType.Absence ||
                                                        x.TimeType == TimeType.RestTime)
                                             .ToList();

                                foreach (var item in offTimeList)
                                {
                                    if (item.SecondLogId > 0)
                                    {
                                        int duration = int.Parse(DateTimeHelper.TimeToMinute(item.Duration));
                                        int totalworkTime = int.Parse(DateTimeHelper.TimeToMinute(totalRealOffTime));

                                        totalRealOffTime = DateTimeHelper.TotalTimes(totalworkTime + duration);
                                    }
                                }

                                report.RealTotaloffTime = totalRealOffTime;
                            }
                            #endregion


                            #region محاسبه اضافه کار اول وقت
                            ///بررسی و محاسبه اضافه کار اول وقت
                            if (report.EarlyEnteranceTime != null && report.EarlyEnteranceTime != "00:00")
                            {
                                ///لیست زمان های کاری را از  برنامه میگیرد
                                List<WorkProgramTime> wpTimes = report.WorkProgramDay.WorkProgramTimes ?? new List<WorkProgramTime>();

                                if (wpTimes != null)
                                    wpTimes = wpTimes.Where(x => x.TimeType == TimeType.OverTimeStart).ToList();

                                if (wpTimes == null)
                                    wpTimes = new List<WorkProgramTime>();


                                /// انتخاب اولین زمان اضافه کار
                                var firstWPTS = wpTimes.FirstOrDefault();

                                ///شناسایی زمان شروع و پایان اضافه کار
                                var wpTimesStart = DateTimeHelper.ToTimeSpan(firstWPTS.StartTime);
                                var wpTimesEnd = DateTimeHelper.ToTimeSpan(firstWPTS.EndTime);

                                ///شناسایی تردد ورود
                                var enteranceTime = report.AnalyzedReportLogs.FirstOrDefault(x => x.TimeType == TimeType.WorkTime);

                                ///لاگ ورودی
                                var analyzedEnteranceTime = DateTimeHelper.ToTimeSpan(enteranceTime.FirstLogTime);
                                var analyzedExitTime = DateTimeHelper.ToTimeSpan(enteranceTime.SecondLogTime);

                                var comareByFirstTime = DateTimeHelper.Comapre(analyzedEnteranceTime.ToString(), wpTimesStart.ToString());
                                ///یعنی زمان ورود پیش از ساعت اضافه کار است
                                if (comareByFirstTime == -1 || comareByFirstTime == 0)
                                {
                                    var comareEnterBylLastTime = DateTimeHelper.Comapre(analyzedEnteranceTime.ToString(), wpTimesEnd.ToString());
                                    var comareExitBylLastTime = DateTimeHelper.Comapre(analyzedExitTime.ToString(), wpTimesEnd.ToString());


                                    ///لاگ خروج در بازه اضافه کار اول وقت خورده است
                                    if (comareExitBylLastTime == -1)
                                    {
                                        report.TotalEarlyEnterance = DateTimeHelper.CalculateDuration(wpTimesStart.ToString(), analyzedExitTime.ToString());
                                    }
                                    else
                                    {
                                        report.TotalEarlyEnterance = DateTimeHelper.CalculateDuration(wpTimesStart.ToString(), wpTimesEnd.ToString());

                                    }
                                }
                                /// زمان ورود بعد از شروع اضافه کار است
                                else
                                {
                                    var comareEnterBylLastTime = DateTimeHelper.Comapre(analyzedEnteranceTime.ToString(), wpTimesEnd.ToString());
                                    var comareExitBylLastTime = DateTimeHelper.Comapre(analyzedExitTime.ToString(), wpTimesEnd.ToString());


                                    ///لاگ خروج در بازه اضافه کار اول وقت خورده است
                                    if (comareExitBylLastTime == -1)
                                    {
                                        report.TotalEarlyEnterance = DateTimeHelper
                                .CalculateDuration(analyzedEnteranceTime.ToString(), analyzedExitTime.ToString());
                                    }
                                    else
                                    {
                                        report.TotalEarlyEnterance = DateTimeHelper
                                  .CalculateDuration(analyzedEnteranceTime.ToString(), wpTimesEnd.ToString());

                                    }
                                }
                            }
                            #endregion


                            #region محاسبه اضافه کار آخر وقت
                            ///بررسی و محاسبه اضافه کار آخر وقت
                            if (report.DelayExitTime != null && report.DelayExitTime != "00:00")
                            {
                                ///لیست زمان های کاری را از  برنامه میگیرد
                                List<WorkProgramTime> wpTimes = report.WorkProgramDay.WorkProgramTimes ?? new List<WorkProgramTime>();

                                if (wpTimes != null)
                                    wpTimes = wpTimes.Where(x => x.TimeType == TimeType.OverTimeEnd).ToList();

                                if (wpTimes == null)
                                    wpTimes = new List<WorkProgramTime>();


                                /// انتخاب اولین زمان اضافه کار
                                var firstWPTS = wpTimes.FirstOrDefault();

                                ///شناسایی زمان شروع و پایان اضافه کار
                                var wpTimesStart = DateTimeHelper.ToTimeSpan(firstWPTS.StartTime);
                                var wpTimesEnd = DateTimeHelper.ToTimeSpan(firstWPTS.EndTime);

                                ///شناسایی تردد خروج
                                var enteranceTime = report.AnalyzedReportLogs.LastOrDefault(x => x.TimeType == TimeType.WorkTime);

                                ///لاگ خروجی
                                var analyzedEnteranceTime = DateTimeHelper.ToTimeSpan(enteranceTime.FirstLogTime);
                                var analyzedExitTime = DateTimeHelper.ToTimeSpan(enteranceTime.SecondLogTime);

                                var comareByFirstTime = DateTimeHelper.Comapre(analyzedEnteranceTime.ToString(), wpTimesEnd.ToString());
                                ///یعنی زمان خروج پیش از اتمام ساعت اضافه کار یا برابر ان است
                                if (comareByFirstTime == -1 || comareByFirstTime == 0)
                                {
                                    var comareEnterBylLastTime = DateTimeHelper.Comapre(analyzedEnteranceTime.ToString(), wpTimesStart.ToString());
                                    var comareExitBylLastTime = DateTimeHelper.Comapre(analyzedExitTime.ToString(), wpTimesEnd.ToString());


                                    ///لاگ ورود پیش از اضافه کار آخر وقت خورده است
                                    if (comareEnterBylLastTime == -1)
                                    {
                                        report.TotalDelayExit = DateTimeHelper.CalculateDuration(wpTimesStart.ToString(), analyzedExitTime.ToString());
                                    }
                                    else
                                    {
                                        report.TotalDelayExit = DateTimeHelper.CalculateDuration(analyzedEnteranceTime.ToString(), analyzedExitTime.ToString());
                                    }
                                }
                                /// زمان خروج بعد از پایان اضافه کار است
                                else
                                {
                                    var comareEnterBylLastTime = DateTimeHelper.Comapre(analyzedEnteranceTime.ToString(), wpTimesStart.ToString());
                                    var comareExitBylLastTime = DateTimeHelper.Comapre(analyzedExitTime.ToString(), wpTimesEnd.ToString());


                                    ///لاگ ورود پیش از اضافه کار آخر وقت خورده است
                                    if (comareEnterBylLastTime == -1 || comareEnterBylLastTime == 0)
                                    {
                                        report.TotalDelayExit = DateTimeHelper.CalculateDuration(wpTimesStart.ToString(), wpTimesEnd.ToString());
                                    }
                                    else
                                    {
                                        report.TotalDelayExit = DateTimeHelper.CalculateDuration(analyzedEnteranceTime.ToString(), analyzedExitTime.ToString());
                                    }
                                }
                            }
                            #endregion
                         
                        }

                      

                        ///وضعیت گزارش را مشخص میکند
                        ReprotState(report);


                      
                    }
                    catch (Exception e)
                    {
                    }

                }

            }
            catch (Exception e)
            {

            }

            context.SaveChanges();

            #region وضعیت را براساس تاخیر ورود مشخص میکند

            list.ForEach(x =>EnteranceAbsence(x.Id));

            #endregion

            context.Dispose();

        }


        /// <summary>
        /// براساس نوع لاگ ها به ببرسی گزارش و تعین وضعیت هرگزارش میپردازد
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        private static bool ReprotState(AnalyzedReport report)
        {
            bool result = false;

            wskhContext context = new wskhContext();

            try
            {
                ///بررسی میشود آیا در برنامه کاری روز کاری است تا با بررسی آنالیز تردد در آن روز به تعیین وضعیت گزارش روزانه بپردازد
                if (report.WorkProgramDay != null && report.WorkProgramDay.WorkType == WorkType.WorkDay)
                {
                    ///اگر لاگی نداشته باشد را بررسی و "غیبت" میزند
                    if (report.AnalyzedReportLogs == null || report.AnalyzedReportLogs.Count() == 0)
                    {
                        report.State = AnalyzedReportState.Absence_NoTransaction;
                        report.Absence = true;
                    }
                    ///اگر اخرین لاگ ، پایان نداشته باشد را "تردد ناقص" در نظر میگیرد
                    else if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0 && report.AnalyzedReportLogs.Last().SecondLogId == null)
                    {
                        report.State = AnalyzedReportState.FractionTrade;
                        report.Absence = false;
                    }
                    ///درغیر اینصورت میشود حضور
                    else if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0)
                    {
                        report.State = AnalyzedReportState.Presence;
                        report.Absence = false;
                    }
                }
                // دراین حالت روزکاری نمیباشد و به آنالیز میپردازد
                else
                {
                    ///این بخش بررسی میکند آیا برای روز تعطیلی/استراحت زمان تردد در برنامه کاری داشته باشیم تا به بررسی بپردازد 
                    if (report.WorkProgramDay.WorkProgramTimes != null && report.WorkProgramDay.WorkProgramTimes.Count() > 0)
                    {
                        ///اگر لاگی نداشته باشد را بررسی و "غیبت" میزند
                        if (report.AnalyzedReportLogs == null || report.AnalyzedReportLogs.Count() == 0)
                        {
                            report.State = AnalyzedReportState.Holiday_Absence_NoTransaction;
                            report.Absence = true;
                        }
                        ///اگر اخرین لاگ ، پایان نداشته باشد را "تردد ناقص" در نظر میگیرد
                        else if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0 && report.AnalyzedReportLogs.Last().SecondLogId == null)
                        {
                            report.State = AnalyzedReportState.HolidayFraction;
                            report.Absence = false;
                        }
                        ///درغیر اینصورت میشود حضور
                        else if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() > 0)
                        {
                            report.State = AnalyzedReportState.Holiday_Presence;
                            report.Absence = false;
                        }
                    }
                    else
                    {
                        ///درصورتیکه در برنامه کاری ترددی ثبت نشده باشد نوع گزارش تعطیلی یا روز استراحت را تعیین میکند
                        if (report.AnalyzedReportLogs != null && report.AnalyzedReportLogs.Count() <= 0)
                        {
                            ///براساس برنامه کاری میشود روز استراحت
                            if (report.WorkProgramDay.WorkType == WorkType.RestDay)
                            {
                                report.State = AnalyzedReportState.RestDay;
                                report.Absence = false;
                            }

                            ///براساس برنامه کاری میشود روز تعطیلی
                            if (report.WorkProgramDay.WorkType == WorkType.Holiday)
                            {
                                report.State = AnalyzedReportState.Holiday;
                                report.Absence = false;
                            }
                        }
                        ///بررسی نوع گزارش در روز تعطیلی یا استراحت با داشتن لاگ تردد در این روز
                        else
                        {
                            report.State = AnalyzedReportState.Holiday_Presence;
                            report.Absence = false;
                        }
                    }
                }


               

                context.SaveChanges();

                result = true;
            }
            catch (Exception e)
            {

            }


            context.Dispose();

            return result;
        }


    }
}
