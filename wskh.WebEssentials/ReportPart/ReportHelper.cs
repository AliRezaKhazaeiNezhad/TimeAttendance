using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using TimeAttendance.WebEssentials.DateAndTime;
using TimeAttendance.WebEssentials.OtherHelper;
using TimeAttendance.WebEssentials.StringAndNumber;
using wskh.WebEssentials.DateAndTime;

namespace TimeAttendance.WebEssentials.ReportHelper
{
    public static class ReportHelper
    {
        public static Tuple<bool, string> IsOff(this List<Request> requsets, DateTime dateTime, string userId)
        {
            bool result = false;
            string message = "-";
            try
            {
                if (!ListHelper.IsListNull(requsets))
                {
                    requsets = requsets
                        .Where(x => x.UserRequesterId == userId
                                    && dateTime.Date <= x.StartDate.Date
                                    && x.EndDate.Date <= dateTime.Date
                                    && x.State == wskh.Core.Enumerator.RequestState.Approved
                                    && (x.Type == wskh.Core.Enumerator.RequestType.DailyRest || x.Type == wskh.Core.Enumerator.RequestType.MissionDaily))
                        .ToList();
                }


                if (!ListHelper.IsListNull(requsets))
                {
                    Request firstRequest = requsets.FirstOrDefault();
                    result = true;
                    switch (firstRequest.Type)
                    {
                        case wskh.Core.Enumerator.RequestType.DailyRest:
                            message = "مرخصی روزانه";
                            break;
                        case wskh.Core.Enumerator.RequestType.MissionDaily:
                            message = "ماموریت روزانه";
                            break;
                        default:
                            message = "-";
                            break;
                    }
                }

            }
            catch (Exception e)
            {
            }

            return Tuple.Create(result, message);
        }

        public static Tuple<string, string> Trades(this List<Log> logs, bool hasTable, int reportDayId, string userId)
        {
            string result = "";
            string result2 = "";
            try
            {
                if (reportDayId > 0)
                {
                    if (logs != null && logs.Count() > 0 && logs.Where(xf => xf.Remove == false).Count() > 0)
                    {
                        result = "<table class='table  table-bordered no-margin' style='width: 100 % !important;'><tbody>";
                        logs = logs.OrderBy(f => TimeHelper.GlobalTimeFormat(f.LogTime)).ToList();
                        if (!hasTable)
                        {
                            for (int i = 0; i < logs.Count(); i++)
                            {
                                if (StringHelper.IsOdd(i + 1))
                                {
                                    if (i + 2 <= logs.Count())
                                    {
                                        result = result + $"<tr><td  style='width: 30%;'>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</td><td style='width: 30%;'>{StringHelper.Danger(logs[i + 1].LogTime.ToHourMinute())}</td><td style='width: 40%;'>{StringHelper.Dark(DateTimeHelper.CalculateDuration(logs[i].LogTime, logs[i + 1].LogTime))}</td></tr>";
                                    }
                                    else
                                    {
                                        result = result + $"<tr><td  style='width: 30%;'>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</td><td style='width: 30%;'>-</td><td style='width: 40%;'>-</td></tr>";
                                    }
                                }
                            }
                        }
                        else
                        {
                            result = "<table class='table  table-bordered no-margin' style='width: 100 % !important;'><tbody>";

                            for (int i = 0; i < logs.Count(); i++)
                            {
                                if (StringHelper.IsOdd(i + 1))
                                {
                                    if (i + 2 <= logs.Count())
                                    {
                                        result = result + $"<tr><td  style='width: 30%;'>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</td><td style='width: 30%;'>{StringHelper.Danger(logs[i + 1].LogTime.ToHourMinute())}</td><td style='width: 40%;'>{StringHelper.Dark(DateTimeHelper.CalculateDuration(logs[i].LogTime, logs[i + 1].LogTime))}</td><th style='width: 10%;'><i class='fa fa-trash customeMargin pointer text-danger' onclick='DeleteTrade({logs[i].Id}, {logs[i + 1].Id})'></i></th></tr>";
                                    }
                                    else
                                    {
                                        result = result + $"<tr><td  style='width: 30%;'>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</td><td style='width: 30%;'> - </td><td style='width: 40%;'> - </td><th style='width: 10%;'><i class='fa fa-trash text-danger customeMargin pointer' onclick='DeleteTrade({logs[i].Id}, 0)'></i></th></tr>";
                                    }
                                }
                            }
                        }
                    }
                }


                result2 = $"<i class='fa fa-plus text-success customeMargin pointer'onclick='AddRowToUser({reportDayId}, \"{userId}\")'></i>";
            }
            catch (Exception e)
            {
            }
            return Tuple.Create(result, result2);
        }



        public static Tuple<string, string> RawTrades(this List<Log> logs, bool hasTable, int reportDayId, string userId)
        {
            string result = "";
            string result2 = "";
            try
            {
                if (reportDayId > 0)
                {
                    if (logs != null && logs.Count() > 0 && logs.Where(xf => xf.Remove == false).Count() > 0)
                    {
                        result = "<table class='table  table-bordered no-margin' style='width: 100 % !important;'><tbody>";
                        logs = logs.OrderBy(f => TimeHelper.GlobalTimeFormat(f.LogTime)).ToList();
                        result = "<table class='table  table-bordered no-margin' style='width: 100 % !important;'><tbody>";

                        for (int i = 0; i < logs.Count(); i++)
                        {
                            if (StringHelper.IsOdd(i + 1))
                            {
                                if (i + 2 <= logs.Count())
                                {
                                    result = result + $"<tr><td  style='width: 10%;'>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</td><td style='width: 10%;'>{StringHelper.Danger(logs[i + 1].LogTime.ToHourMinute())}</td><td style='width: 10%;'>{StringHelper.Dark(DateTimeHelper.CalculateDuration(logs[i].LogTime, logs[i + 1].LogTime))}</td><td style='width: 30%;'>{StringHelper.Dark(logs[i].Device.Title)}</td><td style='width: 30%;'>{StringHelper.Dark(logs[i + 1].Device.Title)}</td><td style='width: 10%;'><i class='fa fa-trash customeMargin pointer text-danger' onclick='DeleteTrade({logs[i].Id}, {logs[i + 1].Id})'></i></td></tr>";
                                }
                                else
                                {
                                    result = result + $"<tr class='table-danger'><td  style='width: 10%;'>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</td><td style='width: 10%;'> - </td><td style='width: 10%;'> - </td><td style='width: 30%;'>{StringHelper.Dark(logs[i].Device.Title)}</td><td style='width: 30%;'> - </td><td style='width: 10%;'><i class='fa fa-trash text-danger customeMargin pointer' onclick='DeleteTrade({logs[i].Id}, 0)'></i></td></tr>";
                                }
                            }
                        }
                    }
                }


                result2 = $"<i class='fa fa-plus text-success customeMargin pointer'onclick='AddRowToUser({reportDayId}, \"{userId}\")'></i>";
            }
            catch (Exception e)
            {
            }
            return Tuple.Create(result, result2);
        }






        public static Tuple<string, string> PDFRawTrades(this List<Log> logs, bool hasTable, int reportDayId, string userId)
        {
            string result = "";
            string result2 = "";
            try
            {
                if (reportDayId > 0)
                {
                    if (logs != null && logs.Count() > 0 && logs.Where(xf => xf.Remove == false).Count() > 0)
                    {
                        result = "<table class='table  table-bordered no-margin' style='width: 100 % !important;'>";
                        logs = logs.OrderBy(f => TimeHelper.GlobalTimeFormat(f.LogTime)).ToList();

                        for (int i = 0; i < logs.Count(); i++)
                        {
                            if (StringHelper.IsOdd(i + 1))
                            {
                                if (i + 2 <= logs.Count())
                                {
                                    result = result + $"<div>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</div><div>{StringHelper.Danger(logs[i + 1].LogTime.ToHourMinute())}</div><div >{StringHelper.Dark(DateTimeHelper.CalculateDuration(logs[i].LogTime, logs[i + 1].LogTime))}</div><div >{StringHelper.Dark(logs[i].Device.Title)}</div><div >{StringHelper.Dark(logs[i + 1].Device.Title)}</div>";
                                }
                                else
                                {
                                    result = result + $"<div><div>{StringHelper.Success(logs[i].LogTime.ToHourMinute())}</div><div> - </div><div> - </div><div>{StringHelper.Dark(logs[i].Device.Title)}</div><div> - </div></div>";
                                }
                            }
                        }
                    }
                }


                result2 = $"{result}</tbody>";
            }
            catch (Exception e)
            {
            }
            return Tuple.Create(result, result2);
        }
    }
}
