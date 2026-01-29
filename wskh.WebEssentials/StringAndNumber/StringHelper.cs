using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.WebEssentials.StringAndNumber
{
    public static class StringHelper
    {
        public static string IfNull(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "-";
            else
                return str;
        }

        public static string IfNull(this string str, string defaultValue)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;
            else
                return str;
        }


        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str) ? true : false;
        }


        public static bool IsOdd(this int count)
        {
            if (count == 0 || count % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static string Success(this string str)
        {
            return $"<span class='text-success'>{str}</span>";
        }
        public static string Danger(this string str)
        {
            return $"<span class='text-danger'>{str}</span>";
        }
        public static string Info(this string str)
        {
            return $"<span class='text-info'>{str}</span>";
        }
        public static string Warning(this string str)
        {
            return $"<span class='text-warning'>{str}</span>";
        }
        public static string Dark(this string str)
        {
            return $"<span class='text-dark'>{str}</span>";
        }
    }
}
