using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.WebEssentials.ReportPart
{
    public static class AnalyzedReportHelper
    {

        /// <summary>
        /// مجموع کل تعجیل ورودها
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalEarlyEnter(this List<AnalyzedReport> analyzedReports)
        {
            string result = "00:00";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int convert = string.IsNullOrEmpty(time.EarlyEnteranceTime) ? 0 : int.Parse(DateTimeHelper.TimeToMinute(time.EarlyEnteranceTime));

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }


        /// <summary>
        /// مجموع کل تاخیرهای ورود
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalDelayEnter(this List<AnalyzedReport> analyzedReports)
        {
            string result = "00:00";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int convert = string.IsNullOrEmpty(time.DelayEnterTime) ? 0 : int.Parse(DateTimeHelper.TimeToMinute(time.DelayEnterTime));

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }


        /// <summary>
        /// مجموع کل تعجیل خروج
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalEarlyExit(this List<AnalyzedReport> analyzedReports)
        {
            string result = "00:00";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int convert = string.IsNullOrEmpty(time.EarlyExit) ? 0 :  int.Parse(DateTimeHelper.TimeToMinute(time.EarlyExit));

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }


        /// <summary>
        /// مجموع کل تاخیرهای خروج
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalDelayExit(this List<AnalyzedReport> analyzedReports)
        {
            string result = "00:00";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int convert = string.IsNullOrEmpty(time.DelayExitTime) ? 0 : int.Parse(DateTimeHelper.TimeToMinute(time.DelayExitTime));

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }



        /// <summary>
        /// مجموع کل رمان کاری
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalWorkTime(this List<AnalyzedReport> analyzedReports)
        {
            string result = "00:00";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int convert = string.IsNullOrEmpty(time.TotalWorkTime) ? 0 : int.Parse(DateTimeHelper.TimeToMinute(time.TotalWorkTime));

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }


        /// <summary>
        /// کل زمان حضور واقعی
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalRealWorkTime(this List<AnalyzedReport> analyzedReports)
        {
            string result = "00:00";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int convert = string.IsNullOrEmpty(time.RealTotalWorkTime) ? 0 : int.Parse(DateTimeHelper.TimeToMinute(time.RealTotalWorkTime));

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }



        /// <summary>
        /// کسر کار
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string LowWorkTme(this List<AnalyzedReport> analyzedReports)
        {
            string result = "0";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int delayEnterTime = int.Parse(wskh.WebEssentials.DateAndTime.DateTimeHelper.TimeToMinute(time.DelayEnterTime));
                        int earlyExitTime = int.Parse(wskh.WebEssentials.DateAndTime.DateTimeHelper.TimeToMinute(time.EarlyExit));

                        int convert = delayEnterTime + earlyExitTime;

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }


        /// <summary>
        /// اضافه کار
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string HighWorkTme(this List<AnalyzedReport> analyzedReports)
        {
            string result = "0";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int earlyEnterTime = int.Parse(wskh.WebEssentials.DateAndTime.DateTimeHelper.TimeToMinute(time.EarlyEnteranceTime));
                        int delayExitTime = int.Parse(wskh.WebEssentials.DateAndTime.DateTimeHelper.TimeToMinute(time.DelayExitTime));

                        int convert = earlyEnterTime + delayExitTime;

                        counter = counter + convert;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }




        /// <summary>
        /// اضافه کار اول وقت
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalEarlyEnteranceTIme(this List<AnalyzedReport> analyzedReports)
        {
            string result = "0";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int delayEnterTime = int.Parse(wskh.WebEssentials.DateAndTime.DateTimeHelper.TimeToMinute(time.TotalEarlyEnterance));

                        counter = counter + delayEnterTime;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }


        /// <summary>
        /// اضافه کار آخر وقت
        /// </summary>
        /// <param name="analyzedReports"></param>
        /// <returns></returns>
        public static string TotalDelayExitTIme(this List<AnalyzedReport> analyzedReports)
        {
            string result = "0";

            if (analyzedReports != null && analyzedReports.Count() > 0)
            {
                int counter = 0;

                foreach (var time in analyzedReports)
                {
                    try
                    {
                        int delayEnterTime = int.Parse(wskh.WebEssentials.DateAndTime.DateTimeHelper.TimeToMinute(time.TotalDelayExit));

                        counter = counter + delayEnterTime;
                    }
                    catch (Exception e)
                    {
                    }
                }
                result = DateTimeHelper.TotalTimes(counter);
            }

            return result;
        }
    }
}
