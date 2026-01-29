using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.WebEssentials.DateAndTime;
using wskh.Core;
using wskh.Service;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.WebEssentials.RequestPart
{
    public static class RequestHelper
    {
        /// <summary>
        /// مشخص میکند آیا کاربر برای سال انتخابی دارای تقویم میباشد یا خیر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool HasCalendar(this List<UserGroupCalendare> calendars, int year)
        {
            bool result = false;
            try
            {
                if (calendars == null || calendars.Count() <= 0)
                    result = false;
                else if (calendars.Where(x => x.Calendar.Remove == false && x.Calendar.Year == year).Count() <= 0)
                    result = false;
                else
                    result = true;

            }
            catch (Exception)
            {

            }
            return result;
        }


        /// <summary>
        /// بدست آوردن مانده مرخصی
        /// </summary>
        public static Tuple<string, string, string, string, string, bool, bool> RemainLeave(string userId, int leaveTypeId, string dateStriing, string startHour, string endHour)
        {
            #region مقادیر قابل برگشت
            string requestDuration = "0";
            string remainLastYear = "0";
            string remainThisYear = "0";
            string remainAfterSubtraction = "0";
            string maximumAbsenceTime = "0";
            bool allowToLeave = false;
            bool isDaily = false;
            #endregion

            try
            {
                #region فراخوانی سرویس ها
                var _userService = DependencyResolver.Current.GetService<IUserService>();
                var _requestService = DependencyResolver.Current.GetService<IRequestService>();
                var _leaveTypeService = DependencyResolver.Current.GetService<ILeaveTypeService>();
                var _calendarService = DependencyResolver.Current.GetService<ICalendarService>();
                var _userGroupCalendarService = DependencyResolver.Current.GetService<IUserGroupCalendareService>();
                #endregion


                #region بدست آوردن مقادیر مورد نیاز
                requestDuration = TimeHelper.TotalMinute(TimeHelper.Duration(startHour, endHour));
                int year = int.Parse(dateStriing.Split('/')[0]);
                var geoDate = DateTimeHelper.ToGeoDate(dateStriing);
                int lastYear = year - 1;
                var user = _userService.GetList.FirstOrDefault(x => x.Id == userId);
                var userGroupCalendarThisYear = _userGroupCalendarService.GetList.FirstOrDefault(x => x.Remove == false && x.Calendar.Year == year && x.UserGroupId == user.UserGroupId);
                var calendarThisYear = userGroupCalendarThisYear.Calendar;
                var userGroupCalendarLastYear = _userGroupCalendarService.GetList.FirstOrDefault(x => x.Remove == false && x.Calendar.Year == lastYear && x.UserGroupId == user.UserGroupId);

                if (userGroupCalendarLastYear == null)
                    userGroupCalendarLastYear = new UserGroupCalendare();

                var calendarLastYear = userGroupCalendarLastYear.Calendar == null || userGroupCalendarLastYear == null ? new Calendar() : userGroupCalendarLastYear.Calendar;


                maximumAbsenceTime = TimeHelper.TotalMinute(userGroupCalendarThisYear.Calendar.RequestRule.MaximumAbsence);
                #endregion


                #region بررسی نوع مرخصی
                if (leaveTypeId == 1)
                {
                    #region بدست آوردن مانده سال قبل
                    if (calendarLastYear != null)
                    {
                        var allRequests = _requestService.GetList.Where(x => x.LeaveTypeId == 1 && x.CalendarId == calendarLastYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved).ToList();

                        if (allRequests != null &&
                            allRequests.Count() > 0 &&
                            int.Parse(calendarLastYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.Id == 1).RestToNextYear) > 0)
                        {
                            string totalTime = "0";
                            totalTime = calendarLastYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.Id == 1).YearlyRestMin;
                            allRequests.ForEach(x => remainLastYear = (int.Parse(remainLastYear) + int.Parse(x.TotalTime)).ToString());

                            remainLastYear = (int.Parse(totalTime) - int.Parse(totalTime)).ToString();

                            if (int.Parse(remainLastYear) < 0)
                                remainLastYear = "0";
                        }
                    }
                    #endregion

                    #region بدست آوردن مانده سال جاری
                    if (calendarThisYear != null)
                    {

                        ///بررسی میکند که اگر میزان مرخصی ساعتی بیشتر از سقف باشد، باید روزانه محاسبه گردد
                        if (int.Parse(maximumAbsenceTime) > int.Parse(requestDuration))
                        {
                            var allRequests = _requestService.GetList.Where(x => x.LeaveTypeId == 1 && x.CalendarId == calendarThisYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved).ToList();

                            string totalTime = "0";

                            totalTime = calendarThisYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.LeaveTypeId == 1).YearlyRestMin;


                            if (allRequests != null)
                                allRequests.ForEach(x => remainThisYear = (int.Parse(remainThisYear) + int.Parse(x.TotalTime)).ToString());

                            remainThisYear = (int.Parse(totalTime) - int.Parse(remainThisYear)).ToString();


                            ///بررسی میشود آیا مانده مثبت از سال قبل دارد یا خیر
                            if (int.Parse(remainLastYear) > 0)
                                remainThisYear = (int.Parse(remainThisYear) + int.Parse(remainLastYear)).ToString();

                            remainAfterSubtraction = (int.Parse(remainThisYear) - int.Parse(requestDuration)).ToString();
                        }
                        else
                        {
                            string totalTime = "0";
                            isDaily = true;


                            ///میزان زمان مرخصی های گرفته شده در سال جاری را محاسبه میکند
                            var allRequests = _requestService
                                .GetList
                                .Where(x => x.LeaveTypeId == 1 && x.CalendarId == calendarThisYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved)
                                .ToList();

                            if (allRequests != null)
                                allRequests.ForEach(x => remainThisYear = (int.Parse(remainThisYear) + int.Parse(x.TotalTime)).ToString());


                            /// سقف مرخصی استحقاقی امسال را حساب میکند
                            totalTime = calendarThisYear
                                .RequestRule
                                .RequestRuleDetails
                                .FirstOrDefault(x => x.LeaveTypeId == 1)
                                .YearlyRestMin;





                            ///بررسی میشود آیا مانده مثبت از سال قبل دارد یا خیر
                            if (int.Parse(remainLastYear) > 0)
                                remainThisYear = (int.Parse(remainThisYear) + int.Parse(remainLastYear)).ToString();




                            remainAfterSubtraction = (int.Parse(remainThisYear) - int.Parse(requestDuration)).ToString();
                        }
                    }
                    #endregion


                }
                else if (leaveTypeId == 3)
                {
                }
                else
                {
                    #region بدست آوردن مانده سال جاری
                    if (calendarThisYear != null)
                    {
                        var allRequests = _requestService.GetList.Where(x => x.LeaveTypeId == leaveTypeId && x.CalendarId == calendarThisYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved).ToList();

                        string totalTime = "0";
                        if (calendarThisYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.LeaveTypeId == leaveTypeId) != null)
                            totalTime = calendarThisYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.LeaveTypeId == leaveTypeId).YearlyRestMin;
                        allRequests.ForEach(x => remainThisYear = (int.Parse(remainThisYear) + int.Parse(x.TotalTime)).ToString());

                        remainThisYear = (int.Parse(totalTime) - int.Parse(remainThisYear)).ToString();


                        ///بررسی میشود آیا مانده مثبت از سال قبل دارد یا خیر
                        remainAfterSubtraction = (int.Parse(remainThisYear) - int.Parse(requestDuration)).ToString();
                    }
                    #endregion
                }
                #endregion


            }
            catch (Exception e)
            {
            }


            return Tuple.Create(requestDuration, remainLastYear, remainThisYear, remainAfterSubtraction, maximumAbsenceTime, allowToLeave, isDaily);
        }


        /// <summary>
        /// بدست آوردن مانده مرخصی
        /// </summary>
        public static Tuple<string, string, string, string, int> RemainDailyLeave(string userId, int leaveTypeId, string startDate, string endDate)
        {
            #region مقادیر قابل برگشت
            string requestDuration = "0";
            string remainLastYear = "0";
            string remainThisYear = "0";
            string remainAfterSubtraction = "0";
            int calendarId = 0;
            #endregion

            try
            {
                #region فراخوانی سرویس ها
                var _userService = DependencyResolver.Current.GetService<IUserService>();
                var _requestService = DependencyResolver.Current.GetService<IRequestService>();
                var _leaveTypeService = DependencyResolver.Current.GetService<ILeaveTypeService>();
                var _calendarService = DependencyResolver.Current.GetService<ICalendarService>();
                var _userGroupCalendarService = DependencyResolver.Current.GetService<IUserGroupCalendareService>();
                #endregion


                #region بدست آوردن مقادیر مورد نیاز
                int year = int.Parse(startDate.Split('/')[0]);
                var geoDateStart = DateTimeHelper.ToGeoDate(startDate);
                var geoDateEnd = DateTimeHelper.ToGeoDate(endDate);
                int lastYear = year - 1;
                var user = _userService.GetList.FirstOrDefault(x => x.Id == userId);
                var userGroupCalendarThisYear = _userGroupCalendarService.GetList.FirstOrDefault(x => x.Remove == false && x.Calendar.Year == year && x.UserGroupId == user.UserGroupId);
                var calendarThisYear = userGroupCalendarThisYear.Calendar;


               

                var userGroupCalendarLastYear = _userGroupCalendarService.GetList.FirstOrDefault(x => x.Remove == false && x.Calendar.Year == lastYear && x.UserGroupId == user.UserGroupId);

                if (userGroupCalendarLastYear == null)
                    userGroupCalendarLastYear = new UserGroupCalendare();

                var calendarLastYear = userGroupCalendarLastYear.Calendar == null || userGroupCalendarLastYear == null ? new Calendar() : userGroupCalendarLastYear.Calendar;

                #endregion


                #region بررسی نوع مرخصی
                if (leaveTypeId == 1)
                {
                    #region بدست آوردن مانده سال قبل
                    if (calendarLastYear != null)
                    {
                        var allRequests = _requestService.GetList.Where(x => x.LeaveTypeId == 1 && x.CalendarId == calendarLastYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved).ToList();

                        if (allRequests != null &&
                            allRequests.Count() > 0 &&
                            int.Parse(calendarLastYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.Id == 1).RestToNextYear) > 0)
                        {
                            string totalTime = "0";
                            totalTime = calendarLastYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.Id == 1).YearlyRestMin;
                            allRequests.ForEach(x => remainLastYear = (int.Parse(remainLastYear) + int.Parse(x.TotalTime)).ToString());

                            remainLastYear = (int.Parse(totalTime) - int.Parse(totalTime)).ToString();

                            if (int.Parse(remainLastYear) < 0)
                                remainLastYear = "0";
                        }
                    }
                    #endregion

                    #region بدست آوردن مانده سال جاری
                    if (calendarThisYear != null)
                    {
                        calendarId = calendarThisYear.Id;
                        ///بررسی میکند که اگر میزان مرخصی ساعتی بیشتر از سقف باشد، باید روزانه محاسبه گردد
                        var allRequests = _requestService.GetList.Where(x => x.LeaveTypeId == 1 && x.CalendarId == calendarThisYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved).ToList();

                        string totalTime = "0";

                        totalTime = calendarThisYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.LeaveTypeId == 1).YearlyRestMin;


                        if (allRequests != null)
                            allRequests.ForEach(x => remainThisYear = (int.Parse(remainThisYear) + int.Parse(x.TotalTime)).ToString());

                        remainThisYear = (int.Parse(totalTime) - int.Parse(remainThisYear)).ToString();


                        ///بررسی میشود آیا مانده مثبت از سال قبل دارد یا خیر
                        if (int.Parse(remainLastYear) > 0)
                            remainThisYear = (int.Parse(remainThisYear) + int.Parse(remainLastYear)).ToString();

                        remainAfterSubtraction = (int.Parse(remainThisYear) - int.Parse(requestDuration)).ToString();
                    }
                    #endregion


                }
                else if (leaveTypeId == 3)
                {
                }
                else
                {
                    #region بدست آوردن مانده سال جاری
                    if (calendarThisYear != null)
                    {
                        var allRequests = _requestService.GetList.Where(x => x.LeaveTypeId == leaveTypeId && x.CalendarId == calendarThisYear.Id && x.UserRequesterId == userId && x.State == wskh.Core.Enumerator.RequestState.Approved).ToList();

                        string totalTime = "0";
                        if (calendarThisYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.LeaveTypeId == leaveTypeId) != null)
                            totalTime = calendarThisYear.RequestRule.RequestRuleDetails.FirstOrDefault(x => x.LeaveTypeId == leaveTypeId).YearlyRestMin;
                        allRequests.ForEach(x => remainThisYear = (int.Parse(remainThisYear) + int.Parse(x.TotalTime)).ToString());

                        remainThisYear = (int.Parse(totalTime) - int.Parse(remainThisYear)).ToString();


                        ///بررسی میشود آیا مانده مثبت از سال قبل دارد یا خیر
                        remainAfterSubtraction = (int.Parse(remainThisYear) - int.Parse(requestDuration)).ToString();
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
            }


            return Tuple.Create(requestDuration, remainLastYear, remainThisYear, remainAfterSubtraction, calendarId);
        }
    }

}


