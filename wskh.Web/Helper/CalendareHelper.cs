using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TimeAttendance.Model;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.Web.Helper
{
    public static class CalendareHelper
    {
        public static string GetMonth(int i)
        {
            switch (i)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "-";
            }
        }
        public static string DayType(int month, int day)
        {
            if ((12 > month && month > 6) && day > 30)
                return "NoDay";
            else if (month == 12 && day > 29)
                return "NoDay";
            else
                return "HasDay customeTDItem";
        }
        public static string DayKind(List<CalendarDayModel> dayModels, int month, int day)
        {
            string result = "NoDateCol";

            if ((month >= 7 && month < 12) & (day > 30))
                result = "-";
            else if (month == 12 && day > 29)
                result = "NoDateCol";
            else
            {
                if (dayModels != null && dayModels.Count() > 0)
                {
                    int year = dayModels != null && dayModels.Count() > 0 ? int.Parse(dayModels.FirstOrDefault().StartDate.Split('/')[0]) : int.Parse(DateTimeHelper.TopersianDate(DateTime.Now).Split('/')[0]);
                    string persianDate = $"{year}/{month}/{day}";
                    DateTime cutomeDate = DateTimeHelper.ToGeoDate(persianDate).GetValueOrDefault();

                    try
                    {
                        if (dayModels.Where(x => x.StartDateGeo.Date == cutomeDate.Date).Count() > 0)
                            result = "StartDateCol";
                        else if (dayModels.Where(x => x.EndDateGeo.Date == cutomeDate.Date).Count() > 0)
                            result = "EndDateCol";
                        else if (dayModels.Where(x => x.StartDateGeo.Date < cutomeDate.Date && cutomeDate.Date < x.EndDateGeo.Date).Count() > 0)
                            result = "MiddleDateCol";
                        else
                            result = "NoDateCol";
                    }
                    catch (Exception e)
                    {
                        result = "NoDateCol";
                    }
                }
                else
                    result = "NoDateCol";
            }


            return result;
        }
        public static string PersianName(this DateTime geoDate)
        {
            string result = "-";
            try
            {
                PersianCalendar pc = new PersianCalendar();
                #region Get Day
                result = pc.GetDayOfMonth(geoDate).ToString();
                #endregion
                #region Get Months
                switch (pc.GetMonth(geoDate))
                {
                    case 1:
                        result = result + " " + "فروردین";
                        break;
                    case 2:
                        result = result + " " + "اردیبهشت";
                        break;
                    case 3:
                        result = result + " " + "خرداد";
                        break;
                    case 4:
                        result = result + " " + "تیر";
                        break;
                    case 5:
                        result = result + " " + "مرداد";
                        break;
                    case 6:
                        result = result + " " + "شهریور";
                        break;
                    case 7:
                        result = result + " " + "مهر";
                        break;
                    case 8:
                        result = result + " " + "آبان";
                        break;
                    case 9:
                        result = result + " " + "آذر";
                        break;
                    case 10:
                        result = result + " " + "دی";
                        break;
                    case 11:
                        result = result + " " + "بهمن";
                        break;
                    case 12:
                        result = result + " " + "اسفند";
                        break;
                    default:
                        result = result + " " + "-";
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                result = "-";
            }
            return result;
        }
    }
}