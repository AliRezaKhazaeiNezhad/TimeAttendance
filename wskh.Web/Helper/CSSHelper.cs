using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAttendance.Model;

namespace TimeAttendance.Web.Helper
{
    public static class CSSHelper
    {
        public static string GetDayTypeColor(OrdinaryWorkProgramDayModel day)
        {
            switch (day.WorkType)
            {
                case wskh.Core.Enumerator.WorkType.WorkDay:
                    return "bgDefault";
                case wskh.Core.Enumerator.WorkType.Holiday:
                    return "bgRed";
                case wskh.Core.Enumerator.WorkType.RestDay:
                    return "bgOrange";
                case wskh.Core.Enumerator.WorkType.HolidayAndSpecialDay:
                    return "bgDefault";
                default:
                    return "bgDefault";
            }
        }
    }
}