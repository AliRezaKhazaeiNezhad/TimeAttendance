using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Core.Enumerator;

namespace wskh.WebEssentials.DateAndTime
{
    public static class DateTimeHelper
    {
        #region متدهای عمومی

        /// <summary>
        /// تبدیل به تاریخ کامل شمسی
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TopersianFull(this DateTime dateTime, string format = null)
        {
            PersianCalendar pc = new PersianCalendar();
            if (string.IsNullOrEmpty(format))
                format = "/";

            return $"{pc.GetYear(dateTime)}{format}{pc.GetMonth(dateTime)}{format}{pc.GetDayOfMonth(dateTime)}  {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}";
        }

        /// <summary>
        /// تبدیل تاریخ متنی به میلادی
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string TopersianDate(this DateTime dateTime, string format = null)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                if (string.IsNullOrEmpty(format))
                    format = "/";

                return $"{pc.GetYear(dateTime)}{format}{pc.GetMonth(dateTime)}{format}{pc.GetDayOfMonth(dateTime)}";
            }
            catch (Exception e)
            {
                return "-";
            }
        }

        /// <summary>
        /// تبدیل تاریخ شمسی رشته به میلادی
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ToGeoDate(this string date)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();

                if (int.Parse(date.Split('/')[1]) == 12 && int.Parse(date.Split('/')[2]) > 29)
                    date = $"{date.Split('/')[0]}/12/29";
                if (int.Parse(date.Split('/')[1]) == 11 && int.Parse(date.Split('/')[2]) > 30)
                    date = $"{date.Split('/')[0]}/11/30";
                if (int.Parse(date.Split('/')[1]) == 10 && int.Parse(date.Split('/')[2]) > 30)
                    date = $"{date.Split('/')[0]}/10/30";
                if (int.Parse(date.Split('/')[1]) == 9 && int.Parse(date.Split('/')[2]) > 30)
                    date = $"{date.Split('/')[0]}/9/30";
                if (int.Parse(date.Split('/')[1]) == 8 && int.Parse(date.Split('/')[2]) > 30)
                    date = $"{date.Split('/')[0]}/8/30";
                if (int.Parse(date.Split('/')[1]) == 7 && int.Parse(date.Split('/')[2]) > 30)
                    date = $"{date.Split('/')[0]}/7/30";
 

                return new DateTime(
                    int.Parse(date.Split('/')[0]),
                    int.Parse(date.Split('/')[1]),
                    int.Parse(date.Split('/')[2]),
                    pc
                    );
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// تبدیل به تاریخ شمنسی
        /// </summary>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string ConvertToCusomePersian(this string date, string year)
        {
            return $"{year}{date.Split('/')[1]}{date.Split('/')[2]}";
        }

        /// <summary>
        /// بدست آوردن عنوان روز به فارسی
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        public static string GetDayName(this int dayIndex)
        {
            switch (dayIndex)
            {
                case 0:
                    return "شنبه";
                case 1:
                    return "یکشنبه";
                case 2:
                    return "دوشنبه";
                case 3:
                    return "سه شنبه";
                case 4:
                    return "چهارشنبه";
                case 5:
                    return "پنجشنبه";
                case 6:
                    return "جمعه";
                default:
                    return "-";
            }
        }

        /// <summary>
        /// بدست آوردن عنوان روز به فارسی
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        public static string GetDayName(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنجشنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                case DayOfWeek.Saturday:
                    return "شنبه";
                default:
                    return "-";
            }
        }

        /// <summary>
        /// بدست آوردن عنوان روز به فارسی
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        public static int GetDayIndex(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 2;
                case DayOfWeek.Monday:
                    return 3;
                case DayOfWeek.Tuesday:
                    return 4;
                case DayOfWeek.Wednesday:
                    return 5;
                case DayOfWeek.Thursday:
                    return 6;
                case DayOfWeek.Friday:
                    return 7;
                case DayOfWeek.Saturday:
                    return 1;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// بدست آوردن بازه زمان از بین دو مقدار
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static string CalculateDuration(this string startTime, string endTime)
        {
            string result = "00:00";

            try
            {
                TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));

                string durationString = duration.ToString();

                string hour = durationString.Split(':')[0];
                string minute = durationString.Split(':')[1];

                if (!string.IsNullOrEmpty(hour) && hour.Length == 1)
                    hour = $"0{hour}";

                if (!string.IsNullOrEmpty(minute) && minute.Length == 1)
                    minute = $"0{minute}";

                result = $"{hour}:{minute}";
            }
            catch (Exception e)
            {
            }

            return result;
        }

        /// <summary>
        /// بررسی میکند که زمان دوم از زمان اول بزرگتر باشد
        /// </summary>
        /// <returns></returns>
        public static bool IsTimeOk(string stTime, string edTime)
        {
            DateTime time1 = DateTime.Parse($"2012/12/12 {stTime}");
            DateTime time2 = DateTime.Parse($"2012/12/12 {edTime}");

            int result = TimeSpan.Compare(time1.TimeOfDay, time2.TimeOfDay);
            if (result == -1)
                return true;
            else
                return false;
        }


        /// <summary>
        /// بررسی میکند زمان اول از دوم بزرگتر است یا خیر
        /// اگر -1 شد زمان اول کوچکتر از زمان دوم است
        /// اگر 0 شد برابر
        /// اگر 1 شد زمان اول بزرگتر از دوم است
        /// </summary>
        /// <param name="stTime"></param>
        /// <param name="edTime"></param>
        /// <returns></returns>
        public static int Comapre(string stTime, string edTime)
        {
            DateTime time1 = DateTime.Parse($"2012/12/12 {stTime.Split(':')[0]}:{stTime.Split(':')[1]}:{stTime.Split(':')[2]}");
            DateTime time2 = DateTime.Parse($"2012/12/12 {edTime.Split(':')[0]}:{edTime.Split(':')[1]}:{edTime.Split(':')[2]}");

            int result = TimeSpan.Compare(time1.TimeOfDay, time2.TimeOfDay);

            return result;
        }

        /// <summary>
        /// بدست آوردن زمان ها برحسب دقیقه برای روزها و برنامه کاری
        /// </summary>
        /// <param name="wp"></param>
        /// <returns></returns>
        public static WorkProgram CalculateTotalMinutes(this WorkProgram wp)
        {
            return TotalMinutes(wp);
        }

        /// <summary>
        /// بدست آوردن زمان کل از دقیقه ها
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string TotalTimes(this int minutes)
        {
            return string.Format("{0:00}:{1:00}", (int)minutes / 60, minutes % 60);
        }



        /// <summary>
        /// نام روز هفته را به فارسی از تاریخ میلادی اعلام میکند
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetPersianDayName(this DateTime dateTime)
        {
            DayOfWeek dayName = dateTime.DayOfWeek;
            switch (dayName)
            {
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنجشنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                case DayOfWeek.Saturday:
                    return "شنبه";
                default:
                    return "-";
            }
        }

        /// <summary>
        /// سال جاری
        /// </summary>
        /// <returns></returns>
        public static int CurrentPersianYear()
        {
            var dt = DateTime.Now;
            var persianDate = TopersianDate(dt);
            return int.Parse(persianDate.Split('/')[0]);
        }


        /// <summary>
        /// بررسی میکند آیا زمان دوم از اول بزرگترمساوی است یا خیر
        /// </summary>
        /// <param name="firstTime"></param>
        /// <param name="secondTime"></param>
        /// <returns></returns>
        public static bool IsBigger(string firstTime, string secondTime)
        {
            TimeSpan first = firstTime.ToTimeSpan();
            TimeSpan second = secondTime.ToTimeSpan();
            return second >= first ? true : false;
        }
        #endregion


        #region متدهای تبدیلگر

        /// <summary>
        /// تبدیل زمان رشته به تایم اسپن
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string time)
        {
            TimeSpan result = new TimeSpan(0,0,0);
            TimeSpan.TryParse(time, out result);
            return result;
        }


        /// <summary>
        /// تبدیل به فرمت ساعت و دقیقه
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToHourMinute(this string time)
        {
            try
            {
                string hour = time.Split(':')[0];
                string minute = time.Split(':')[1];

                if (!string.IsNullOrEmpty(hour) && hour.Length == 1)
                    hour = $"0{hour}";

                if (!string.IsNullOrEmpty(minute) && minute.Length == 1)
                    minute = $"0{minute}";

                return $"{hour}:{minute}";
            }
            catch (Exception e)
            {
                return "-";
            }
        }


        /// <summary>
        /// تبدیل دقیقه به ساعت و دقیقه
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string MinuteToHoureAndMinute(this string min)
        {
            try
            {
                return string.Format("{0:00}:{1:00}", int.Parse(min) / 60, int.Parse(min) % 60);
            }
            catch (Exception e)
            {
                return "-";
            }
        }

        #endregion


        #region متدهای خصوصی
        public static string TimeToMinute(string time)
        {
            var res = ToTimeSpan(time);
            try
            {
                return $"{(int.Parse(time.Split(':')[0]) * 60) + int.Parse(time.Split(':')[1])}";
            }
            catch (Exception)
            {
                return time;
            }
        }

        private static WorkProgram TotalMinutes(this WorkProgram wp)
        {
            wp.WorkProgramDays = TotalMinutes(wp.WorkProgramDays);
            foreach (var day in wp.WorkProgramDays)
            {
                wp.TotalWorkTimeMinute = wp.TotalWorkTimeMinute + (day.TotalWorkTimeMinute < 0 ? day.TotalWorkTimeMinute * -1 : day.TotalWorkTimeMinute);
                wp.TotalRestTimeMinute = wp.TotalRestTimeMinute + (day.TotalRestTimeMinute < 0 ? day.TotalRestTimeMinute * -1 : day.TotalRestTimeMinute);
                wp.TotalOverTimeStartMinute = wp.TotalOverTimeStartMinute + (day.TotalOverTimeStartMinute < 0 ? day.TotalOverTimeStartMinute * -1 : day.TotalOverTimeStartMinute);
                wp.TotalOverTimeEndMinute = wp.TotalOverTimeEndMinute + (day.TotalOverTimeEndMinute < 0 ? day.TotalOverTimeEndMinute * -1 : day.TotalOverTimeEndMinute);
            }
            return wp;
        }
        private static List<WorkProgramDay> TotalMinutes(this List<WorkProgramDay> wpds)
        {
            foreach (var day in wpds)
            {
                day.TotalWorkTimeMinute = day.WorkProgramTimes.TotalMinutes(TimeType.WorkTime);
                day.TotalRestTimeMinute = day.WorkProgramTimes.TotalMinutes(TimeType.RestTime);
                day.TotalOverTimeStartMinute = day.WorkProgramTimes.TotalMinutes(TimeType.OverTimeStart);
                day.TotalOverTimeEndMinute = day.WorkProgramTimes.TotalMinutes(TimeType.OverTimeEnd);
            }
            return wpds;
        }
        private static int TotalMinutes(this List<WorkProgramTime> times, TimeType timeType)
        {
            int result = 0;
            times = times.Where(x => x.TimeType == timeType).ToList();
            foreach (var time in times)
            {
                int extraMinute = 0;
                string newTimeDuration = time.Duration;
                if (newTimeDuration == "24:00")
                {
                    newTimeDuration = "23:59";
                    extraMinute = 1;
                }

                TimeSpan duration = TimeSpan.Parse(newTimeDuration);
                result = result + (int)duration.TotalMinutes + extraMinute;
            }
            return result;
        }
        #endregion
    }
}
