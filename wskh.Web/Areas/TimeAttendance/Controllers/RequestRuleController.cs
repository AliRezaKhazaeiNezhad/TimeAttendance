using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Service;
using TimeAttendance.WebEssentials.DateAndTime;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class RequestRuleController : Controller
    {
        #region Propertices
        private readonly IRequestRuleService _requestRuleService;
        private readonly ICalendarService _calendarService;
        #endregion
        #region Ctor
        public RequestRuleController(IRequestRuleService requestRuleService, ICalendarService calendarService)
        {
            _requestRuleService = requestRuleService;
            _calendarService = calendarService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "rulesandprograms";
            ViewBag.MenuName = "requestrule";

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
            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<RequestRuleGridModel>();
            modelItem.draw = request.draw;
            var data = _requestRuleService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _requestRuleService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new RequestRuleGridModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = x.Title;

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add or update
        [HttpGet]
        public ActionResult AddOrUpdate(int id = 0)
        {
            ViewBag.MenuName = "requestrule";
            RequestRuleModel model = new RequestRuleModel();
            if (id <= 0)
            {
                model.MaximumAbsence = "02:00";
                model.DayDuration = "07:20";
            }


            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(RequestRuleModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    RequestRule entity = new RequestRule();

                    entity.Title = model.Title;
                    entity.MaximumAbsence = model.MaximumAbsence ?? "0";
                    entity.DayDuration = model.DayDuration ?? "7:20";
                    entity.DelayAction = (DelayAction)model.DelayActionId;
                    entity.DelayActionPercent = model.DelayActionPercent ?? "0";
                    entity.DelyMonthMin = model.DelyMonthMin ?? "0";
                    entity.DelayDayMin = model.DelayDayMin ?? "0";
                    entity.FlowTime = model.FlowTimeId == 0 ? false : true;
                    entity.FlowTimeMin = model.FlowTimeMin ?? "0";


                    if (entity.FlowTime)
                    {
                        entity.DelayAction = (DelayAction)model.DelayActionId;
                        entity.DelayActionPercent = "1";
                        entity.DelyMonthMin = "0";
                        entity.DelayDayMin = "0";
                    }



                    int dayDurationMin = int.Parse(TimeHelper.TotalMinute(model.DayDuration));
                    int maxAbsebnceMin = int.Parse(TimeHelper.TotalMinute(model.MaximumAbsence));

                    entity.RequestRuleDetails = new System.Collections.Generic.List<RequestRuleDetail>();
                    

                    foreach (var item in model.RequestRuleDetailModels)
                    {
                        RequestRuleDetail newEntity = new RequestRuleDetail();
                        newEntity.Remove = false;
                        newEntity.RestDayCountMonthly = item.RestDayCountMonthly;
                        newEntity.LeaveTypeId = item.LeaveTypeId;


                        if (item.LeaveTypeId == 1)
                        {

                            if (item.CalculateYearly.Contains("true"))
                            {
                                newEntity.RestDayCountYearly = item.RestDayCountYearly;
                                newEntity.RestToNextYear = item.RestToNextYear;
                                newEntity.RestDayCountMonthly = "0";
                                newEntity.RestToNextMonth = "0";
                                newEntity.YearlyRestMin = (int.Parse(newEntity.RestDayCountYearly) * dayDurationMin).ToString();
                                newEntity.MonthlyRestMin = "0";
                            }

                            if (item.CalculateYearly.Contains("false"))
                            {
                                newEntity.RestDayCountYearly = "0";
                                newEntity.RestToNextYear = "0";
                                newEntity.RestDayCountMonthly = item.RestDayCountMonthly;
                                newEntity.RestToNextMonth = item.RestToNextMonth;
                                newEntity.YearlyRestMin = "0";
                                newEntity.MonthlyRestMin = (int.Parse(item.RestDayCountMonthly) * dayDurationMin).ToString();
                            }
                        }
                        else if (item.LeaveTypeId == 3)
                        {
                            newEntity.YearlyRestMin = "0";
                            newEntity.MonthlyRestMin = "0";
                        }
                        else if (item.LeaveTypeId == 2)
                        {
                            newEntity.RestDayCountYearly = item.RestDayCountYearly;
                            newEntity.RestDayCountMonthly = "0";
                            newEntity.YearlyRestMin = (int.Parse(newEntity.RestDayCountYearly) * dayDurationMin).ToString();
                            newEntity.MonthlyRestMin = "0";
                        }
                        else
                        {
                            newEntity.RestDayCountYearly = item.RestDayCountYearly;
                            newEntity.RestDayCountMonthly = "0";
                        }

                        entity.RequestRuleDetails.Add(newEntity);
                    }

                    _requestRuleService.Create(entity);
                    return RedirectToAction("Index", "RequestRule", new { area = "TimeAttendance" });
                }
                catch (Exception e)
                {
                }
            }
            return View(model);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            int result = 0;
            try
            {
                var list = _calendarService.GetList.Where(x => x.RequestRuleId == id).ToList();
                if (list != null && list.Count() > 0)
                {
                    result = 2;
                }
                else
                {
                    RequestRule entity = _requestRuleService.FindById(id);
                    entity.Remove = true;
                    _requestRuleService.Update(entity);
                    result = 1;
                }
            }
            catch (Exception e)
            {
                result = -1;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Show
        public ActionResult Show(int id)
        {
            var entity = _requestRuleService.FindById(id);
            return PartialView("_Show", entity);
        }
        #endregion
        #region Description
        public ActionResult Description()
        {
            return PartialView("_Description");
        }
        #endregion
    }
}