using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Model;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.WebEssentials.DateAndTime
{
    public static class TimeHelper
    {
        public static bool IsTimeOneBigger(this string timeOne, string timeTwo)
        {
            TimeSpan time1 = TimeSpan.Parse(timeOne);
            TimeSpan time2 = TimeSpan.Parse(timeTwo);
            if (time1 > time2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// مدت زمان را از بین دوتا ساعت بدست میاورد
        /// </summary>
        /// <param name="timeOne"></param>
        /// <param name="timeTwo"></param>
        /// <returns></returns>
        public static string Duration(this string timeOne, string timeTwo, bool isAnalyze = false)
        {
            string result = "00:00";
            try
            {
                if (timeOne.ToLower() != timeTwo.ToLower() && isAnalyze == false)
                {
                    TimeSpan midNight = TimeSpan.Parse("23:59");
                    TimeSpan morning = TimeSpan.Parse("00:00");
                    TimeSpan extraTime = TimeSpan.Parse("00:01");


                    TimeSpan time1 = TimeSpan.Parse(timeOne);
                    TimeSpan time2 = TimeSpan.Parse(timeTwo);
                    if (time2 < time1)
                        result = (((midNight + extraTime) - time1) + (time2 - morning)).ToString();
                    else if (time2 == time1)
                        result = "24:00";
                    else
                        result = (time2 - time1).ToString();
                }
                else
                {

                    TimeSpan time1 = TimeSpan.Parse(timeOne);
                    TimeSpan time2 = TimeSpan.Parse(timeTwo);
                    result = (time2 - time1).ToString();
                }
            }
            catch (Exception e)
            {
                result = "00:00";
            }
            return result;
        }


    


        /// <summary>
        /// بدست اوردن کل دقایق از فرمت رشته ای ساعت
        /// </summary>
        /// <param name="timeOne"></param>
        /// <returns></returns>
        public static string TotalMinute(this string timeOne)
        {
            string result = "0";
            try
            {
                if (timeOne != "00:00")
                {
                    timeOne = RoundTime(timeOne);
                    //TimeSpan conver = TimeSpan.Parse(timeOne);
                    //result = conver.TotalMinutes.ToString();
                    result = timeOne;
                }
            }
            catch (Exception e)
            {
                result = "0";
            }
            return result;
        }

        public static string RoundTime(string time)
        {
            string result = "0";
            try
            {
                if (time != "00:00")
                {
                    int hours = int.Parse(time.Split(':')[0]);
                    int minutes = int.Parse(time.Split(':')[1]);
                    int seconds = time.Length > 5 ? int.Parse(time.Split(':')[2]) : 0;
                    if (seconds > 30)
                    {
                        minutes = minutes + 1;
                        seconds = 0;
                    }
                    else
                    {
                        seconds = 0;
                    }

                    TimeSpan conver = TimeSpan.Parse($"{hours}:{minutes}:{seconds}");
                    result = conver.TotalMinutes.ToString();
                }
            }
            catch (Exception e)
            {
                result = "0";
            }
            return result;
        }


        /// <summary>
        /// بدست آوردن کل ساعات و دقیقه ها
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string TotalHours(this string minutes)
        {
            string result = "00:00";
            try
            {
                int minu = int.Parse(minutes);
                result = $"{minu / 60}:{minu % 60}";
            }
            catch (Exception e)
            {
                result = "0";
            }
            return result;
        }


        /// <summary>
        /// سال جاری را به شمسی میدهد
        /// </summary>
        /// <returns></returns>
        public static int ThisYear()
        {
            int year = 0;

            var dtNowGeo = DateTime.Now;
            var persianDate = DateTimeHelper.TopersianDate(dtNowGeo);

            year = int.Parse(persianDate.Split('/')[0]);

            return year;
        }

        /// <summary>
        /// سال گذشته را به شمسی میدهد
        /// </summary>
        /// <returns></returns>
        public static int LastYear()
        {
            return ThisYear() - 1;
        }




        /// <summary>
        /// تبدیل رشته زمانی به جئو
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan GlobalTimeFormat(this string time)
        {
            return TimeSpan.Parse(time);
        }


        /// <summary>
        /// زمان کل را محاسبه میکند
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeType"></param>
        /// <returns></returns>
        public static string Calculate(this List<OrdinaryWorkProgramTimeModel> model, TimeType timeType)
        {
            string result = "00:00";

            try
            {
                switch (timeType)
                {
                    case TimeType.WorkTime:
                        model = model.Where(x => x.TimeType == TimeType.WorkTime).ToList();
                        break;
                    case TimeType.OverTimeStart:
                        model = model.Where(x => x.TimeType == TimeType.OverTimeStart).ToList();
                        break;
                    case TimeType.OverTimeEnd:
                        model = model.Where(x => x.TimeType == TimeType.OverTimeEnd).ToList();
                        break;
                    case TimeType.RestTime:
                        model = model.Where(x => x.TimeType == TimeType.RestTime).ToList();
                        break;
                    default:
                        break;
                }
                foreach (var time in model)
                {
                    result = (TimeSpan.Parse(result) + TimeSpan.Parse(Duration(time.StartTime, time.EndTime))).ToString();
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }


        /// <summary>
        /// بدست آوردن نام روز
        /// </summary>
        /// <param name="dayIndex"></param>
        /// <returns></returns>
        public static string GetDayName(int dayIndex)
        {
            try
            {
                switch (dayIndex)
                {
                    case 1:
                    case 1 + 7:
                    case 1 + (2 * 7):
                    case 1 + (3 * 7):
                    case 1 + (4 * 7):
                    case 1 + (5 * 7):
                    case 1 + (6 * 7):
                    case 1 + (7 * 7):
                    case 1 + (8 * 7):
                    case 1 + (9 * 7):
                    case 1 + (11 * 7):
                    case 1 + (12 * 7):
                    case 1 + (13 * 7):
                        return "شنبه";
                    case 2:
                    case 2 + 7:
                    case 2 + (2 * 7):
                    case 2 + (3 * 7):
                    case 2 + (4 * 7):
                    case 2 + (5 * 7):
                    case 2 + (6 * 7):
                    case 2 + (7 * 7):
                    case 2 + (8 * 7):
                    case 2 + (9 * 7):
                    case 2 + (10 * 7):
                    case 2 + (11 * 7):
                    case 2 + (12 * 7):
                    case 2 + (13 * 7):
                        return "یکشنبه";
                    case 3:
                    case 3 + 7:
                    case 3 + (2 * 7):
                    case 3 + (3 * 7):
                    case 3 + (4 * 7):
                    case 3 + (5 * 7):
                    case 3 + (6 * 7):
                    case 3 + (7 * 7):
                    case 3 + (8 * 7):
                    case 3 + (9 * 7):
                    case 3 + (10 * 7):
                    case 3 + (11 * 7):
                    case 3 + (12 * 7):
                    case 3 + (13 * 7):
                        return "دوشنبه";
                    case 4:
                    case 4 + 7:
                    case 4 + (2 * 7):
                    case 4 + (3 * 7):
                    case 4 + (4 * 7):
                    case 4 + (5 * 7):
                    case 4 + (6 * 7):
                    case 4 + (7 * 7):
                    case 4 + (8 * 7):
                    case 4 + (9 * 7):
                    case 4 + (10 * 7):
                    case 4 + (11 * 7):
                    case 4 + (12 * 7):
                    case 4 + (13 * 7):
                        return "سه شنبه";
                    case 5:
                    case 5 + 7:
                    case 5 + (2 * 7):
                    case 5 + (3 * 7):
                    case 5 + (4 * 7):
                    case 5 + (5 * 7):
                    case 5 + (6 * 7):
                    case 5 + (7 * 7):
                    case 5 + (8 * 7):
                    case 5 + (9 * 7):
                    case 5 + (10 * 7):
                    case 5 + (11 * 7):
                    case 5 + (12 * 7):
                    case 5 + (13 * 7):
                        return "چهارشنبه";
                    case 6:
                    case 6 + 7:
                    case 6 + (2 * 7):
                    case 6 + (3 * 7):
                    case 6 + (4 * 7):
                    case 6 + (5 * 7):
                    case 6 + (6 * 7):
                    case 6 + (7 * 7):
                    case 6 + (8 * 7):
                    case 6 + (9 * 7):
                    case 6 + (10 * 7):
                    case 6 + (11 * 7):
                    case 6 + (12 * 7):
                    case 6 + (13 * 7):
                        return "پنجشنبه";
                    case 7:
                    case 7 + 7:
                    case 7 + (2 * 7):
                    case 7 + (3 * 7):
                    case 7 + (4 * 7):
                    case 7 + (5 * 7):
                    case 7 + (6 * 7):
                    case 7 + (7 * 7):
                    case 7 + (8 * 7):
                    case 7 + (9 * 7):
                    case 7 + (10 * 7):
                    case 7 + (11 * 7):
                    case 7 + (12 * 7):
                    case 7 + (13 * 7):
                        return "جمعه";
                    default:
                        return "-";
                }
            }
            catch (Exception e)
            {
            }
            return "-";
        }

        /// <summary>
        /// بازه را بدست میاورد
        /// </summary>
        /// <param name="timeList"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool DuratinAllow(this List<OrdinaryWorkProgramTimeModel> timeList, string startTime, string endTime)
        {
            bool result = true;
            try
            {
                if (timeList == null || timeList.Count() <= 0)
                    result = true;
                else
                {
                    TimeSpan time1 = GlobalTimeFormat(startTime);
                    TimeSpan time2 = GlobalTimeFormat(endTime);

                    foreach (var time in timeList)
                    {
                        TimeSpan innerTime1 = GlobalTimeFormat(time.StartTime);
                        TimeSpan innerTime2 = GlobalTimeFormat(time.EndTime);

                        if ((innerTime1 <= time1 && time1 <= innerTime2) || (innerTime1 <= time2 && time2 <= innerTime2)
                            || (innerTime1 <= time1 && time1 <= innerTime2 && innerTime1 <= time2 && time2 <= innerTime2))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// بازه را بدست میاورد
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeType"></param>
        /// <returns></returns>
        public static string DuratinAllow(this OrdinaryWorkProgramModel model, TimeType timeType)
        {
            string result = "00:00";
            try
            {
                List<OrdinaryWorkProgramTimeModel> timeList = new List<OrdinaryWorkProgramTimeModel>();
                if (model.OrdinaryWorkProgramDayModels != null && model.OrdinaryWorkProgramDayModels.Count() > 0)
                {
                    foreach (var day in model.OrdinaryWorkProgramDayModels)
                    {
                        var list = day.OrdinaryWorkProgramTimeModels.Where(x => x.TimeType == timeType).ToList();

                        TimeSpan durationMinutes = TimeSpan.FromMinutes(0);
                        foreach (var time in list)
                        {
                            if (time.Duration.Length > 4)
                                time.Duration = time.Duration.Substring(0, 5);

                            string oo = time.Duration[0].ToString();
                            string one = time.Duration[1].ToString();
                            string th = time.Duration[3].ToString();
                            string fore = time.Duration[4].ToString();

                            int totalMinutes = int.Parse($"{oo}{one}") * 60;
                            totalMinutes = totalMinutes + int.Parse($"{th}{fore}");



                            string ooPrime = result[0].ToString();
                            string onePrime = result[1].ToString();
                            string thPrime = result[3].ToString();
                            string forePrime = result[4].ToString();

                            totalMinutes = totalMinutes + int.Parse($"{ooPrime}{onePrime}") * 60;
                            totalMinutes = totalMinutes + int.Parse($"{thPrime}{forePrime}");


                            durationMinutes = TimeSpan.FromMinutes(totalMinutes);

                            result = string.Format("{0:00}:{1:00}", (int)durationMinutes.TotalHours, durationMinutes.Minutes);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = "00:00";
            }
            return result;
        }




        public static DayInWeek GetDayInWeek(DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return DayInWeek.Sunday;
                case DayOfWeek.Monday:
                    return DayInWeek.Monday;
                case DayOfWeek.Tuesday:
                    return DayInWeek.Tuesday;
                case DayOfWeek.Wednesday:
                    return DayInWeek.Wednesday;
                case DayOfWeek.Thursday:
                    return DayInWeek.Thursday;
                case DayOfWeek.Friday:
                    return DayInWeek.Friday;
                case DayOfWeek.Saturday:
                    return DayInWeek.Saturday;
                default:
                    return DayInWeek.Saturday;
            }
        }
    }
}
