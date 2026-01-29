using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.DateAndTime;
using TimeAttendance.WebEssentials.OtherHelper;
using TimeAttendance.WebEssentials.ReportHelper;
using TimeAttendance.WebEssentials.RequestPart;
using TimeAttendance.WebEssentials.StringAndNumber;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    public class InstantReportAudienceController : Controller
    {
        #region Propertices
        private IUserService _userService { get; set; }
        #endregion


        #region Ctor

        public InstantReportAudienceController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion


        #region Search
        [HttpGet]
        public ActionResult Search(int groupId = 0, string userId = "")
        {
            #region پر کردن مدل
            TradeSearchModel model = PrepareSearchModel(groupId, userId);
            #endregion

            #region پر کردن سشن
            PrepareSession(model);
            #endregion

            return PartialView("_Search", model);
        }
        #endregion


        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = $"instantreportaudience";
            return View();
        }
        #endregion
 

        #region List
        [HttpGet]
        public ActionResult ListIndex()
        {
            return PartialView("_List");
        }
        [HttpGet]
        public JsonResult List(DataTableRequest request, [ModelBinder(typeof(DataTableModelBinder))]DataTableRequestFilter filter)
        {
            #region Grid configuration
             var sessionModel = RetriveSession();

            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<InstantReportModel>();
            modelItem.draw = request.draw;
            var data = _userService.InstantFilterData(request.start, request.length, filter.Search, 1, sessionModel.UserId, sessionModel.UserGroupId);
            modelItem.recordsTotal = _userService.InstantCount(filter.Search, 1, sessionModel.UserId, sessionModel.UserGroupId);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new InstantReportModel();
                model.Index = ++add;
                try
                {
                    model.Id = 0;
                    model.GroupName = x.UserGroup.Title;
                    model.NameAndFamily = $"{x.FirstName} {x.Lastname}";

                    if (x.ReportDays != null && x.ReportDays.Count() > 0)
                    {

                        var reportDay = x.ReportDays.FirstOrDefault();


                        model.Id = reportDay.Id;
                        var logs = reportDay
                        .Logs
                        .OrderBy(f => f.LogDate.GetValueOrDefault().Date)
                        .ThenBy(f => f.LogDate.GetValueOrDefault().TimeOfDay)
                        .ToList();


                        model.EnteranceTime = ReportHelper.Trades(logs, false, reportDay.Id, x.Id).Item1;
                        model.StateString = "<button type='button' class='btn btn-sm btn-success' disabled>حاضر</button>";
                        model.Button1 = "<button class='btn btn-info btn-sm' type='button' onclick='Detail(" + reportDay.Id + ", \"" + x.Id + "\")' > عملیات</button>";
                    }
                    else
                    {
                        model.EnteranceTime = "-";
                        model.ExitTime = "-";
                        model.StateString = "<button type='button' class='btn btn-sm btn-danger' disabled>غایب</button>";
                        model.Button1 = "<button class='btn btn-info btn-sm' type='button' onclick='Detail(" + 0 + ", \"" + x.Id + "\")' > عملیات</button>";
                    }

                }
                catch (Exception e)
                {
                }

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region PrepareMethods
        private static TradeSearchModel PrepareSearchModel(int groupId, string userId)
        {
            TradeSearchModel model = new TradeSearchModel();
            if (!string.IsNullOrEmpty(userId) && userId == "0")
            {
                userId = "";
            }
            model.UserGroupId = groupId;
            model.UserId = userId;
            return model;
        }

        private void PrepareSession(TradeSearchModel model)
        {
            Session["UserGroupId"] = model.UserGroupId;
            Session["UserId"] = model.UserId;
        }

        private TradeSearchModel RetriveSession()
        {
            TradeSearchModel model = new TradeSearchModel();
            model.UserGroupId = string.IsNullOrEmpty(Session["UserGroupId"].ToString()) ? 0 : int.Parse(Session["UserGroupId"].ToString());
            model.UserId = string.IsNullOrEmpty(Session["UserId"].ToString()) ? "" : Session["UserId"].ToString();

            return model;
        }
        #endregion


        #region GetDrop
        public ActionResult GetDrop(int usergroupId = 0)
        {
            List<DropDownModel> modelList = new List<DropDownModel>();
            modelList.Add(new DropDownModel() { Value = "0", Text = "تمامی پرسنل" });

            if (usergroupId > 0)
            {
                var userList = _userService.GetList;
                if (!ListHelper.IsListNull(userList))
                {
                    userList = userList.Where(x => x.UserGroupId == usergroupId).ToList();
                    foreach (var user in userList)
                    {
                        modelList.Add(new DropDownModel() { Value = user.Id, Text = $"{user.FirstName} {user.Lastname}" });
                    }
                }
            }
            return View("_GetDrop", modelList);
        }
        #endregion
    }
}