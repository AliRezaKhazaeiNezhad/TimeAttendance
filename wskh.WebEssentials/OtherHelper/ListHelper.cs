using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.WebEssentials.OtherHelper
{
    public static class ListHelper
    {
        public static bool IsListNull<T>(this List<T> list)
        {
            return list == null || list.Count() <= 0 ? true : false;
        }
    }
}
