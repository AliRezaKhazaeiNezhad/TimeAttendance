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
    public static class ReportDayHelper
    {
        public static async void Initial()
        {

            try
            {
                bool stepOne = await StepOne_RawReport();

                bool stepTwo = await StepTwo_AddingDetail();

                bool stepThree = await StepThree_FirstAnalyz();

                bool stepFour = await StepFour_NullDays();
            }
            catch (Exception e)
            {
            }

        }

        #region توابع عملیاتی

        #region تهیه گزارش های خام و لحظه ای حاضرین/غایبین/کل
        /// <summary>
        /// گزارش روزانه اولیه را تشکیل میدهد
        /// در این بخش فقط بازای انرول های دارای والد و لاگ های بدون گزارش روزانه، میاید و گزارش روزانه را تشکیل میدهد
        /// دراین بخش عملا گزارش های خام تردد، لحظه ای حاضرین/غیبین/کل تشکیل میشوند
        /// </summary>
        /// <param name="context"></param>
        private static async Task<bool> StepOne_RawReport()
        {
            wskhContext context = new wskhContext();
            bool result = true;
            try
            {
                var logList = context.Logs.ToList();
                var userList = context.Users.ToList();
                var enrollList = context.Enrolls.ToList();



                if (logList != null && logList.Count() > 0)
                    logList = logList.Where(x => x.ReportDayId == null).ToList();


                foreach (var user in userList)
                {
                    try
                    {
                        var selectedEnrolls = enrollList.Where(x => x.UserId == user.Id).ToList();

                        foreach (var enroll in selectedEnrolls)
                        {
                            var selectedLogList = logList.Where(x => x.EnrollNo == enroll.EnrollNo && x.DeviceId == enroll.FingerDeviceId && x.ReportDayId == null && x.LogDate != null).ToList();


                            if (selectedLogList != null && selectedLogList.Count() > 0)
                            {
                                foreach (var log in selectedLogList)
                                {
                                    try
                                    {
                                        var firstReportDay = new ReportDay();
                                        var reportDayList = context.ReportDays.ToList();
                                        reportDayList = reportDayList.Where(x => x.UserId == user.Id).ToList();


                                        if (reportDayList != null && reportDayList.Count() > 0)
                                        {
                                            firstReportDay = reportDayList.FirstOrDefault(x => x.ReportDate.Date == log.LogDate.GetValueOrDefault().Date);


                                            if (firstReportDay != null)
                                            {
                                                firstReportDay.Step = ReportDayStep.WaitingToFirstAnalyze;

                                                if (firstReportDay.Logs == null && firstReportDay.Logs.Count() < 0)
                                                {
                                                    firstReportDay.Logs = new List<Log>();
                                                    firstReportDay.Logs.Add(log);
                                                    context.Logs.AddOrUpdate(log);
                                                }
                                                else
                                                {
                                                    log.ReportDayId = firstReportDay.Id;
                                                    context.Logs.AddOrUpdate(log);
                                                }
                                            }
                                            else
                                            {
                                                var persianDate = DateTimeHelper.TopersianDate(log.LogDate.GetValueOrDefault());

                                                if (firstReportDay == null)
                                                    firstReportDay = new ReportDay();

                                                if (firstReportDay.Logs == null && firstReportDay.Logs.Count() < 0)
                                                    firstReportDay.Logs = new List<Log>();

                                                firstReportDay.Logs.Add(log);
                                                firstReportDay.PersianDate = persianDate;
                                                firstReportDay.PersianDay = int.Parse(persianDate.Split('/')[2]);
                                                firstReportDay.PersianDayName = DateTimeHelper.GetPersianDayName(log.LogDate.GetValueOrDefault());
                                                firstReportDay.PersianMonth = int.Parse(persianDate.Split('/')[1]);
                                                firstReportDay.PersianYear = int.Parse(persianDate.Split('/')[0]);
                                                firstReportDay.ReportDate = log.LogDate.GetValueOrDefault().Date;
                                                firstReportDay.State = ReportState.Analyzing;
                                                firstReportDay.TradeType = TradeType.UnKnown;
                                                firstReportDay.UserId = user.Id;
                                                firstReportDay.Step = ReportDayStep.WaitingToFirstAnalyze;

                                                context.ReportDays.Add(firstReportDay);
                                                context.SaveChanges();
                                            }
                                        }
                                        else
                                        {

                                            var persianDate = DateTimeHelper.TopersianDate(log.LogDate.GetValueOrDefault());

                                            if (firstReportDay.Logs == null && firstReportDay.Logs.Count() < 0)
                                                firstReportDay.Logs = new List<Log>();


                                            firstReportDay.Logs.Add(log);
                                            firstReportDay.PersianDate = persianDate;
                                            firstReportDay.PersianDay = int.Parse(persianDate.Split('/')[2]);
                                            firstReportDay.PersianDayName = DateTimeHelper.GetPersianDayName(log.LogDate.GetValueOrDefault());
                                            firstReportDay.PersianMonth = int.Parse(persianDate.Split('/')[1]);
                                            firstReportDay.PersianYear = int.Parse(persianDate.Split('/')[0]);
                                            firstReportDay.ReportDate = log.LogDate.GetValueOrDefault().Date;
                                            firstReportDay.State = ReportState.Analyzing;
                                            firstReportDay.TradeType = TradeType.UnKnown;
                                            firstReportDay.UserId = user.Id;
                                            firstReportDay.Step = ReportDayStep.WaitingToFirstAnalyze;

                                            context.ReportDays.Add(firstReportDay);
                                            context.SaveChanges();
                                        }

                                        logList = logList.Where(x => x != log).ToList();
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            context.SaveChanges();
            context.Dispose();
            return result;
        }
        #endregion


        #region انتساب تقویم کاری/تعطیلی/ نوع برنامه کاری/ نوع تردد کامل یا ناقص به  گزارشات روزانه دارای تقویم کاری
        private static async Task<bool> StepTwo_AddingDetail()
        {
            wskhContext context = new wskhContext();
            bool result = true;


            try
            {
                var userGroupCalendarList = context.UserGroupCalendares.Where(x => x.Remove == false).ToList();
                var reportDayList = context.ReportDays.Where(x => x.Step == ReportDayStep.WaitingToPreAnalyzing).ToList();

                ///درصورت وجود قراردادی که حدف نشده است به این تابع میاید
                if (userGroupCalendarList != null && userGroupCalendarList.Where(x => x.Remove == false).Count() > 0)
                {
                    foreach (var contract in userGroupCalendarList)
                    {

                        try
                        {
                            List<SpecialDay> holidays = new List<SpecialDay>();
                            if (contract.Calendar != null && contract.Calendar.SpecialDayGroupings != null && contract.Calendar.SpecialDayGroupings.SpecialDays != null)
                            {
                                holidays = context.SpecialDays.Where(x => x.SpecialDayGroupingId == contract.Calendar.SpecialDayGroupingId).ToList();
                            }

                            ///درصورت وجود کاربر برای گروه کاربری در قرارداد به این بخش میاید
                            if (contract.UserGroup.Users != null)
                            {
                                ///پیمایش تک تک کاربران گروه مربوطه
                                foreach (var user in contract.UserGroup.Users)
                                {

                                    ///تهیه گزارش روزانه برای روزهای بدون لاگ
                                    AddingReportDay(contract, user);


                                    List<ReportDay> currentUserReportDays = new List<ReportDay>();

                                    currentUserReportDays = reportDayList
                                        .Where(x =>
                                        x.UserId == user.Id &&
                                        x.PersianYear == contract.Calendar.Year &&
                                        x.State != ReportState.Analyzed)
                                        .ToList();

                                    var calendarDay = context.Calendars.FirstOrDefault(x => x.Id == contract.CalendarId);


                                    foreach (var cDay in calendarDay.CalendarDays)
                                    {
                                        var customeCday = context.CalendarDays.FirstOrDefault(x => x.Id == cDay.Id);

                                        var customeRd = currentUserReportDays.Where(x => customeCday.StartDate.Date <= x.ReportDate.Date && x.ReportDate.Date <= customeCday.EndDate.Date).ToList();

                                        ///پیمایش تک تک گزارشات روزانه متناسب با آن کاربر
                                        ///افزودن تقویم به آن گزارش روزانه
                                        foreach (var rd in customeRd)
                                        {
                                            try
                                            {
                                                #region بخش عملیاتی تابع

                                                ///افزودن ای دی تقویم
                                                rd.CalendarId = contract.CalendarId;


                                                ///افزودن ای دی تعطیلی
                                                if (holidays != null)
                                                {
                                                    var holiday = holidays.FirstOrDefault(x => x.Remove == false && x.StartDate.Date == rd.ReportDate.Date);
                                                    if (holiday != null)
                                                        rd.SpecialDayId = holiday.Id;
                                                }


                                                ///بررسی نوع تردد
                                                PreprateReportDayTradeType(rd);


                                                ///افزودن برنامه کاری
                                                if (contract.Calendar.CalendarDays != null)
                                                {
                                                    WorkProgram wp = new WorkProgram();
                                                    wp = customeCday.WorkProgram;

                                                    if (wp != null && wp.Id > 0)
                                                    {
                                                        rd.WorkProgram = wp;
                                                        rd.WorkProgramId = wp.Id;
                                                        rd.WorkProgramType = wp.Type;
                                                        rd.Step = ReportDayStep.WaitingToFirstAnalyze;
                                                    }

                                                }

                                                rd.Step = ReportDayStep.WaitingToSecondAnalyze;

                                                context.ReportDays.AddOrUpdate(rd);
                                                context.SaveChanges();

                                                #endregion
                                            }
                                            catch (Exception e)
                                            {
                                                result = false;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        ///بدست آوردن لیست تعطلات مربوطه

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


        #region در انالیز اولیه نوع برنامه کاری روزانه را منتسب میکند به گزارش روزانه و سپس تشخصی میدهد کدام تابع باید آنالیز کند - تا بع شیفت منظرم/چرخشی/ شناور
        private static async Task<bool> StepThree_FirstAnalyz()
        {
            wskhContext context = new wskhContext();
            bool result = true;

            try
            {
                ///برای برنامه کاری منظم /چرخشی یا بیمارستای میاید و ای دی برنامه کاری روزانه را منتسب میکند
                PrepareWorkProgramDayId();

                List<ReportDay> reportDays = new List<ReportDay>();
                reportDays = context.ReportDays.Where(x => x.Step == ReportDayStep.WaitingToFirstAnalyze).ToList();

                foreach (var reportDay in reportDays)
                {
                    try
                    {
                        reportDay.WorkTime = reportDay.WorkProgramDay.TotalWorkTimeMinute.ToString();
                        reportDay.TotalWorkTime = reportDay.WorkProgramDay.TotalWorkTimeMinute.ToString();

                        switch (reportDay.WorkProgramType)
                        {
                            case WorkProgramType.All:
                                break;
                            case WorkProgramType.Ordinary:
                                break;
                            case WorkProgramType.Flow:
                                break;
                            case WorkProgramType.Complex:
                                break;
                            default:
                                break;
                        }


                        context.ReportDays.AddOrUpdate(reportDay);
                        context.SaveChanges();
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



        #region این بخش اولین و آخرین لاگ هر کاربر را میگیرد و دربین آن برای روزهای بدون تردد مثل جمعه یا تعطیلی، گزارش روزانه تشکیل میدهد
        private static async Task<bool> StepFour_NullDays()
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

        #endregion



        #region توابع عمومی

        /// <summary>
        /// نوع تردد را برای گزارش روزانه مشخص میکند
        /// </summary>
        /// <param name="rd"></param>
        private static void PreprateReportDayTradeType(ReportDay rd)
        {
            if (rd.Logs == null || rd.Logs.Where(x => x.Remove == false).Count() <= 0)
            {
                rd.TradeType = TradeType.Absence;
            }
            else
            {
                if (rd.Logs.Where(x => x.Remove == false).Count() % 2 == 0)
                {
                    rd.TradeType = TradeType.Completed;
                }
                else
                {
                    rd.TradeType = TradeType.Fraction;
                }
            }
        }


        /// <summary>
        /// ازیک برنامه کاری با بازه شروع و پایان میاید و یک مدل لیست را تشکیل میدهد
        /// </summary>
        /// <returns></returns>
        public static List<CustomeWorkProgramModel> PrepareDaysModelList(WorkProgram wp, DateTime startDateTime, DateTime endDateTime)
        {
            List<CustomeWorkProgramModel> modelList = new List<CustomeWorkProgramModel>();

            try
            {
                bool start = true;
                int dayIndex = 1;


                DateTime stDate = startDateTime;

                wp.WorkProgramDays = wp.WorkProgramDays != null ? wp.WorkProgramDays.OrderBy(x => x.DayIndex).ToList() : wp.WorkProgramDays;

                do
                {
                    if (start)
                    {
                        int entityDayIndex = DateTimeHelper.GetDayIndex(startDateTime);
                        var findWPD = wp.WorkProgramDays.FirstOrDefault(x => x.DayIndex == entityDayIndex);

                        dayIndex = findWPD.DayIndex;

                        modelList.Add(new CustomeWorkProgramModel()
                        {
                            WorkProgramId = findWPD.Id,
                            WorkProgramDayId = findWPD.Id,
                            DayIndex = findWPD.DayIndex,
                            WorkType = findWPD.WorkType,
                            DateTime = stDate
                        });

                        stDate = stDate.AddDays(1);
                        dayIndex = findWPD.DayIndex % 7 == 0 && wp.WorkProgramDays.FirstOrDefault(x => x.DayIndex == (dayIndex + 1)) == null ? 1 : dayIndex + 1;

                        start = false;
                    }
                    else
                    {
                        int entityDayIndex = DateTimeHelper.GetDayIndex(startDateTime);
                        var findWPD = wp.WorkProgramDays.FirstOrDefault(x => x.DayIndex == dayIndex);

                        modelList.Add(new CustomeWorkProgramModel()
                        {
                            WorkProgramId = findWPD.Id,
                            WorkProgramDayId = findWPD.Id,
                            DayIndex = findWPD.DayIndex,
                            WorkType = findWPD.WorkType,
                            DateTime = stDate
                        });

                        stDate = stDate.AddDays(1);
                        dayIndex = findWPD.DayIndex % 7 == 0 && wp.WorkProgramDays.FirstOrDefault(x => x.DayIndex == (dayIndex + 1)) == null ? 1 : dayIndex + 1;
                    }


                } while (stDate < endDateTime.Date.AddDays(1));



            }
            catch (Exception e)
            {

            }

            return modelList;
        }


        /// <summary>
        /// ای دی برنامه کاری روزانه را انتساب میدهد دبه برنامه روزانه 
        /// برای برنامه منظم/چرخشی و بیمارستانی کاربرد دارد
        /// در برنامه شناور کاربرد ندارد
        /// </summary>
        /// <param name="context"></param>
        private static void PrepareWorkProgramDayId()
        {
            wskhContext context = new wskhContext();
            List<CustomeWorkProgramModel> modelList = new List<CustomeWorkProgramModel>();
            var calList = context.CalendarDays.Where(x => x.Remove == false).ToList();
            var rdList = context.ReportDays.ToList();
            rdList = rdList.Where(x => x.Step == ReportDayStep.WaitingToFirstAnalyze && x.WorkProgramDayId == null).ToList();

            foreach (var calDay in calList)
            {
                modelList = PrepareDaysModelList(calDay.WorkProgram, calDay.StartDate, calDay.EndDate);
                var customeRdList = rdList.Where(x => x.WorkProgramId == calDay.WorkProgramId).ToList();
                foreach (var rd in customeRdList)
                {
                    try
                    {
                        var firstModel = modelList.FirstOrDefault(x => x.DateTime.Date == rd.ReportDate.Date);
                        rd.WorkProgramDayId = firstModel.WorkProgramDayId;
                        rd.WorkType = firstModel.WorkType;
                        context.ReportDays.AddOrUpdate(rd);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            context.Dispose();
        }


        /// <summary>
        /// در بازه تقویم برای روزهای بدون لاگ میاید و گزارش روزانه تشکلیل میدهد
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contract"></param>
        /// <param name="user"></param>
        private static void AddingReportDay(UserGroupCalendare contract, wskh.Core.wskhUser user)
        {
            wskhContext context = new wskhContext();

            try
            {
                foreach (var item in contract.Calendar.CalendarDays)
                {

                    DateTime stDateTime = item.StartDate;
                    DateTime edDateTime = item.EndDate;


                    var reportDaysList = context.ReportDays.ToList();
                    reportDaysList = reportDaysList
                        .Where(x => x.UserId == user.Id && x.State != ReportState.Analyzed && item.StartDate.Date <= x.ReportDate.Date && x.ReportDate.Date <= edDateTime.Date)
                        .ToList();

                    do
                    {
                        var findReportDay = reportDaysList.FirstOrDefault(x => x.ReportDate.Date == stDateTime.Date);
                        if (findReportDay == null || findReportDay.Id <= 0)
                        {
                            ReportDay entity = new ReportDay();
                            entity.ReportDate = stDateTime;
                            entity.State = ReportState.Analyzing;
                            entity.WorkType = WorkType.Other;
                            entity.DayInWeek = DayInWeek.Saturday;
                            entity.PersianDayName = DateTimeHelper.GetPersianDayName(stDateTime);
                            entity.PersianDate = DateTimeHelper.TopersianDate(stDateTime);
                            entity.PersianDay = int.Parse(entity.PersianDate.Split('/')[2].ToString());
                            entity.PersianMonth = int.Parse(entity.PersianDate.Split('/')[1].ToString());
                            entity.PersianYear = int.Parse(entity.PersianDate.Split('/')[0].ToString());
                            entity.UserId = user.Id;
                            entity.WorkProgramType = WorkProgramType.All;
                            entity.Step = ReportDayStep.WaitingToPreAnalyzing;

                            context.ReportDays.Add(entity);
                            context.SaveChanges();
                        }

                        stDateTime = stDateTime.Date.AddDays(1);

                    } while (stDateTime.Date < edDateTime.Date.AddDays(1));
                }
            }
            catch (Exception e)
            {
            }

            context.Dispose();
        }

        #endregion
    }

    public class CustomeWorkProgramModel
    {
        public CustomeWorkProgramModel()
        {

        }

        public int WorkProgramDayId { get; set; }
        public int WorkProgramId { get; set; }
        public int DayIndex { get; set; }
        public DateTime DateTime { get; set; }
        public WorkType WorkType { get; set; }
    }
}

