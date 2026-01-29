using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wskh.Core.Enumerator;

namespace TimeAttendance.Web.Helper
{
    public static class EnumHelper
    {
        public static string FunctionKeyToString(this FunctionKeyType type)
        {
            switch (type)
            {
                case FunctionKeyType.Enterance:
                    return "ورود";
                case FunctionKeyType.Exit:
                    return "خروج";
                case FunctionKeyType.HourlyLeave:
                    return "مرخصی ساعتی";
                case FunctionKeyType.HourlyMission:
                    return "ماموریت ساعتی";
                case FunctionKeyType.TransportDelay:
                    return "تاخیر سرویس";
                case FunctionKeyType.Rest:
                    return "نماز، نهار، استراحت";
                case FunctionKeyType.ChildRest:
                    return "پاس شیر";
                case FunctionKeyType.Other:
                    return "غیره...";
                default:
                    return "-";
            }
        }
        public static string WorkTypeToString(this WorkType type)
        {
            switch (type)
            {
                case WorkType.WorkDay:
                    return "روزکاری";
                case WorkType.Holiday:
                    return "تعطیلی";
                case WorkType.RestDay:
                    return "روز استراحت";
                case WorkType.HolidayAndSpecialDay:
                    return "ایام خاص";
                case WorkType.Other:
                    return "-";
                default:
                    return "-";
            }
        }
    }
}