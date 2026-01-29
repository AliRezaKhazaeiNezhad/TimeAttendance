using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Service;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials;
using TimeAttendance.WebEssentials.CommandPart;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    public class CalendareController : Controller
    {
        #region Propertices
        private ICalendarService _calendarService;
        private IWorkProgramService _workProgramService;
        private List<string> _persianYears = WebConfigHelper.PersianYears();
        #endregion
        #region Ctor
        public CalendareController(ICalendarService CalendarService, IWorkProgramService workProgramService)
        {
            _calendarService = CalendarService;
            _workProgramService = workProgramService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "rulesandprograms";
            ViewBag.MenuName = "calendar";
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
            var modelItem = new DataTableResponse<CalendarModel>();
            modelItem.draw = request.draw;
            var data = _calendarService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _calendarService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new CalendarModel(_persianYears);
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = x.Title;
                
                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add or Update
        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            ViewBag.PanelName = "rulesandprograms";
            ViewBag.MenuName = "calendar";

            CalendarModel model = new CalendarModel(_persianYears) { Id = id };
            if (id > 0)
            {
                Calendar entity = _calendarService.FindById(id);
                model.Id = entity.Id;
                model.Title = entity.Title;
                model.HolidayAndSpecialDayId = entity.SpecialDayGroupingId.GetValueOrDefault();

                model.IsEdit = true;
                model.RequestRuleId = entity.RequestRuleId;
                model.PersianYearId = entity.Year;

                if (entity.CalendarDays != null && entity.CalendarDays.Count() > 0)
                {
                    foreach (var day in entity.CalendarDays)
                    {
                        CalendarDayModel dayModel = new CalendarDayModel();
                        dayModel.Id = day.Id;
                        dayModel.StartDate = DateTimeHelper.TopersianDate(day.StartDate);
                        dayModel.EndDate = DateTimeHelper.TopersianDate(day.EndDate);
                        dayModel.StartDateGeo = day.StartDate;
                        dayModel.EndDateGeo = day.EndDate;
                        dayModel.WorkProgramId = day.WorkProgramId;
                        model.CalendarFormatModel.CalendarDayModels.Add(dayModel);
                    }
                }


            }
            Session["calendarmodel"] = model;
            return View();
        }
        public ActionResult GetCalendare()
        {
            return PartialView("_GetCalendare");
        }
        [HttpPost]
        public ActionResult AddOrUpdate(int id = 0)
        {
            CalendarModel model = (CalendarModel)Session["calendarmodel"];
            Calendar entity = new Calendar();
            Calendar entityNew = new Calendar();
            entity.Title = model.Title;
            JsonModel jsonModel = new JsonModel();
            int parentId = 0;
            int orginalParentId = 0;

            try
            {
                if (id > 0)
                {
                    entity.Remove = true;

                    entity = _calendarService.FindById(model.Id.GetValueOrDefault());
                    if (entity.ParentId == null)
                    {
                        parentId = entity.Id;
                        orginalParentId = entity.Id;
                    }
                    else
                    {
                        parentId = entity.Id;
                        orginalParentId = entity.OriginalParentId.GetValueOrDefault();
                    }

                    entityNew.ParentId = parentId;
                 
                    entityNew.OriginalParentId = orginalParentId;
                    entityNew.Remove = false;

                    if (entity.UserGroupCalendares != null && entity.UserGroupCalendares.Count() > 0)
                    {
                        foreach (var item in entity.UserGroupCalendares)
                        {
                            item.Remove = true;
                        }
                    }

                    entity.Remove = true;

                    _calendarService.Update(entity);


                }



                entityNew.RequestRuleId = model.RequestRuleId;
                entityNew.Title = model.Title;
                entityNew.SpecialDayGroupingId = model.HolidayAndSpecialDayId;
                entityNew.Year = model.PersianYearId;
                entityNew.IsBigYear = WebConfigHelper.Bigyears().Where(x => x.Contains(model.PersianYearId.ToString())).Count() > 0 ? true : false;

                _calendarService.Create(entityNew);


                if (model.CalendarFormatModel.WorkPrograms != null && model.CalendarFormatModel.WorkPrograms.Count() > 0)
                    foreach (var day in model.CalendarFormatModel.CalendarDayModels)
                    {
                        entityNew.CalendarDays.Add(new CalendarDay()
                        {
                            StartDate = DateTimeHelper.ToGeoDate($"{day.StartDate.Split('/')[0]}/{day.StartDate.Split('/')[1]}/{day.StartDate.Split('/')[2]}").GetValueOrDefault(),
                            EndDate = DateTimeHelper.ToGeoDate(day.EndDate).GetValueOrDefault(),
                            WorkProgramId = day.WorkProgramId
                        });
                    }


                if (entity.UserGroupCalendares != null && entity.UserGroupCalendares.Count() > 0)
                {
                    foreach (var item in entity.UserGroupCalendares)
                    {
                        entityNew.UserGroupCalendares.Add(new UserGroupCalendare()
                        {
                            CalendarId = entityNew.Id,
                            UserGroupId = item.UserGroupId,
                            Remove = false
                        });
                    }
                }
                _calendarService.Update(entityNew);

                if (model.Id > 0)
                {
                    CommandHelper.Create(CommandCategory.CalendarAdded, false, "تقویم کاری بروزرسانی شد", 0, UserHelper.CurrentUser(), entityNew.Id);
                }
                else
                {
                    CommandHelper.Create(CommandCategory.CalendarUpdate, false, "تقویم کاری ایجاد شد", 0, UserHelper.CurrentUser(), entityNew.Id);
                }


                jsonModel.Success();

                //return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                jsonModel.Exception();
            }
            return Json(jsonModel);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            bool result = false;
            try
            {
                Calendar entity = _calendarService.FindById(id);
                entity.Remove = true;
                _calendarService.Update(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Description
        public ActionResult Description()
        {
            return PartialView("_Description");
        }
        #endregion
        #region AddCalendare
        public ActionResult AddCalendare(string title)
        {
            CalendarModel model = (CalendarModel)Session["calendarmodel"];
            JsonModel jsonModel = new JsonModel();
            try
            {
                model.Title = title;
            }
            catch (Exception e)
            {
                jsonModel.Exception();
            }
            Session["calendarmodel"] = model;
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AddDay
        public ActionResult AddDay(string startDate, string endDate, int wpId)
        {
            CalendarModel model = (CalendarModel)Session["calendarmodel"];
            JsonModel jsonModel = new JsonModel();
            try
            {
                DateTime stDate = DateTimeHelper.ToGeoDate(startDate).GetValueOrDefault();
                DateTime edDate = DateTimeHelper.ToGeoDate(endDate).GetValueOrDefault();
                if (model.CalendarFormatModel.CalendarDayModels != null)
                {
                    if (model.CalendarFormatModel.CalendarDayModels.Where(x => (x.StartDateGeo.Date <= stDate.Date && x.EndDateGeo.Date >= stDate.Date) || (x.StartDateGeo.Date <= edDate.Date && x.EndDateGeo.Date >= edDate.Date)).Count() > 0)
                        jsonModel.Other("بازه تکراری میباشد");
                    else
                    {
                        model.CalendarFormatModel.CalendarDayModels.Add(new CalendarDayModel()
                        {
                            EndDate = endDate,
                            StartDate = startDate,
                            StartDateGeo = stDate,
                            EndDateGeo = edDate,
                            WorkProgramId = wpId
                        });
                        jsonModel.Success();
                    }
                }
            }
            catch (Exception e)
            {
                jsonModel.Exception();
            }
            Session["calendarmodel"] = model;
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region RemoveDay
        public ActionResult RemoveDay(int dayIndex)
        {
            CalendarModel model = (CalendarModel)Session["calendarmodel"];
            JsonModel jsonModel = new JsonModel();
            try
            {
                try
                {
                    model.CalendarFormatModel.CalendarDayModels.Remove(model.CalendarFormatModel.CalendarDayModels[dayIndex]);
                    jsonModel.Success();
                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }
            catch (Exception e)
            {
                jsonModel.Exception();
            }
            Session["calendarmodel"] = model;
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AllowDate
        public ActionResult AllowDate(string stDate, string edDate)
        {
            ///0 درست
            ///1 تاریخ پایان کوچکتر
            ///2 تاریخ ها برابر
            ///3خطا 
            int result = 3;
            try
            {
                try
                {
                    var StartDate = DateTimeHelper.ToGeoDate(stDate);
                    var EndDate = DateTimeHelper.ToGeoDate(edDate);
                    if (StartDate == EndDate)
                        result = 2;
                    else if (EndDate < StartDate)
                        result = 2;
                }
                catch (Exception e)
                {
                    result = 3;
                }
            }
            catch (Exception e)
            {
                result = 3;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetProgram
        public ActionResult GetProgram(int id)
        {
            Session["getprogrammodel"] = new CalendarModel(_persianYears);
            var entity = _calendarService.FindById(id);
            CalendarModel model = new CalendarModel(_persianYears);

            model.Id = entity.Id;
            model.Title = entity.Title;

            if (entity.CalendarDays != null && entity.CalendarDays.Count() > 0)
            {
                foreach (var day in entity.CalendarDays)
                {
                    CalendarDayModel dayModel = new CalendarDayModel();
                    dayModel.Id = day.Id;
                    dayModel.StartDate = DateTimeHelper.TopersianDate(day.StartDate);
                    dayModel.EndDate = DateTimeHelper.TopersianDate(day.EndDate);
                    dayModel.StartDateGeo = day.StartDate;
                    dayModel.EndDateGeo = day.EndDate;
                    dayModel.WorkProgramId = day.WorkProgramId;
                    model.CalendarFormatModel.CalendarDayModels.Add(dayModel);
                }
            }
            Session["getprogrammodel"] = model;

            return PartialView("_GetProgram");
        }
        #endregion
        #region AddDataPartOne
        public ActionResult AddDataPartOne(string title, int persianYearId, int holidayId, int requestRuleId)
        {
            CalendarModel model = (CalendarModel)Session["calendarmodel"];
            JsonModel jsonModel = new JsonModel();
            try
            {
                model.Title = title;
                model.PersianYearId = persianYearId;
                model.HolidayAndSpecialDayId = holidayId;
                model.RequestRuleId = requestRuleId;
                jsonModel.Success();
            }
            catch (Exception e)
            {
                jsonModel.Exception();
            }
            Session["calendarmodel"] = model;
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AddDataPartTwo
        public ActionResult AddDataPartTwo(
            bool card,
            int delayActionId,
            string delayActionPercent,
            string delayDayMin,
            string delyMonthMin,
            int flowTimeId,
            string flowTimeMin,
            bool functionKey,
            string restDayCountYearly,
            string restDayCountMonthly,
            bool restToNextMonth,
            bool restToNextYear,
            bool workCode, 
            string nextYearRest,
            string nextMonthRest
            )
        {
            CalendarModel model = (CalendarModel)Session["calendarmodel"];
            JsonModel jsonModel = new JsonModel();
            try
            {
                //model.WorkRuleModel.Card = card;
                //model.WorkRuleModel.DelayActionId = delayActionId;
                //model.WorkRuleModel.DelayActionPercent = delayActionPercent;
                //model.WorkRuleModel.DelayDayMin = delayDayMin;
                //model.WorkRuleModel.DelyMonthMin = delyMonthMin;
                //model.WorkRuleModel.FlowTimeId = flowTimeId;
                //model.WorkRuleModel.FlowTimeMin = flowTimeMin;
                //model.WorkRuleModel.FunctionKey = functionKey;
                //model.WorkRuleModel.RestDayCountYearly = restDayCountYearly;
                //model.WorkRuleModel.RestDayCountMonthly = restDayCountMonthly;
                //model.WorkRuleModel.RestToNextMonth = restToNextMonth;
                //model.WorkRuleModel.RestToNextYear = restToNextYear;
                //model.WorkRuleModel.WorkCode = workCode;
                //model.WorkRuleModel.DaylyLeaveForNextYear = restToNextYear ? nextYearRest : "0";
                //model.WorkRuleModel.DaylyLeaveForNextMonth = restToNextMonth ? nextMonthRest : "0";
                jsonModel.Success();
            }
            catch (Exception e)
            {
                jsonModel.Exception();
            }
            Session["calendarmodel"] = model;
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region PartialView
        public ActionResult CalendarInformation()
        {
            return PartialView("_CalendarInformation");
        }

        public ActionResult MovementInformation()
        {
            return PartialView("_MovementInformation");
        }

        public ActionResult CalendarDetail()
        {
            return PartialView("_CalendarDetail");
        }
        #endregion
    }
}