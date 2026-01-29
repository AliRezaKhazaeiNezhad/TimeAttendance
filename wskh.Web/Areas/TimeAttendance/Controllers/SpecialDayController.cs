using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
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
    public class SpecialDayController : Controller
    {
        #region Propertices
        private ISpecialDayService _specialDayService;
        private IWorkProgramService _workProgramService;
        #endregion
        #region Ctor
        public SpecialDayController(ISpecialDayService specialDayService, IWorkProgramService workProgramService)
        {
            _specialDayService = specialDayService;
            _workProgramService = workProgramService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "rulesandprograms";
            ViewBag.MenuName = "specialday";
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
            var modelItem = new DataTableResponse<SpecialDayModel>();
            modelItem.draw = request.draw;
            var data = _specialDayService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _specialDayService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new SpecialDayModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = x.Title;
                model.DelayEnterance = $"{x.DelayEnterance} <br/> {x.DelayExit}";
                model.EndDate = x.EndDate != null ? DateTimeHelper.TopersianDate(x.EndDate.GetValueOrDefault()) : "-";
                model.StartDate = DateTimeHelper.TopersianDate(x.StartDate);
                model.WorkProgramString = x.SpecialDayGrouping.Title;

                model.StartDate = $"{model.StartDate}";

                model.SpecialDayGroupingString = x.SpecialDayGroupingId == null ? "-" : x.SpecialDayGrouping.Title;
                //model.WorkProgramString = x.WorkProgramId != null && x.Type == Core.Enumerator.SpecialDayType.SpecialDuration ? x.WorkProgram.Title : "-";
                switch (x.Type)
                {
                    case Core.Enumerator.SpecialDayType.Holiday:
                        model.TypeString = "تعطیلات";
                        break;
                    case Core.Enumerator.SpecialDayType.SpecialDay:
                        model.TypeString = "روز خاص";
                        break;
                    case Core.Enumerator.SpecialDayType.SpecialDuration:
                        model.TypeString = "ایام خاص";
                        break;
                    default:
                        model.TypeString = "-";
                        break;
                }

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add or Update
        [HttpGet]
        public ActionResult AddOrUpdate(int id = 0)
        {
            SpecialDayModel model = new SpecialDayModel();
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(SpecialDayModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    SpecialDay entity = new SpecialDay();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                    }
                    else
                    {
                        var entityList = _specialDayService.GetList;
                        if (entityList != null && entityList.Count() > 0)
                        {
                            entityList = entityList.Where(x => x.SpecialDayGroupingId == model.SpecialDayGroupingId).ToList();
                            if (model.TypeId == 1 && entityList != null && entityList.Count() > 0)
                            {
                                entityList = CheckListOne(model, entityList);
                                if (entityList.Count > 0)
                                {
                                    model = PrepareCreate(model, jsonModel, entity);
                                }
                                else if (model.TypeId == 2 && entityList != null && entityList.Count() > 0)
                                {
                                    jsonModel.State = "15";
                                    jsonModel.Message = "تاریخ تکراری میباشد";
                                }
                            }
                            else
                            {
                                entityList = CheckListTwo(model, entityList);
                                if (entityList.Count >= 0)
                                {
                                    model = PrepareCreate(model, jsonModel, entity);
                                }
                                else
                                {
                                    jsonModel.State = "15";
                                    jsonModel.Message = "بازه تاریخی تکراری میباشد";
                                }
                            }

                        }
                        else
                        {
                            model = PrepareCreate(model, jsonModel, entity);
                        }

                    }

                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }
            jsonModel.Html = HtmlToJsonHelper.RenderPartialView(this, "_AddOrUpdate", model);
            return Json(jsonModel);
        }

        private static List<SpecialDay> CheckListTwo(SpecialDayModel model, List<SpecialDay> entityList)
        {
            entityList = entityList.Where(x => x.Type == SpecialDayType.Holiday).ToList();
            if (entityList != null && entityList.Count() > 0)
                entityList = entityList
                    .Where(x => 
                    (x.StartDate.Date >= model.StartDate.ToGeoDate().GetValueOrDefault() && x.EndDate.GetValueOrDefault().Date <= model.StartDate.ToGeoDate().GetValueOrDefault()) ||
                     (x.StartDate.Date >= model.EndDate.ToGeoDate().GetValueOrDefault() && x.EndDate.GetValueOrDefault().Date <= model.EndDate.ToGeoDate().GetValueOrDefault())
                     ).ToList();
            return entityList;
        }

        private static List<SpecialDay> CheckListOne(SpecialDayModel model, List<SpecialDay> entityList)
        {
            entityList = entityList.Where(x => x.Type == SpecialDayType.Holiday).ToList();
            if (entityList != null && entityList.Count() > 0)
                entityList = entityList.Where(x => x.StartDate.Date == model.StartDate.ToGeoDate().GetValueOrDefault()).ToList();
            return entityList;
        }

        private SpecialDayModel PrepareCreate(SpecialDayModel model, JsonModel jsonModel, SpecialDay entity)
        {
            entity.Id = entity.Id;
            entity.EndDate = entity.EndDate;
            entity.StartDate = DateTimeHelper.ToGeoDate(model.StartDate).GetValueOrDefault();
            entity.Title = model.Title;
            entity.DelayEnterance = model.DelayEnterance;
            entity.DelayExit = model.DelayExit;
            entity.SpecialDayGroupingId = model.SpecialDayGroupingId;
            entity.Type = (SpecialDayType)model.TypeId;
            _specialDayService.Create(entity);


            CommandHelper.Create(CommandCategory.SpecialDayChange, false, $"ایجاد تعطیلی/روزخاص در تقویم با نام : {entity.Title}", 0, UserHelper.CurrentUser(), entity.Id);

            //CalendarHistoryHelper.UpdateBySpecialDay(entity.Id);

            model = new SpecialDayModel();
            jsonModel.Success();
            return model;
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            bool result = false;
            try
            {
                SpecialDay entity = _specialDayService.FindById(id);
                _specialDayService.Delete(entity);
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
    }
}