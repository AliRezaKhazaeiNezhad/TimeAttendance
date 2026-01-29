using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using TimeAttendance.WebEssentials.DateAndTime;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.WebEssentials.DateAndTime;

namespace OrdinaryWorkProgram.anly
{
    /// <summary>
    /// این کلاس و ظیفه انالیز برنامه کاری منظم / چرخشی را دارد
    /// این نوع برنامه به روزهای هفته وابسته میباشد و دوره گردش آن هفت روز میباشد
    /// شروع دوره از شنبه و پایان آن جمعه است
    /// </summary>
    public partial class Analyzer
    {
        /// <summary>
        /// سازنده
        /// </summary>
        public Analyzer()
        {

        }


        /// <summary>
        /// این تابع موارد زیر را انجام میدهد:
        /// ایجاد گزارش تحلیل شده اولیه
        /// انتساب لاگ های موردنظر به هر گزارش
        /// مشخص کردن تعطیلات و ایام خاص برای هرکدام
        /// نوع روز و وضعیت گزارش را مشخص میکند
        /// </summary>
        public void Initial()
        {
            ///تقویم هایی که حذف نشده اند را انتخاب میکنیم
            var calendarList = GetCalendars();

            ///تقویم های جدیدتر را در اولویت قرار میدهد
            if (calendarList != null && calendarList.Count() > 0)
                calendarList = calendarList.OrderByDescending(x => x.Year).ToList();

            try
            {
                ///تک تک تقویم ها را بررسی میکنیم
                foreach (var calendar in calendarList)
                {
                    ///لیست کاربرهایی که دارای قرارداد برای تقویم مذکور هستند
                    var userList = GetUserList(calendar.Id);

                    ///اگر لیست کاربران خالی نباشد
                    if (userList != null)
                    {
                        ///بررسی تک تک کاربران
                        foreach (var user in userList)
                        {
                            ///لیست انرول های یک کاربر را میگیرد
                            var enrollList = EnrollList(user.Id);

                            var calendarDays = GetCalendarDays(calendar.Id);

                            ///تک تک بازه های تقویم را برای هر کاربر بررسی میکند
                            foreach (var calendarDay in calendarDays)
                            {
                                DateTime stDate = calendarDay.StartDate;
                                DateTime secondStDate = calendarDay.StartDate;
                                DateTime edDate = calendarDay.EndDate;

                                List<WorkProgramDayModel> workProgramDayList = GetWorkProgramDayModelList(calendarDay, secondStDate, edDate);


                                ///لیست لاگ هایی را برای بازه تقویم جاری را میگیرد که انالیز نشده اند
                                var logList = GetLogList(calendarDay.Id);

                                ///لیست گزارش های آنالیز شده برای بازه مذکور 
                                var analyzedReportList = GetAnalyzedReports(calendarDay.Id, user.Id);

                                ///لیست تعطیلات را میگیرد
                                var holidayList = GetHolidayList(calendar.SpecialDayGroupingId.GetValueOrDefault());

                                ///لاگ هایی را میگیرد که حذف نشده و
                                ///دربازه تقویم ورودی باشد
                                var selectedLogList = logList
                                    .Where(x =>
                                    x.Remove == false &&
                                    x.LogDate.GetValueOrDefault().Date >= stDate.Date &&
                                    x.LogDate.GetValueOrDefault().Date <= edDate.Date &&
                                    enrollList.Where(z => z.EnrollNo == x.EnrollNo && z.FingerDeviceId == x.DeviceId).Count() > 0)
                                    .ToList();


                                do
                                {
                                    ///لاگ های مربوط به تاریخ جاری
                                    var secondSelectedLogList = new List<Log>();



                                    secondSelectedLogList = selectedLogList
                                        .Where(x => x.LogDate.GetValueOrDefault().Date == secondStDate.Date)
                                        .ToList();


                                    ///مرتب سازی لاگ ها
                                    if (secondSelectedLogList != null)
                                        secondSelectedLogList = secondSelectedLogList
                                            .OrderBy(x => TimeHelper.GlobalTimeFormat(x.LogTime))
                                            .ToList();

                                    ///گزارش های انالیز شده ای را میگیرد که در تاریخ جاری است
                                    var reportList = analyzedReportList
                                        .Where(x => x.ReportDate.Date == secondStDate.Date)
                                        .ToList();




                                    ///بررسی میکند آیا گزارش آنالیز شده ای وجود دارد یا نه
                                    if (reportList != null && reportList.Count() > 0)
                                    {
                                        ///بروزرسانی گزارش
                                        UpdateAnalyzedReport(secondSelectedLogList, reportList, user.Id, calendarDay.Id, secondStDate, workProgramDayList, holidayList);
                                    }
                                    ///درصورتیکه گزارش انالیز شده نداشته باشیم باید گزارش ایجاد شود
                                    else
                                    {
                                        ///ایجاد گزارش
                                        CreateAnalyzedReport(secondSelectedLogList, user.Id, calendarDay.Id, secondStDate, workProgramDayList, holidayList);
                                    }

                                    secondStDate = secondStDate.AddDays(1);
                                } while (secondStDate.Date <= edDate.Date);
                            }


                        }
                    }
                }
            }
            catch (Exception e)
            {
            }



            /// نوع هر زمان را برای لاگ تحلیل شده مشخص میکند
            ChangeAnalyzedReportLogState();



            /// کل زمان کارکرد در روز را مشخص میکند
            /// بیشتر این تابع برای وضیعت درحال آنالیز میباشد
            AddTotalWorkTime();




            /// تعجیل و تاخیر ورود و خروج را مشخص میکند
            /// برای گزارشاتی که وضعیت آنها "در حال آنالیز" میباشد
            /// همچنین وضعیت گزارش را بررسی و تغییر میدهد
            /// جریمه در تاخیر ورود را محاسبه میکند
            EnteranceAndExitAnalyzing();

        }

    }
}
