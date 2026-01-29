using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.Web.Helper
{
    public static class WebConfigHelper
    {
        public static string ConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["wskhContext"].ToString();
            return connectionString;
        }

        public static List<string> Bigyears()
        {
            string BigYears = ConfigurationManager.AppSettings["BigYear"];
            return BigYears.Split(';').ToList();
        }

        public static List<string> PersianYears()
        {
            string BigYears = ConfigurationManager.AppSettings["PersianYears"];
            List<string> finalList = new List<string>();
            List<string> list = BigYears.Split(';').ToList();
            int CurrentYear = int.Parse(DateTimeHelper.TopersianDate(DateTime.Now).Split('/')[0]) - 1;
            foreach (var item in list)
            {
                if (int.Parse(item) >= CurrentYear && finalList.Count() <=10)
                    finalList.Add(item);
            }

            return finalList;
        }
    }
}