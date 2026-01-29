using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.ReportHelper;
using TimeAttendance.WebEssentials.ReportPart;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.ReportModel;
using wskh.WebEssentials.DateAndTime;
using static System.Net.WebRequestMethods;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class StimulReportController : Controller
    {
        #region TradeReprot - گزارش خام تردد
        public ActionResult TradeReportIndex(int groupId = 0, string userId = "", string startDate = "", string endDate = "", int tradeTypeId = 0)
        {
            Session["groupId"] = groupId;
            Session["userId"] = userId;
            Session["startDate"] = startDate;
            Session["endDate"] = endDate;
            Session["tradeTypeId"] = tradeTypeId;

            return View();
        }

        public ActionResult TradeReport()
        {
            int groupId = int.Parse(Session["groupId"].ToString());
            string userId = Session["userId"].ToString();
            string startDate = Session["startDate"].ToString();
            string endDate = Session["endDate"].ToString();
            int tradeTypeId = int.Parse(Session["tradeTypeId"].ToString());


            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();
            var reportList = _reportDayService.GetList;
            var userList = _userService.GetList;

            if (reportList != null && reportList.Count() > 0)
            {
                reportList = reportList.Where(x => x.ReportDate >= DateTimeHelper.ToGeoDate(startDate) && x.ReportDate <= DateTimeHelper.ToGeoDate(endDate)).ToList();

                if (userId == "null" || userId == "0")
                    userId = "";

                if (!string.IsNullOrEmpty(userId))
                    reportList = reportList.Where(x => x.UserId == userId).ToList();
            }


            if (userList != null && userList.Count() > 0 && !string.IsNullOrEmpty(userId))
                userList = userList.Where(x => x.Id == userId).ToList();

            if (userList != null && userList.Count() > 0)
                userList = userList.OrderBy(x => x.Lastname).ToList();

            #endregion

            #region Fill Base Model

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش خام تردد";
            model.PrintDate = DateTimeHelper.TopersianDate(DateTime.Now);
            model.StartDate = DateTimeHelper.TopersianDate(DateTimeHelper.ToGeoDate(startDate).GetValueOrDefault());
            model.EndDate = DateTimeHelper.TopersianDate(DateTimeHelper.ToGeoDate(endDate).GetValueOrDefault());
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;


            foreach (var user in userList)
            {
                BasicInfo userModel = new BasicInfo();

                userModel.Index = userIndex.ToString();

                userModel.GroupName = user.UserGroup.Title;

                userModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(user.NationalCode);

                userModel.TotalTime = "0";


                var seondReportList = new List<ReportDay>();

                if (reportList != null && reportList.Count() > 0)
                {
                    seondReportList = reportList.Where(x => x.Remove == false && x.UserId == user.Id).ToList();
                }



                if (seondReportList != null && seondReportList.Count() > 0)
                {
                    int tradeIndex = 1;

                    foreach (var reportItem in seondReportList)
                    {
                        Trade tradeModel = new Trade();

                        tradeModel.Index = tradeIndex.ToString();

                        tradeModel.PersianDate = reportItem.PersianDate;

                        tradeModel.PersianDayName = reportItem.PersianDayName;


                        if (reportItem.Logs != null && reportItem.Logs.Count() > 0)
                        {
                            reportItem.Logs = reportItem.Logs.Where(x => x.Remove == false).ToList();
                        }


                        if (reportItem.Logs != null && reportItem.Logs.Count() > 0)
                        {
                            int logIndex = 0;
                            int logCount = reportItem.Logs.Count();

                            for (int i = 0; i <= logCount; i++)
                            {
                                TradeLog tradeLogModel = new TradeLog();

                                var firstLog = new Log();
                                var secondLog = new Log();

                                if (logIndex < logCount && reportItem.Logs[logIndex] != null)
                                {
                                    firstLog = reportItem.Logs[logIndex];

                                    tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                                    tradeLogModel.DeviceOne = firstLog.Device.Title;

                                    logIndex = 1 + logIndex;
                                }

                                if (logIndex < logCount && reportItem.Logs[logIndex] != null)
                                {
                                    secondLog = reportItem.Logs[logIndex];

                                    tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                                    tradeLogModel.DeviceTwo = secondLog.Device.Title;

                                    logIndex = 1 + logIndex;
                                }

                                if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                                {
                                    tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                                    string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                                    string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                                    int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                                    userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                                }
                                else
                                {
                                    tradeLogModel.Duration = "00:00";
                                }

                                tradeModel.TradeLogs.Add(tradeLogModel);

                            }
                        }


                        userModel.Trades.Add(tradeModel);

                        tradeIndex = 1 + tradeIndex;
                    }
                }


                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }



            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion

            var report = new StiReport();
            report.Compile();
            report.Load(Server.MapPath("/Reports/TradeReport.mrt"));
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }



        public ActionResult TradeReportNoLayoutIndex(int groupId = 0, string userId = "", string startDate = "", string endDate = "", int tradeTypeId = 0)
        {
            Session["groupId"] = groupId;
            Session["userId"] = userId;
            Session["startDate"] = startDate;
            Session["endDate"] = endDate;
            Session["tradeTypeId"] = tradeTypeId;

            return View();
        }

        public ActionResult TradeReportNoLayout()
        {
            int groupId = int.Parse(Session["groupId"].ToString());
            string userId = Session["userId"].ToString();
            string startDate = Session["startDate"].ToString();
            string endDate = Session["endDate"].ToString();
            int tradeTypeId = int.Parse(Session["tradeTypeId"].ToString());


            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();
            var reportList = _reportDayService.GetList;
            var userList = _userService.GetList;

            if (reportList != null && reportList.Count() > 0)
            {
                reportList = reportList.Where(x => x.ReportDate >= DateTimeHelper.ToGeoDate(startDate) && x.ReportDate <= DateTimeHelper.ToGeoDate(endDate)).ToList();

                if (userId == "null" || userId == "0")
                    userId = "";

                if (!string.IsNullOrEmpty(userId))
                    reportList = reportList.Where(x => x.UserId == userId).ToList();
            }


            if (userList != null && userList.Count() > 0 && !string.IsNullOrEmpty(userId))
                userList = userList.Where(x => x.Id == userId).ToList();

            if (userList != null && userList.Count() > 0)
                userList = userList.OrderBy(x => x.Lastname).ToList();

            #endregion

            #region Fill Base Model

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش خام تردد";
            model.PrintDate = DateTimeHelper.TopersianDate(DateTime.Now);
            model.StartDate = DateTimeHelper.TopersianDate(DateTimeHelper.ToGeoDate(startDate).GetValueOrDefault());
            model.EndDate = DateTimeHelper.TopersianDate(DateTimeHelper.ToGeoDate(endDate).GetValueOrDefault());
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;


            foreach (var user in userList)
            {
                BasicInfo userModel = new BasicInfo();

                userModel.Index = userIndex.ToString();

                userModel.GroupName = user.UserGroup.Title;

                userModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(user.NationalCode);

                userModel.TotalTime = "0";


                var seondReportList = new List<ReportDay>();

                if (reportList != null && reportList.Count() > 0)
                {
                    seondReportList = reportList.Where(x => x.Remove == false && x.UserId == user.Id).ToList();
                }



                if (seondReportList != null && seondReportList.Count() > 0)
                {
                    int tradeIndex = 1;

                    foreach (var reportItem in seondReportList)
                    {
                        Trade tradeModel = new Trade();

                        tradeModel.Index = tradeIndex.ToString();

                        tradeModel.PersianDate = reportItem.PersianDate;

                        tradeModel.PersianDayName = reportItem.PersianDayName;


                        if (reportItem.Logs != null && reportItem.Logs.Count() > 0)
                        {
                            reportItem.Logs = reportItem.Logs.Where(x => x.Remove == false).ToList();
                        }


                        if (reportItem.Logs != null && reportItem.Logs.Count() > 0)
                        {
                            int logIndex = 0;
                            int logCount = reportItem.Logs.Count();

                            for (int i = 0; i <= logCount; i++)
                            {
                                TradeLog tradeLogModel = new TradeLog();

                                var firstLog = new Log();
                                var secondLog = new Log();

                                if (logIndex < logCount && reportItem.Logs[logIndex] != null)
                                {
                                    firstLog = reportItem.Logs[logIndex];

                                    tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                                    tradeLogModel.DeviceOne = firstLog.Device.Title;

                                    logIndex = 1 + logIndex;
                                }

                                if (logIndex < logCount && reportItem.Logs[logIndex] != null)
                                {
                                    secondLog = reportItem.Logs[logIndex];

                                    tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                                    tradeLogModel.DeviceTwo = secondLog.Device.Title;

                                    logIndex = 1 + logIndex;
                                }

                                if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                                {
                                    tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                                    string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                                    string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                                    int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                                    userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                                }
                                else
                                {
                                    tradeLogModel.Duration = "00:00";
                                }

                                tradeModel.TradeLogs.Add(tradeLogModel);

                            }
                        }


                        userModel.Trades.Add(tradeModel);

                        tradeIndex = 1 + tradeIndex;
                    }
                }


                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }



            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion

            var report = new StiReport();
            report.Compile();
            report.Load(Server.MapPath("/Reports/TradeReportNoLayout.mrt"));
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region TradeReprot - گزارش لحظه ای کل
        public ActionResult InstantReportIndex(int userGroupId = 0, string userId = "")
        {
            Session["userGroupId"] = userGroupId;
            Session["userId"] = userId;

            return View();
        }

        public ActionResult InstantReport()
        {

            int userGroupId = int.Parse(Session["userGroupId"].ToString());
            string userId = Session["userId"].ToString();

            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.UserGroupId == userGroupId).ToList();

            if (list != null)
                list = list.OrderBy(x => x.NationalCode).ToList();

            #endregion

            #region Fill Base Model

            var cureDate = DateTime.Now;

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش لحظه ای کل";
            model.PrintDate = $"{DateTimeHelper.TopersianDate(DateTime.Now)}  {cureDate.Hour}:{cureDate.Minute}";
            model.StartDate = DateTimeHelper.TopersianDate(cureDate);
            model.EndDate = DateTimeHelper.TopersianDate(cureDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            int totalOne = list.Count();
            int totalTwo = 0;
            int totalThree = 0;
            int totalFoure = 0;


            foreach (var item in list)
            {
                BasicInfo userModel = new BasicInfo();

                userModel.Index = userIndex.ToString();

                userModel.GroupName = item.UserGroup.Title;

                userModel.NameAndFamily = $"{item.FirstName} {item.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(item.NationalCode);

                userModel.Branch = item.OrganizationBranch != null ? item.OrganizationBranch.Title : "-";

                userModel.TotalTime = "-";


                var reportDay = item;


                var logs = reportDay.Logs;

                if (logs != null && logs.Count() > 0)
                {
                    logs = logs.Where(f => f.Remove == false).ToList();
                }

                if (item.Logs == null)
                {
                    item.Logs = new List<Log>();

                    userModel.TotalTime = $"غایب";
                }
                else
                {
                    if (item.Logs.Count() == 0)
                    {
                        userModel.TotalTime = $"غایب";
                        totalTwo = 1 + totalTwo;
                    }
                    else
                    {
                        if (item.Logs.Count() % 2 == 0)
                        {
                            userModel.TotalTime = $"خارج از مجموعه";
                            totalFoure = 1 + totalFoure;
                        }
                        else
                        {
                            userModel.TotalTime = $"حاضر";
                            totalThree = 1 + totalThree;
                        }
                    }
                }


                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                }

                Trade trde = new Trade();

                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    int logIndex = 0;
                    int logCount = item.Logs.Count();

                    for (int i = 0; i <= logCount; i++)
                    {
                        TradeLog tradeLogModel = new TradeLog();

                        var firstLog = new Log();
                        var secondLog = new Log();


                        tradeLogModel.TradeTwo = "-";
                        tradeLogModel.DeviceTwo = "-";
                        tradeLogModel.DeviceOne = "-";
                        tradeLogModel.DeviceTwo = "-";


                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            firstLog = item.Logs[logIndex];

                            tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                            tradeLogModel.DeviceOne = firstLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            secondLog = item.Logs[logIndex];

                            tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                            tradeLogModel.DeviceTwo = secondLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                        {
                            tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                            string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                            string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                            int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                            userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                        }
                        else
                        {
                            tradeLogModel.Duration = "00:00";
                        }

                        trde.TradeLogs.Add(tradeLogModel);


                    }

                    userModel.Trades.Add(trde);
                }

                if (trde.TradeLogs.Count() <= 0)
                {
                    trde.TradeLogs.Add(new TradeLog()
                    {
                        DeviceOne = "-",
                        DeviceTwo = "-",
                        TradeOne = "-",
                        TradeTwo = "-",
                    });
                }

                userModel.Trades.Add(trde);

                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }

            model.TotalOne = totalOne.ToString();
            model.TotalTwo = totalTwo.ToString();
            model.TotalThree = totalThree.ToString();
            model.TotalFour = totalFoure.ToString();

            #endregion

            #region داده خالی باشد

            if (model.BasicInfos.Count() <= 0)
            {
                model.TotalThree = "0";
                model.BasicInfos.Add(new BasicInfo()
                {
                    Index = "-",
                    PersonalCode = "-",
                    NameAndFamily = "-",
                    GroupName = "-",
                    Branch = "-",
                    TotalTime = "-",
                    Trades = new List<Trade>() {
                        new Trade(){
                            Index = "-",
                            PersianDate = "-",
                            PersianDayName = "-",
                            State = "-",
                            TradeLogs = new List<TradeLog>(){
                                new TradeLog(){
                                    DeviceOne = "-",
                                    DeviceTwo = "-",
                                    TradeOne = "-",
                                    TradeTwo = "-",
                                }
                            }
                        }
                    }
                });
            }

            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion


            var dtNow = DateTime.Now;
            model.PrintDate = $"{DateTimeHelper.TopersianDate(dtNow)} {dtNow.Hour}:{dtNow.Minute} ";

            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/InstantReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }


        public ActionResult InstantNoLayoutIndex(int userGroupId = 0, string userId = "")
        {
            Session["userGroupId"] = userGroupId;
            Session["userId"] = userId;

            return View();
        }

        public ActionResult InstantNoLayoutReport()
        {

            int userGroupId = int.Parse(Session["userGroupId"].ToString());
            string userId = Session["userId"].ToString();

            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.UserGroupId == userGroupId).ToList();

            if (list != null)
                list = list.OrderBy(x => x.NationalCode).ToList();

            #endregion

            #region Fill Base Model

            var cureDate = DateTime.Now;

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش لحظه ای کل";
            model.PrintDate = $"{DateTimeHelper.TopersianDate(DateTime.Now)}  {cureDate.Hour}:{cureDate.Minute}";
            model.StartDate = DateTimeHelper.TopersianDate(cureDate);
            model.EndDate = DateTimeHelper.TopersianDate(cureDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            int totalOne = list.Count();
            int totalTwo = 0;
            int totalThree = 0;
            int totalFoure = 0;


            foreach (var item in list)
            {
                BasicInfo userModel = new BasicInfo();

                userModel.Index = userIndex.ToString();

                userModel.GroupName = item.UserGroup.Title;

                userModel.NameAndFamily = $"{item.FirstName} {item.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(item.NationalCode);

                userModel.Branch = item.OrganizationBranch != null ? item.OrganizationBranch.Title : "-";

                userModel.TotalTime = "-";


                var reportDay = item;


                var logs = reportDay.Logs;

                if (logs != null && logs.Count() > 0)
                {
                    logs = logs.Where(f => f.Remove == false).ToList();
                }

                if (item.Logs == null)
                {
                    item.Logs = new List<Log>();

                    userModel.TotalTime = $"غایب";
                }
                else
                {
                    if (item.Logs.Count() == 0)
                    {
                        userModel.TotalTime = $"غایب";
                        totalTwo = 1 + totalTwo;
                    }
                    else
                    {
                        if (item.Logs.Count() % 2 == 0)
                        {
                            userModel.TotalTime = $"خارج از مجموعه";
                            totalFoure = 1 + totalFoure;
                        }
                        else
                        {
                            userModel.TotalTime = $"حاضر";
                            totalThree = 1 + totalThree;
                        }
                    }
                }


                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                }

                Trade trde = new Trade();

                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    int logIndex = 0;
                    int logCount = item.Logs.Count();

                    for (int i = 0; i <= logCount; i++)
                    {
                        TradeLog tradeLogModel = new TradeLog();

                        var firstLog = new Log();
                        var secondLog = new Log();


                        tradeLogModel.TradeTwo = "-";
                        tradeLogModel.DeviceTwo = "-";
                        tradeLogModel.DeviceOne = "-";
                        tradeLogModel.DeviceTwo = "-";


                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            firstLog = item.Logs[logIndex];

                            tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                            tradeLogModel.DeviceOne = firstLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            secondLog = item.Logs[logIndex];

                            tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                            tradeLogModel.DeviceTwo = secondLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                        {
                            tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                            string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                            string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                            int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                            userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                        }
                        else
                        {
                            tradeLogModel.Duration = "00:00";
                        }

                        trde.TradeLogs.Add(tradeLogModel);


                    }

                    userModel.Trades.Add(trde);
                }

                if (trde.TradeLogs.Count() <= 0)
                {
                    trde.TradeLogs.Add(new TradeLog()
                    {
                        DeviceOne = "-",
                        DeviceTwo = "-",
                        TradeOne = "-",
                        TradeTwo = "-",
                    });
                }

                userModel.Trades.Add(trde);

                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }

            model.TotalOne = totalOne.ToString();
            model.TotalTwo = totalTwo.ToString();
            model.TotalThree = totalThree.ToString();
            model.TotalFour = totalFoure.ToString();

            #endregion

            #region داده خالی باشد

            if (model.BasicInfos.Count() <= 0)
            {
                model.TotalThree = "0";
                model.BasicInfos.Add(new BasicInfo()
                {
                    Index = "-",
                    PersonalCode = "-",
                    NameAndFamily = "-",
                    GroupName = "-",
                    Branch = "-",
                    TotalTime = "-",
                    Trades = new List<Trade>() {
                        new Trade(){
                            Index = "-",
                            PersianDate = "-",
                            PersianDayName = "-",
                            State = "-",
                            TradeLogs = new List<TradeLog>(){
                                new TradeLog(){
                                    DeviceOne = "-",
                                    DeviceTwo = "-",
                                    TradeOne = "-",
                                    TradeTwo = "-",
                                }
                            }
                        }
                    }
                });
            }

            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion


            var dtNow = DateTime.Now;
            model.PrintDate = $"{DateTimeHelper.TopersianDate(dtNow)} {dtNow.Hour}:{dtNow.Minute} ";

            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/InstantNoLayoutReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region TradeReprot - گزارش لحظه ای حاضرین
        public ActionResult InstantAudianceReportIndex(int userGroupId = 0, string userId = "")
        {
            Session["userGroupId"] = userGroupId;
            Session["userId"] = userId;

            return View();
        }

        public ActionResult InstantAudianceReport()
        {

            int userGroupId = int.Parse(Session["userGroupId"].ToString());
            string userId = Session["userId"].ToString();

            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.UserGroupId == userGroupId).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.Logs != null && x.Logs.Count() % 2 != 0).ToList();


            if (list != null)
                list = list.OrderBy(x => x.NationalCode).ToList();

            #endregion

            #region Fill Base Model

            var cureDate = DateTime.Now;

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش لحظه ای حاضرین";
            model.PrintDate = $"{DateTimeHelper.TopersianDate(DateTime.Now)}  {cureDate.Hour}:{cureDate.Minute}";
            model.StartDate = DateTimeHelper.TopersianDate(cureDate);
            model.EndDate = DateTimeHelper.TopersianDate(cureDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            int totalOne = list.Count();
            int totalTwo = 0;
            int totalThree = 0;
            int totalFoure = 0;


            foreach (var item in list)
            {
                var reportDay = item;

                var logs = reportDay.Logs;

                if (item.Logs.Count() % 2 != 0)
                {
                    BasicInfo userModel = new BasicInfo();

                    userModel.TotalTime = $"حاضر";


                    userModel.Index = userIndex.ToString();

                    userModel.GroupName = item.UserGroup.Title;

                    userModel.NameAndFamily = $"{item.FirstName} {item.Lastname}";

                    userModel.PersonalCode = HashHelper.Decrypt(item.NationalCode);

                    userModel.Branch = item.OrganizationBranch != null ? item.OrganizationBranch.Title : "-";

                    userModel.TotalTime = "-";



                    if (item.Logs != null && item.Logs.Count() > 0)
                    {
                        item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                    }

                    Trade trde = new Trade();

                    if (item.Logs != null && item.Logs.Count() > 0)
                    {
                        int logIndex = 0;
                        int logCount = item.Logs.Count();

                        for (int i = 0; i <= logCount; i++)
                        {
                            TradeLog tradeLogModel = new TradeLog();

                            var firstLog = new Log();
                            var secondLog = new Log();


                            tradeLogModel.TradeTwo = "-";
                            tradeLogModel.DeviceTwo = "-";
                            tradeLogModel.DeviceOne = "-";
                            tradeLogModel.DeviceTwo = "-";


                            if (logIndex < logCount && item.Logs[logIndex] != null)
                            {
                                firstLog = item.Logs[logIndex];

                                tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                                tradeLogModel.DeviceOne = firstLog.Device.Title;

                                logIndex = 1 + logIndex;
                            }

                            if (logIndex < logCount && item.Logs[logIndex] != null)
                            {
                                secondLog = item.Logs[logIndex];

                                tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                                tradeLogModel.DeviceTwo = secondLog.Device.Title;

                                logIndex = 1 + logIndex;
                            }

                            if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                            {
                                tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                                string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                                string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                                int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                                userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                            }
                            else
                            {
                                tradeLogModel.Duration = "00:00";
                            }

                            trde.TradeLogs.Add(tradeLogModel);


                        }

                        userModel.Trades.Add(trde);
                    }

                    if (trde.TradeLogs.Count() <= 0)
                    {
                        trde.TradeLogs.Add(new TradeLog()
                        {
                            DeviceOne = "-",
                            DeviceTwo = "-",
                            TradeOne = "-",
                            TradeTwo = "-",
                        });
                    }

                    userModel.Trades.Add(trde);

                    model.BasicInfos.Add(userModel);

                    userIndex = 1 + userIndex;
                }
            }

            model.TotalOne = model.BasicInfos.Count().ToString();
            model.TotalTwo = totalTwo.ToString();
            model.TotalThree = totalThree.ToString();
            model.TotalFour = totalFoure.ToString();

            #endregion

            #region داده خالی باشد

            if (model.BasicInfos.Count() <= 0)
            {
                model.TotalThree = "0";
                model.BasicInfos.Add(new BasicInfo()
                {
                    Index = "-",
                    PersonalCode = "-",
                    NameAndFamily = "-",
                    GroupName = "-",
                    Branch = "-",
                    TotalTime = "-",
                    Trades = new List<Trade>() {
                        new Trade(){
                            Index = "-",
                            PersianDate = "-",
                            PersianDayName = "-",
                            State = "-",
                            TradeLogs = new List<TradeLog>(){
                                new TradeLog(){
                                    DeviceOne = "-",
                                    DeviceTwo = "-",
                                    TradeOne = "-",
                                    TradeTwo = "-",
                                }
                            }
                        }
                    }
                });
            }

            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion



            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/InstantAudianceReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }




        public ActionResult InstantAudianceNoLayoutIndex(int userGroupId = 0, string userId = "")
        {
            Session["userGroupId"] = userGroupId;
            Session["userId"] = userId;

            return View();
        }

        public ActionResult InstantAudianceNoLayoutReport()
        {

            int userGroupId = int.Parse(Session["userGroupId"].ToString());
            string userId = Session["userId"].ToString();

            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.UserGroupId == userGroupId).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.Logs != null && x.Logs.Count() % 2 != 0).ToList();


            if (list != null)
                list = list.OrderBy(x => x.NationalCode).ToList();

            #endregion

            #region Fill Base Model

            var cureDate = DateTime.Now;

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش لحظه ای حاضرین";
            model.PrintDate = $"{DateTimeHelper.TopersianDate(DateTime.Now)}  {cureDate.Hour}:{cureDate.Minute}";
            model.StartDate = DateTimeHelper.TopersianDate(cureDate);
            model.EndDate = DateTimeHelper.TopersianDate(cureDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            int totalOne = list.Count();
            int totalTwo = 0;
            int totalThree = 0;
            int totalFoure = 0;


            foreach (var item in list)
            {
                var reportDay = item;

                var logs = reportDay.Logs;

                if (item.Logs.Count() % 2 != 0)
                {
                    BasicInfo userModel = new BasicInfo();

                    userModel.TotalTime = $"حاضر";


                    userModel.Index = userIndex.ToString();

                    userModel.GroupName = item.UserGroup.Title;

                    userModel.NameAndFamily = $"{item.FirstName} {item.Lastname}";

                    userModel.PersonalCode = HashHelper.Decrypt(item.NationalCode);

                    userModel.Branch = item.OrganizationBranch != null ? item.OrganizationBranch.Title : "-";

                    userModel.TotalTime = "-";



                    if (item.Logs != null && item.Logs.Count() > 0)
                    {
                        item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                    }

                    Trade trde = new Trade();

                    if (item.Logs != null && item.Logs.Count() > 0)
                    {
                        int logIndex = 0;
                        int logCount = item.Logs.Count();

                        for (int i = 0; i <= logCount; i++)
                        {
                            TradeLog tradeLogModel = new TradeLog();

                            var firstLog = new Log();
                            var secondLog = new Log();


                            tradeLogModel.TradeTwo = "-";
                            tradeLogModel.DeviceTwo = "-";
                            tradeLogModel.DeviceOne = "-";
                            tradeLogModel.DeviceTwo = "-";


                            if (logIndex < logCount && item.Logs[logIndex] != null)
                            {
                                firstLog = item.Logs[logIndex];

                                tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                                tradeLogModel.DeviceOne = firstLog.Device.Title;

                                logIndex = 1 + logIndex;
                            }

                            if (logIndex < logCount && item.Logs[logIndex] != null)
                            {
                                secondLog = item.Logs[logIndex];

                                tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                                tradeLogModel.DeviceTwo = secondLog.Device.Title;

                                logIndex = 1 + logIndex;
                            }

                            if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                            {
                                tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                                string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                                string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                                int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                                userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                            }
                            else
                            {
                                tradeLogModel.Duration = "00:00";
                            }

                            trde.TradeLogs.Add(tradeLogModel);


                        }

                        userModel.Trades.Add(trde);
                    }

                    if (trde.TradeLogs.Count() <= 0)
                    {
                        trde.TradeLogs.Add(new TradeLog()
                        {
                            DeviceOne = "-",
                            DeviceTwo = "-",
                            TradeOne = "-",
                            TradeTwo = "-",
                        });
                    }

                    userModel.Trades.Add(trde);

                    model.BasicInfos.Add(userModel);

                    userIndex = 1 + userIndex;
                }
            }

            model.TotalOne = model.BasicInfos.Count().ToString();
            model.TotalTwo = totalTwo.ToString();
            model.TotalThree = totalThree.ToString();
            model.TotalFour = totalFoure.ToString();

            #endregion

            #region داده خالی باشد

            if (model.BasicInfos.Count() <= 0)
            {
                model.TotalThree = "0";
                model.BasicInfos.Add(new BasicInfo()
                {
                    Index = "-",
                    PersonalCode = "-",
                    NameAndFamily = "-",
                    GroupName = "-",
                    Branch = "-",
                    TotalTime = "-",
                    Trades = new List<Trade>() {
                        new Trade(){
                            Index = "-",
                            PersianDate = "-",
                            PersianDayName = "-",
                            State = "-",
                            TradeLogs = new List<TradeLog>(){
                                new TradeLog(){
                                    DeviceOne = "-",
                                    DeviceTwo = "-",
                                    TradeOne = "-",
                                    TradeTwo = "-",
                                }
                            }
                        }
                    }
                });
            }

            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion



            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/InstantAudianceNoLayoutReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region TradeReprot - گزارش لحظه ای غایبین
        public ActionResult InstantAbsenceReportIndex(int userGroupId = 0, string userId = "")
        {
            Session["userGroupId"] = userGroupId;
            Session["userId"] = userId;

            return View();
        }

        public ActionResult InstantAbsenceReport()
        {

            int userGroupId = int.Parse(Session["userGroupId"].ToString());
            string userId = Session["userId"].ToString();

            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.UserGroupId == userGroupId).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.Logs != null && (x.Logs.Count() <= 0 || x.Logs.Count() % 2 == 0)).ToList();


            if (list != null)
                list = list.OrderBy(x => x.NationalCode).ToList();

            #endregion

            #region Fill Base Model

            var cureDate = DateTime.Now;

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش لحظه ای غایبین";
            model.PrintDate = $"{DateTimeHelper.TopersianDate(DateTime.Now)}  {cureDate.Hour}:{cureDate.Minute}";
            model.StartDate = DateTimeHelper.TopersianDate(cureDate);
            model.EndDate = DateTimeHelper.TopersianDate(cureDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            int totalOne = list.Count();
            int totalTwo = 0;
            int totalThree = 0;
            int totalFoure = 0;


            foreach (var item in list)
            {
                var reportDay = item;

                var logs = reportDay.Logs;


                BasicInfo userModel = new BasicInfo();



                if (item.Logs.Count() <= 0)
                {
                    userModel.TotalTime = $"غایب";
                }

                if (item.Logs.Count() > 0 && item.Logs.Count() % 2 == 0)
                {
                    userModel.TotalTime = $"خارج مجموعه";
                }

                userModel.Index = userIndex.ToString();

                userModel.GroupName = item.UserGroup.Title;

                userModel.NameAndFamily = $"{item.FirstName} {item.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(item.NationalCode);

                userModel.Branch = item.OrganizationBranch != null ? item.OrganizationBranch.Title : "-";

                userModel.TotalTime = "-";



                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                }

                Trade trde = new Trade();

                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    int logIndex = 0;
                    int logCount = item.Logs.Count();

                    for (int i = 0; i <= logCount; i++)
                    {
                        TradeLog tradeLogModel = new TradeLog();

                        var firstLog = new Log();
                        var secondLog = new Log();


                        tradeLogModel.TradeTwo = "-";
                        tradeLogModel.DeviceTwo = "-";
                        tradeLogModel.DeviceOne = "-";
                        tradeLogModel.DeviceTwo = "-";


                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            firstLog = item.Logs[logIndex];

                            tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                            tradeLogModel.DeviceOne = firstLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            secondLog = item.Logs[logIndex];

                            tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                            tradeLogModel.DeviceTwo = secondLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                        {
                            tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                            string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                            string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                            int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                            userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                        }
                        else
                        {
                            tradeLogModel.Duration = "00:00";
                        }

                        trde.TradeLogs.Add(tradeLogModel);


                    }

                    userModel.Trades.Add(trde);

                    totalTwo = 1 + totalTwo;
                }

                if (trde.TradeLogs.Count() <= 0)
                {
                    trde.TradeLogs.Add(new TradeLog()
                    {
                        DeviceOne = "-",
                        DeviceTwo = "-",
                        TradeOne = "-",
                        TradeTwo = "-",
                    });
                }

                userModel.Trades.Add(trde);

                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }

            model.TotalOne = model.BasicInfos.Count().ToString();
            model.TotalTwo = totalTwo.ToString();
            model.TotalThree = totalThree.ToString();
            model.TotalFour = totalFoure.ToString();

            #endregion

            #region داده خالی باشد

            if (model.BasicInfos.Count() <= 0)
            {
                model.TotalThree = "0";
                model.BasicInfos.Add(new BasicInfo()
                {
                    Index = "-",
                    PersonalCode = "-",
                    NameAndFamily = "-",
                    GroupName = "-",
                    Branch = "-",
                    TotalTime = "-",
                    Trades = new List<Trade>() {
                        new Trade(){
                            Index = "-",
                            PersianDate = "-",
                            PersianDayName = "-",
                            State = "-",
                            TradeLogs = new List<TradeLog>(){
                                new TradeLog(){
                                    DeviceOne = "-",
                                    DeviceTwo = "-",
                                    TradeOne = "-",
                                    TradeTwo = "-",
                                }
                            }
                        }
                    }
                });
            }

            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion



            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/InstantAbsenceReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }




        public ActionResult InstantAbsenceNoLayoutIndex(int userGroupId = 0, string userId = "")
        {
            Session["userGroupId"] = userGroupId;
            Session["userId"] = userId;

            return View();
        }

        public ActionResult InstantAbsenceNoLayoutReport()
        {

            int userGroupId = int.Parse(Session["userGroupId"].ToString());
            string userId = Session["userId"].ToString();

            ReportLayout model = new ReportLayout();

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.UserGroupId == userGroupId).ToList();

            if (list != null && list.Count() > 0 && userGroupId > 0)
                list = list.Where(x => x.Logs != null && (x.Logs.Count() <= 0 || x.Logs.Count() % 2 == 0)).ToList();


            if (list != null)
                list = list.OrderBy(x => x.NationalCode).ToList();

            #endregion

            #region Fill Base Model

            var cureDate = DateTime.Now;

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش لحظه ای غایبین";
            model.PrintDate = $"{DateTimeHelper.TopersianDate(DateTime.Now)}  {cureDate.Hour}:{cureDate.Minute}";
            model.StartDate = DateTimeHelper.TopersianDate(cureDate);
            model.EndDate = DateTimeHelper.TopersianDate(cureDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            int totalOne = list.Count();
            int totalTwo = 0;
            int totalThree = 0;
            int totalFoure = 0;


            foreach (var item in list)
            {
                var reportDay = item;

                var logs = reportDay.Logs;


                BasicInfo userModel = new BasicInfo();



                if (item.Logs.Count() <= 0)
                {
                    userModel.TotalTime = $"غایب";
                }

                if (item.Logs.Count() > 0 && item.Logs.Count() % 2 == 0)
                {
                    userModel.TotalTime = $"خارج مجموعه";
                }

                userModel.Index = userIndex.ToString();

                userModel.GroupName = item.UserGroup.Title;

                userModel.NameAndFamily = $"{item.FirstName} {item.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(item.NationalCode);

                userModel.Branch = item.OrganizationBranch != null ? item.OrganizationBranch.Title : "-";

                userModel.TotalTime = "-";



                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    item.Logs = item.Logs.Where(x => x.Remove == false).ToList();
                }

                Trade trde = new Trade();

                if (item.Logs != null && item.Logs.Count() > 0)
                {
                    int logIndex = 0;
                    int logCount = item.Logs.Count();

                    for (int i = 0; i <= logCount; i++)
                    {
                        TradeLog tradeLogModel = new TradeLog();

                        var firstLog = new Log();
                        var secondLog = new Log();


                        tradeLogModel.TradeTwo = "-";
                        tradeLogModel.DeviceTwo = "-";
                        tradeLogModel.DeviceOne = "-";
                        tradeLogModel.DeviceTwo = "-";


                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            firstLog = item.Logs[logIndex];

                            tradeLogModel.TradeOne = DateTimeHelper.ToHourMinute(firstLog.LogTime);
                            tradeLogModel.DeviceOne = firstLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (logIndex < logCount && item.Logs[logIndex] != null)
                        {
                            secondLog = item.Logs[logIndex];

                            tradeLogModel.TradeTwo = DateTimeHelper.ToHourMinute(secondLog.LogTime);
                            tradeLogModel.DeviceTwo = secondLog.Device.Title;

                            logIndex = 1 + logIndex;
                        }

                        if (!string.IsNullOrEmpty(tradeLogModel.TradeOne) && !string.IsNullOrEmpty(tradeLogModel.TradeTwo))
                        {
                            tradeLogModel.Duration = DateTimeHelper.CalculateDuration(tradeLogModel.TradeOne, tradeLogModel.TradeTwo);

                            string totalMinutes = DateTimeHelper.TimeToMinute(userModel.TotalTime);

                            string duationMinutes = DateTimeHelper.TimeToMinute(tradeLogModel.Duration);

                            int total = int.Parse(totalMinutes) + int.Parse(duationMinutes);

                            userModel.TotalTime = DateTimeHelper.TotalTimes(total);
                        }
                        else
                        {
                            tradeLogModel.Duration = "00:00";
                        }

                        trde.TradeLogs.Add(tradeLogModel);


                    }

                    userModel.Trades.Add(trde);

                    totalTwo = 1 + totalTwo;
                }

                if (trde.TradeLogs.Count() <= 0)
                {
                    trde.TradeLogs.Add(new TradeLog()
                    {
                        DeviceOne = "-",
                        DeviceTwo = "-",
                        TradeOne = "-",
                        TradeTwo = "-",
                    });
                }

                userModel.Trades.Add(trde);

                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }

            model.TotalOne = model.BasicInfos.Count().ToString();
            model.TotalTwo = totalTwo.ToString();
            model.TotalThree = totalThree.ToString();
            model.TotalFour = totalFoure.ToString();

            #endregion

            #region داده خالی باشد

            if (model.BasicInfos.Count() <= 0)
            {
                model.TotalThree = "0";
                model.BasicInfos.Add(new BasicInfo()
                {
                    Index = "-",
                    PersonalCode = "-",
                    NameAndFamily = "-",
                    GroupName = "-",
                    Branch = "-",
                    TotalTime = "-",
                    Trades = new List<Trade>() {
                        new Trade(){
                            Index = "-",
                            PersianDate = "-",
                            PersianDayName = "-",
                            State = "-",
                            TradeLogs = new List<TradeLog>(){
                                new TradeLog(){
                                    DeviceOne = "-",
                                    DeviceTwo = "-",
                                    TradeOne = "-",
                                    TradeTwo = "-",
                                }
                            }
                        }
                    }
                });
            }

            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion



            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/InstantAbsenceNoLayoutReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region AbsenceReport - گزارش غیبت
        public ActionResult AbsenceReportIndex(int groupId = 0, string userId = "", string stDate = "", string edDate = "", int analyzedReportState = 0)
        {
            Session["groupId"] = groupId;
            Session["userId"] = userId;
            Session["stDate"] = stDate;
            Session["edDate"] = edDate;
            Session["analyzedReportState"] = analyzedReportState;

            return View();
        }

        public ActionResult AbsenceReport()
        {

            int groupId = int.Parse(Session["groupId"].ToString());
            string userId = Session["userId"].ToString();
            DateTime stDate = DateTimeHelper.ToGeoDate(Session["stDate"].ToString()).GetValueOrDefault();
            DateTime edDate = DateTimeHelper.ToGeoDate(Session["edDate"].ToString()).GetValueOrDefault();
            int analyzedReportState = int.Parse(Session["analyzedReportState"].ToString());

            ReportLayout model = new ReportLayout();
            var currentDate = DateTime.Now.Date;
            var currentDatePersian = DateTimeHelper.TopersianDate(DateTime.Now.Date);

            string state = "";

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();
            var _analyzedReportService = DependencyResolver.Current.GetService<Service.IAnalyzedReportService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();
            var userList = _userService.List();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && groupId > 0)
                list = list.Where(x => x.UserGroupId == groupId).ToList();

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.Logs == null || x.Logs.Where(y => y.Remove == false).Count() <= 0).ToList();

            #endregion

            #region Fill Base Model

            switch (analyzedReportState)
            {
                case 1:
                    state = "غیبت و غیبت سیستمی";
                    break;
                case 2:
                    state = "غیبت سیستمی";
                    break;
                case 3:
                    state = "غیبت";
                    break;
                default:
                    state = "-";
                    break;
            }

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش غایبین";
            model.PrintDate = DateTimeHelper.TopersianDate(DateTime.Now);
            model.StartDate = DateTimeHelper.TopersianDate(stDate);
            model.EndDate = DateTimeHelper.TopersianDate(edDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            if (userList != null)
            {
                foreach (var user in userList)
                {

                    BasicInfo userModel = new BasicInfo();

                    userModel.Index = userIndex.ToString();

                    userModel.GroupName = user.UserGroup.Title;

                    userModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";

                    userModel.PersonalCode = HashHelper.Decrypt(user.NationalCode);

                    userModel.TotalTime = "0";



                    #region شناسایی گزارش های آنالیز شده مربوط به کاربر جاری

                    var analyzedReports = user.AnalyzedReports.Where(x => x.Date.Date >= stDate.Date && x.Date.Date <= edDate.Date).ToList();

                    if (analyzedReports != null && list.Count() > 0)
                        analyzedReports = analyzedReports.Where(x => x.UserId == user.Id).ToList();

                    if (analyzedReports != null && list.Count() > 0)
                        analyzedReports = analyzedReports.Where(x =>
                        x.State == AnalyzedReportState.Absence_NoTransaction ||
                        x.State == AnalyzedReportState.Holiday_Absence_NoTransaction ||
                        x.State == AnalyzedReportState.Absence_Systemic ||
                        x.State == AnalyzedReportState.Holiday_Absence_Systemic ||
                        x.State == AnalyzedReportState.FractionTrade ||
                        x.State == AnalyzedReportState.HolidayFraction)
                            .ToList();

                    if (analyzedReports != null && list.Count() > 0)
                        analyzedReports = analyzedReports.OrderBy(x => x.Date.Date).ToList();



                    if (analyzedReports != null && analyzedReports.Count() > 0)
                    {

                        switch (analyzedReportState)
                        {
                            case 1:
                                analyzedReports = analyzedReports.Where(x => x.State == AnalyzedReportState.Absence_NoTransaction || x.State == AnalyzedReportState.Holiday_Absence_NoTransaction).ToList();
                                state = "غیبت و غیبت سیستمی";
                                break;
                            case 2:
                                analyzedReports = analyzedReports.Where(x => x.State == AnalyzedReportState.Absence_Systemic || x.State == AnalyzedReportState.Holiday_Absence_Systemic).ToList();
                                state = "غیبت سیستمی";
                                break;
                            case 3:
                                analyzedReports = analyzedReports.Where(x => x.State == AnalyzedReportState.FractionTrade || x.State == AnalyzedReportState.HolidayFraction).ToList();
                                state = "غیبت";
                                break;
                            default:
                                break;
                        }

                        analyzedReports = analyzedReports.OrderBy(x => x.Date.Date).ToList();
                    }


                    if (analyzedReports != null && analyzedReports.Count() > 0)
                        analyzedReports = analyzedReports.OrderBy(x => x.Date.Date).ToList();

                    #endregion


                    if (analyzedReports != null && analyzedReports.Count() > 0)
                    {
                        int tradeIndex = 1;

                        foreach (var analyzedReport in analyzedReports)
                        {
                            try
                            {
                                Trade tradeModel = new Trade();

                                tradeModel.Index = tradeIndex.ToString();

                                tradeModel.PersianDate = DateTimeHelper.TopersianDate(analyzedReport.Date);

                                tradeModel.PersianDayName = DateTimeHelper.GetPersianDayName(analyzedReport.Date);


                                switch (analyzedReport.State)
                                {
                                    case AnalyzedReportState.WaitingForAnalyzing:
                                        tradeModel.State = "درحال آنالیز";
                                        break;
                                    case AnalyzedReportState.Presence:
                                        tradeModel.State = "حاضر";
                                        break;
                                    case AnalyzedReportState.Absence_NoTransaction:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.Absence_Systemic:
                                        tradeModel.State = "غیبت سیستمی";
                                        break;
                                    case AnalyzedReportState.FractionTrade:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.RestDay:
                                        tradeModel.State = "روز استراحت";
                                        break;
                                    case AnalyzedReportState.Holiday:
                                        tradeModel.State = "تعطیلی";
                                        break;
                                    case AnalyzedReportState.Holiday_Presence:
                                        break;
                                    case AnalyzedReportState.HolidayFraction:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.Holiday_Absence_NoTransaction:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.Holiday_Absence_Systemic:
                                        tradeModel.State = "غیبت سیستمی";
                                        break;
                                    default:
                                        tradeModel.State = "-";
                                        break;
                                }


                                if (analyzedReport.AnalyzedReportLogs != null && analyzedReport.AnalyzedReportLogs.Count() > 0)
                                {
                                    foreach (var log in analyzedReport.AnalyzedReportLogs)
                                    {

                                        TradeLog tradeLog = new TradeLog();

                                        tradeLog.TradeOne = DateTimeHelper.ToHourMinute(log.FirstLog.LogTime);

                                        tradeLog.DeviceOne = log.FirstLog.Device.Title;

                                        if (log.SecondLogId != null && log.SecondLogId > 0)
                                        {
                                            tradeLog.TradeTwo = DateTimeHelper.ToHourMinute(log.SecondLog.LogTime);

                                            tradeLog.DeviceTwo = log.SecondLog.Device.Title;

                                            string duration = DateTimeHelper.CalculateDuration(log.FirstLog.LogTime, log.SecondLog.LogTime);

                                            tradeLog.Duration = duration;
                                        }

                                        tradeModel.TradeLogs.Add(tradeLog);
                                    }
                                }

                                tradeIndex = 1 + tradeIndex;

                                userModel.Trades.Add(tradeModel);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }


                    model.BasicInfos.Add(userModel);
                }
            }

            #endregion

            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/AbsenceReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }




        public ActionResult AbsenceNoLayoutIndex(int groupId = 0, string userId = "", string stDate = "", string edDate = "", int analyzedReportState = 0)
        {
            Session["groupId"] = groupId;
            Session["userId"] = userId;
            Session["stDate"] = stDate;
            Session["edDate"] = edDate;
            Session["analyzedReportState"] = analyzedReportState;

            return View();
        }

        public ActionResult AbsenceNoLayoutReport()
        {

            int groupId = int.Parse(Session["groupId"].ToString());
            string userId = Session["userId"].ToString();
            DateTime stDate = DateTimeHelper.ToGeoDate(Session["stDate"].ToString()).GetValueOrDefault();
            DateTime edDate = DateTimeHelper.ToGeoDate(Session["edDate"].ToString()).GetValueOrDefault();
            int analyzedReportState = int.Parse(Session["analyzedReportState"].ToString());

            ReportLayout model = new ReportLayout();
            var currentDate = DateTime.Now.Date;
            var currentDatePersian = DateTimeHelper.TopersianDate(DateTime.Now.Date);

            string state = "";

            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();
            var _analyzedReportService = DependencyResolver.Current.GetService<Service.IAnalyzedReportService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();
            var userList = _userService.List();

            var list = _userService.GetList;

            if (list != null && list.Count() > 0)
                list = list.OrderBy(x => x.ReportDays != null).ToList();
            else
                list = new List<Core.wskhUser>();

            if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(userId))
                list = list.Where(x => x.Id.Contains(userId)).ToList();

            if (list != null && list.Count() > 0 && groupId > 0)
                list = list.Where(x => x.UserGroupId == groupId).ToList();

            if (list != null && list.Count() > 0)
                list = list.Where(x => x.Logs == null || x.Logs.Where(y => y.Remove == false).Count() <= 0).ToList();

            #endregion

            #region Fill Base Model

            switch (analyzedReportState)
            {
                case 1:
                    state = "غیبت و غیبت سیستمی";
                    break;
                case 2:
                    state = "غیبت سیستمی";
                    break;
                case 3:
                    state = "غیبت";
                    break;
                default:
                    state = "-";
                    break;
            }

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش غایبین";
            model.PrintDate = DateTimeHelper.TopersianDate(DateTime.Now);
            model.StartDate = DateTimeHelper.TopersianDate(stDate);
            model.EndDate = DateTimeHelper.TopersianDate(edDate);
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;

            if (userList != null)
            {
                foreach (var user in userList)
                {

                    BasicInfo userModel = new BasicInfo();

                    userModel.Index = userIndex.ToString();

                    userModel.GroupName = user.UserGroup.Title;

                    userModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";

                    userModel.PersonalCode = HashHelper.Decrypt(user.NationalCode);

                    userModel.TotalTime = "0";



                    #region شناسایی گزارش های آنالیز شده مربوط به کاربر جاری

                    var analyzedReports = user.AnalyzedReports.Where(x => x.Date.Date >= stDate.Date && x.Date.Date <= edDate.Date).ToList();

                    if (analyzedReports != null && list.Count() > 0)
                        analyzedReports = analyzedReports.Where(x => x.UserId == user.Id).ToList();

                    if (analyzedReports != null && list.Count() > 0)
                        analyzedReports = analyzedReports.Where(x =>
                        x.State == AnalyzedReportState.Absence_NoTransaction ||
                        x.State == AnalyzedReportState.Holiday_Absence_NoTransaction ||
                        x.State == AnalyzedReportState.Absence_Systemic ||
                        x.State == AnalyzedReportState.Holiday_Absence_Systemic ||
                        x.State == AnalyzedReportState.FractionTrade ||
                        x.State == AnalyzedReportState.HolidayFraction)
                            .ToList();

                    if (analyzedReports != null && list.Count() > 0)
                        analyzedReports = analyzedReports.OrderBy(x => x.Date.Date).ToList();



                    if (analyzedReports != null && analyzedReports.Count() > 0)
                    {

                        switch (analyzedReportState)
                        {
                            case 1:
                                analyzedReports = analyzedReports.Where(x => x.State == AnalyzedReportState.Absence_NoTransaction || x.State == AnalyzedReportState.Holiday_Absence_NoTransaction).ToList();
                                state = "غیبت و غیبت سیستمی";
                                break;
                            case 2:
                                analyzedReports = analyzedReports.Where(x => x.State == AnalyzedReportState.Absence_Systemic || x.State == AnalyzedReportState.Holiday_Absence_Systemic).ToList();
                                state = "غیبت سیستمی";
                                break;
                            case 3:
                                analyzedReports = analyzedReports.Where(x => x.State == AnalyzedReportState.FractionTrade || x.State == AnalyzedReportState.HolidayFraction).ToList();
                                state = "غیبت";
                                break;
                            default:
                                break;
                        }

                        analyzedReports = analyzedReports.OrderBy(x => x.Date.Date).ToList();
                    }


                    if (analyzedReports != null && analyzedReports.Count() > 0)
                        analyzedReports = analyzedReports.OrderBy(x => x.Date.Date).ToList();

                    #endregion


                    if (analyzedReports != null && analyzedReports.Count() > 0)
                    {
                        int tradeIndex = 1;

                        foreach (var analyzedReport in analyzedReports)
                        {
                            try
                            {
                                Trade tradeModel = new Trade();

                                tradeModel.Index = tradeIndex.ToString();

                                tradeModel.PersianDate = DateTimeHelper.TopersianDate(analyzedReport.Date);

                                tradeModel.PersianDayName = DateTimeHelper.GetPersianDayName(analyzedReport.Date);


                                switch (analyzedReport.State)
                                {
                                    case AnalyzedReportState.WaitingForAnalyzing:
                                        tradeModel.State = "درحال آنالیز";
                                        break;
                                    case AnalyzedReportState.Presence:
                                        tradeModel.State = "حاضر";
                                        break;
                                    case AnalyzedReportState.Absence_NoTransaction:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.Absence_Systemic:
                                        tradeModel.State = "غیبت سیستمی";
                                        break;
                                    case AnalyzedReportState.FractionTrade:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.RestDay:
                                        tradeModel.State = "روز استراحت";
                                        break;
                                    case AnalyzedReportState.Holiday:
                                        tradeModel.State = "تعطیلی";
                                        break;
                                    case AnalyzedReportState.Holiday_Presence:
                                        break;
                                    case AnalyzedReportState.HolidayFraction:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.Holiday_Absence_NoTransaction:
                                        tradeModel.State = "غیبت";
                                        break;
                                    case AnalyzedReportState.Holiday_Absence_Systemic:
                                        tradeModel.State = "غیبت سیستمی";
                                        break;
                                    default:
                                        tradeModel.State = "-";
                                        break;
                                }


                                if (analyzedReport.AnalyzedReportLogs != null && analyzedReport.AnalyzedReportLogs.Count() > 0)
                                {
                                    foreach (var log in analyzedReport.AnalyzedReportLogs)
                                    {

                                        TradeLog tradeLog = new TradeLog();

                                        tradeLog.TradeOne = DateTimeHelper.ToHourMinute(log.FirstLog.LogTime);

                                        tradeLog.DeviceOne = log.FirstLog.Device.Title;

                                        if (log.SecondLogId != null && log.SecondLogId > 0)
                                        {
                                            tradeLog.TradeTwo = DateTimeHelper.ToHourMinute(log.SecondLog.LogTime);

                                            tradeLog.DeviceTwo = log.SecondLog.Device.Title;

                                            string duration = DateTimeHelper.CalculateDuration(log.FirstLog.LogTime, log.SecondLog.LogTime);

                                            tradeLog.Duration = duration;
                                        }

                                        tradeModel.TradeLogs.Add(tradeLog);
                                    }
                                }

                                tradeIndex = 1 + tradeIndex;

                                userModel.Trades.Add(tradeModel);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }


                    model.BasicInfos.Add(userModel);
                }
            }

            #endregion

            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/AbsenceNoLayoutReport.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region SoftwareUserIndex - گزارش کاربران نرم افزار
        public ActionResult SoftwareUserIndex()
        {
            return View();
        }

        public ActionResult SoftwareUserReport()
        {
            ReportLayout model = new ReportLayout();

            #region Services

            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();
            var userList = _userService.GetList;

            #endregion

            #region Fill Base Model

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش کاربران";
            model.PrintDate = DateTimeHelper.TopersianDate(DateTime.Now);
            model.StartDate = "-";
            model.EndDate = "-";
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;


            foreach (var user in userList)
            {
                BasicInfo userModel = new BasicInfo();

                userModel.Index = userIndex.ToString();

                userModel.GroupName = user.UserGroup.Title;

                userModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(user.NationalCode);

                userModel.Branch = user.OrganizationBranch != null ? user.OrganizationBranch.Title : "-";

                userModel.EducationLevel = user.EducationLevel != null ? user.EducationLevel.Title : "-";

                userModel.State = user.Active ? "فعال" : "غیرفعال";

                userModel.TotalTime = "0";



                var currentPersianYear = DateTimeHelper.CurrentPersianYear();


                var contract = new UserGroupCalendare();

                if (user.UserGroup.UserGroupCalendares != null)
                {
                    contract = user.UserGroup.UserGroupCalendares.LastOrDefault(x => x.Remove == false && x.Calendar.Year == 1399);

                    if (contract != null)
                    {
                        userModel.Contracts.Add(new Contract()
                        {
                            HolidayName = contract.Calendar.SpecialDayGroupings.Title,
                            TradeRuleName = contract.Calendar.RequestRule.Title,
                            WorkProgramName = contract.Calendar.Title,
                        });
                    }

                    if (user.Enrolls != null)
                    {
                        foreach (var item in user.Enrolls)
                        {
                            userModel.DeviceAndEnrolls.Add(new DeviceAndEnroll()
                            {
                                DeviceName = item.FingerDevice.Title,
                                EnrollName = item.EnrollNo.ToString(),
                            });
                        }
                    }
                }
                else
                {
                    userModel.Contracts.Add(new Contract() {
                        HolidayName = "-",
                        TradeRuleName = "-",
                        WorkProgramName = "-",
                    });

                    userModel.DeviceAndEnrolls.Add(new DeviceAndEnroll()
                    {
                        DeviceName = "-",
                        EnrollName = "-",
                    });
                }


             

                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }



            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion

            var report = new StiReport();
            report.Compile();
            report.Load(Server.MapPath("/Reports/SoftwareUserReport.mrt"));
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }



        public ActionResult SoftwareUserNoLayoutIndex()
        {
            return View();
        }

        public ActionResult SoftwareUserNoLayoutReport()
        {
            ReportLayout model = new ReportLayout();

            #region Services

            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();

            #endregion

            #region Entity List

            var organizationEntity = _organizationInformationService.GetList.FirstOrDefault();
            var userList = _userService.GetList;

            #endregion

            #region Fill Base Model

            model.SoftwareCompany = "سامانه هوشمند حضور و غیاب فینگرتک";
            model.CompanyName = organizationEntity.Title;
            model.CompanyCategory = organizationEntity.Category;
            model.ReportTitle = "گزارش کاربران";
            model.PrintDate = DateTimeHelper.TopersianDate(DateTime.Now);
            model.StartDate = "-";
            model.EndDate = "-";
            model.PrintUser = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";

            #endregion

            #region Fill Model

            int userIndex = 1;


            foreach (var user in userList)
            {
                BasicInfo userModel = new BasicInfo();

                userModel.Index = userIndex.ToString();

                userModel.GroupName = user.UserGroup.Title;

                userModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";

                userModel.PersonalCode = HashHelper.Decrypt(user.NationalCode);

                userModel.Branch = user.OrganizationBranch != null ? user.OrganizationBranch.Title : "-";

                userModel.EducationLevel = user.EducationLevel != null ? user.EducationLevel.Title : "-";

                userModel.State = user.Active ? "فعال" : "غیرفعال";

                userModel.TotalTime = "0";



                var currentPersianYear = DateTimeHelper.CurrentPersianYear();


                var contract = new UserGroupCalendare();

                if (user.UserGroup.UserGroupCalendares != null)
                {
                    contract = user.UserGroup.UserGroupCalendares.LastOrDefault(x => x.Remove == false && x.Calendar.Year == 1399);

                    if (contract != null)
                    {
                        userModel.Contracts.Add(new Contract()
                        {
                            HolidayName = contract.Calendar.SpecialDayGroupings.Title,
                            TradeRuleName = contract.Calendar.RequestRule.Title,
                            WorkProgramName = contract.Calendar.Title,
                        });
                    }

                    if (user.Enrolls != null)
                    {
                        foreach (var item in user.Enrolls)
                        {
                            userModel.DeviceAndEnrolls.Add(new DeviceAndEnroll()
                            {
                                DeviceName = item.FingerDevice.Title,
                                EnrollName = item.EnrollNo.ToString(),
                            });
                        }
                    }
                }
                else
                {
                    userModel.Contracts.Add(new Contract()
                    {
                        HolidayName = "-",
                        TradeRuleName = "-",
                        WorkProgramName = "-",
                    });

                    userModel.DeviceAndEnrolls.Add(new DeviceAndEnroll()
                    {
                        DeviceName = "-",
                        EnrollName = "-",
                    });
                }




                model.BasicInfos.Add(userModel);

                userIndex = 1 + userIndex;
            }



            #endregion

            #region تصویر سازمان

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string imageUrl = "";

            if (!string.IsNullOrEmpty(organizationEntity.LogoPath))
            {
                imageUrl = organizationEntity.LogoPath.Replace('/', '\\');
            }


            Bitmap bm = new Bitmap($"{directory}{imageUrl}");
            var image = ImageToByteArray(bm);
            model.CompanyLogo = image;

            #endregion

            var report = new StiReport();
            report.Compile();
            report.Load(Server.MapPath("/Reports/SoftwareUserNoLayoutReport.mrt"));
            report.RegBusinessObject("ReportLayout", model);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region AnalyzedReport - گزارش ریزکارکرد
        public ActionResult AnalyzedReportIndex(int groupId = 0, string userId = "", string stDate = "", string edDate = "")
        {
            Session["groupId"] = groupId;
            Session["userId"] = userId;
            Session["stDate"] = stDate;
            Session["edDate"] = edDate;

            return View();
        }

        public ActionResult AnalyzedReport()
        {

            int groupId = int.Parse(Session["groupId"].ToString());
            string userId = Session["userId"].ToString();
            DateTime stDate = DateTimeHelper.ToGeoDate(Session["stDate"].ToString()).GetValueOrDefault();
            DateTime edDate = DateTimeHelper.ToGeoDate(Session["edDate"].ToString()).GetValueOrDefault();

            List<TradeStiReportModel> modelList = new List<TradeStiReportModel>();
            var currentDate = DateTime.Now.Date;
            var currentDatePersian = DateTimeHelper.TopersianDate(DateTime.Now.Date);


            #region Services

            var _fingerDeviceService = DependencyResolver.Current.GetService<Service.IFingerDeviceService>();
            var _userGroupService = DependencyResolver.Current.GetService<Service.IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<Service.IUserService>();
            var _organizationInformationService = DependencyResolver.Current.GetService<Service.IOrganizationInformationService>();
            var _analyzedReportService = DependencyResolver.Current.GetService<Service.IAnalyzedReportService>();
            var _reportDayService = DependencyResolver.Current.GetService<Service.IReportDayService>();

            #endregion

            #region Entity List

            var entityList = _analyzedReportService.List(userId, stDate, edDate);

            var organizationEntity = _organizationInformationService.List().FirstOrDefault();

            var user = _userService.List().FirstOrDefault(x => x.Id == userId);

            #endregion

            #region Fill Base Model

            string softwareCompanyStr = "سامانه هوشمند حضور و غیاب فینگرتک";
            string companyNameStr = organizationEntity.Title;
            string reportTitleStr = $"گزارش ریز کارکرد";
            string printDateStr = DateTimeHelper.TopersianDate(DateTime.Now);
            string startDateStr = DateTimeHelper.TopersianDate(currentDate);
            string endDateStr = DateTimeHelper.TopersianDate(currentDate);
            string printUserStr = $"{UserHelper.CurrentUser().FirstName} {UserHelper.CurrentUser().Lastname}";


            string totalWorkTime = AnalyzedReportHelper.TotalWorkTime(entityList);
            string legalWorkTime = AnalyzedReportHelper.TotalRealWorkTime(entityList);
            string lowWorkTime = AnalyzedReportHelper.LowWorkTme(entityList);
            string delayEnteranceTime = AnalyzedReportHelper.TotalDelayEnter(entityList);
            string earlyEnteranceTime = AnalyzedReportHelper.TotalEarlyEnter(entityList);
            string delayExitTime = AnalyzedReportHelper.TotalDelayExitTIme(entityList);
            string earlyExitTime = AnalyzedReportHelper.TotalEarlyEnteranceTIme(entityList);

            #endregion

            #region Fill Model

            int index = 1;

            if (entityList != null && entityList.Count() > 0)
            {
                foreach (var reportEntity in entityList)
                {
                    try
                    {
                        var finalModel = new TradeStiReportModel();
                        finalModel.SoftwareCompany = softwareCompanyStr;
                        finalModel.CompanyName = companyNameStr;
                        finalModel.ReportTitle = reportTitleStr;
                        finalModel.StartDate = startDateStr;
                        finalModel.EndDate = endDateStr;
                        finalModel.PrintUser = printUserStr;
                        finalModel.PrintDate = printDateStr;

                        finalModel.NameAndFamily = $"{user.FirstName} {user.Lastname}";
                        finalModel.GroupName = user.UserGroup.Title;
                        finalModel.Index = index.ToString();
                        finalModel.Date = currentDatePersian;


                        finalModel.TotalLegalTime = totalWorkTime;
                        finalModel.TotalLiveTime = legalWorkTime;
                        finalModel.TotalLowWorkTime = lowWorkTime;
                        finalModel.TotalDelayEnterance = delayEnteranceTime;
                        finalModel.TotalEarlyEnterance = earlyEnteranceTime;
                        finalModel.TotalDelayExit = delayExitTime;
                        finalModel.TotalEarlyExit = earlyExitTime;


                        finalModel.PersianDayName = reportEntity.PersianDayName;

                        finalModel.LegalTime = reportEntity.TotalWorkTime;
                        finalModel.LiveTime = reportEntity.RealTotalWorkTime;

                        int delayEnter = int.Parse(DateTimeHelper.TimeToMinute(reportEntity.DelayEnterTime));
                        int earlyExit = int.Parse(DateTimeHelper.TimeToMinute(reportEntity.EarlyExit));
                        int totalTime = delayEnter + earlyExit;

                        finalModel.LowWorkTime = totalTime.ToString();

                        finalModel.DelayEnterance = reportEntity.DelayEnterTime;
                        finalModel.EarlyEnterance = reportEntity.EarlyEnteranceTime;
                        finalModel.DelayExit = reportEntity.DelayExitTime;
                        finalModel.EarlyExit = reportEntity.EarlyExit;

                        switch (reportEntity.State)
                        {
                            case AnalyzedReportState.WaitingForAnalyzing:
                                finalModel.State = "درحل آنالیز";
                                break;
                            case AnalyzedReportState.Presence:
                                finalModel.State = "حضور";
                                break;
                            case AnalyzedReportState.Absence_NoTransaction:
                                finalModel.State = "غیبت";
                                break;
                            case AnalyzedReportState.Absence_Systemic:
                                finalModel.State = "غیبت سیستمی";
                                break;
                            case AnalyzedReportState.FractionTrade:
                                finalModel.State = "تردد ناقص";
                                break;
                            case AnalyzedReportState.RestDay:
                                finalModel.State = "روز استراحت";
                                break;
                            case AnalyzedReportState.Holiday:
                                finalModel.State = "تعطیلی";
                                break;
                            case AnalyzedReportState.Holiday_Presence:
                                finalModel.State = "حضور-روز تعطیل";
                                break;
                            case AnalyzedReportState.HolidayFraction:
                                finalModel.State = "ترددناقص-روز تعطیل";
                                break;
                            case AnalyzedReportState.Holiday_Absence_NoTransaction:
                                finalModel.State = "غیبت-روز تعطیل";
                                break;
                            case AnalyzedReportState.Holiday_Absence_Systemic:
                                finalModel.State = "غیبت سیستمی-روز تعطیل";
                                break;
                            default:
                                finalModel.StartDate = "-";
                                break;
                        }

                        index = index + 1;

                        modelList.Add(finalModel);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            #endregion

            #region داده خام
            if (modelList == null || modelList.Count() <= 0)
            {
                modelList.Add(new TradeStiReportModel()
                {
                    SoftwareCompany = softwareCompanyStr,
                    CompanyName = companyNameStr,
                    ReportTitle = reportTitleStr,
                    Date = "-",
                    StartDate = startDateStr,
                    EndDate = endDateStr,
                    NameAndFamily = "-",
                    GroupName = "-",
                    Index = "-",
                    Trades = "-",
                });
            }
            #endregion

            var report = new StiReport();
            report.Load(Server.MapPath("/Reports/AnalyzedReport.mrt"));
            report.Compile();
            report.RegBusinessObject("DataSource", modelList);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        #endregion


        #region ViewerEvent

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(HttpContext);
        }

        #endregion


        #region ExtentionMethods
        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageIn.RawFormat);
            return ms.ToArray();
        }
        #endregion
    }
}