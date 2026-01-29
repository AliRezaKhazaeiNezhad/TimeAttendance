using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.DateAndTime;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class OrdinaryWorkProgramController : Controller
    {
        #region Propertices
        private readonly IWorkProgramService _workProgramService;
        #endregion
        #region Ctor
        public OrdinaryWorkProgramController(IWorkProgramService workProgramService)
        {
            _workProgramService = workProgramService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "ordinaryworkprogram";
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
            var modelItem = new DataTableResponse<WorkProgramGridModel>();
            modelItem.draw = request.draw;
            var data = _workProgramService.FilterData(request.start, request.length, filter.Search, WorkProgramType.All);
            modelItem.recordsTotal = _workProgramService.Count(filter.Search, WorkProgramType.All);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new WorkProgramGridModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = x.Title;
                model.CreateDateTime = DateTimeHelper.TopersianDate(x.CreateDateTime.GetValueOrDefault());
                switch (x.Type)
                {
                    case WorkProgramType.All:
                        model.TypeInGrid = "نامشخص";
                        break;
                    case WorkProgramType.Ordinary:
                        model.TypeInGrid = "منظم/چرخشی";
                        break;
                    case WorkProgramType.Flow:
                        model.TypeInGrid = "شناور";
                        break;
                    case WorkProgramType.Complex:
                        model.TypeInGrid = "بیمارستانی/نگهبانی";
                        break;
                    default:
                        model.TypeInGrid = "-";
                        break;
                }

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AddOrUpdate
        public ActionResult AddOrUpdate(int id = 0)
        {
            ViewBag.PanelName = "rulesandprograms";
            ViewBag.MenuName = "ordinaryworkprogram";

            OrdinaryWorkProgramModel model = new OrdinaryWorkProgramModel() { WorkProgramType = -1 };
            try
            {
                if (id > 0)
                {
                    WorkProgram entity = _workProgramService.FindById(id);
                }
                else
                    model.OrdinaryWorkProgramDayModels.Add(new OrdinaryWorkProgramDayModel()
                    {
                        DayIndex = 1,
                        WorkType = WorkType.WorkDay
                    });
            }
            catch (Exception e)
            {
            }

            Session["OrdinaryWorkProgramModel"] = model;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(OrdinaryWorkProgramModel model)
        {
            ViewBag.PanelName = "rulesandprograms";
            ViewBag.MenuName = "ordinaryworkprogram";
            var entity = new WorkProgram();
            bool result = false;


            if (ModelState.IsValid)
            {
                try
                {
                    OrdinaryWorkProgramModel modelSession = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                    model.WorkProgramType = modelSession.WorkProgramType;
                    model.Type = (WorkProgramType)model.WorkProgramType;

                    if (model.Id == 0)
                    {
                        entity.Title = model.Title;
                        //entity.CalculateAbsence = model.CalculateAbsence;
                        //براساس نوع برنامه کاری عملیات را مدیریت میکند
                        switch (model.Type)
                        {
                            case WorkProgramType.Ordinary:
                                OrdinaryProgramEntityToModel(entity, modelSession);
                                break;
                            case WorkProgramType.Flow:
                                FlowProgramEntityToModel(model, entity, modelSession);
                                break;
                            case WorkProgramType.Complex:
                                ComplexProgramEntityToModel(entity, modelSession);
                                break;
                            default:
                                break;
                        }
                        if (model.Type != WorkProgramType.All)
                        {
                            entity = DateTimeHelper.CalculateTotalMinutes(entity);
                            entity.CreateDateTime = DateTime.Now;
                            _workProgramService.Create(entity);
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            if (result)
                return RedirectToAction("Index", "OrdinaryWorkProgram");
            else
                return View(model);
        }




        #endregion
        #region GetTable
        public ActionResult GetTable()
        {
            return PartialView("_Table");
        }
        #endregion
        #region AddDay
        public ActionResult AddDay()
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                model.OrdinaryWorkProgramDayModels.Add(new OrdinaryWorkProgramDayModel()
                {
                    DayIndex = model.OrdinaryWorkProgramDayModels.Count() + 1,
                    WorkType = Core.Enumerator.WorkType.WorkDay,
                });
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DeleteDay
        public ActionResult DeleteDay(int dayIndex = 0)
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                var selectedDay = model.OrdinaryWorkProgramDayModels.Where(x => x.DayIndex == dayIndex).FirstOrDefault();
                model.OrdinaryWorkProgramDayModels.Remove(selectedDay);
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ChangeDayType
        public ActionResult ChangeWorkType(int dayIndex = 0, int workType = 0)
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                var selectedDay = model.OrdinaryWorkProgramDayModels[(dayIndex - 1)];
                switch (workType)
                {
                    case 0:
                        selectedDay.WorkType = WorkType.WorkDay;
                        selectedDay.WorkTypeId = 0;
                        break;
                    case 1:
                        selectedDay.WorkType = WorkType.Holiday;
                        selectedDay.WorkTypeId = 1;
                        break;
                    case 2:
                        selectedDay.WorkType = WorkType.RestDay;
                        selectedDay.WorkTypeId = 2;
                        break;
                    default:
                        selectedDay.WorkType = WorkType.WorkDay;
                        selectedDay.WorkTypeId = 0;
                        break;
                };

                model.OrdinaryWorkProgramDayModels[(dayIndex - 1)] = selectedDay;
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ChangeDayIndex
        public ActionResult ChangeDayIndex(int index = 0)
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                model.WorkProgramType = index;
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region RefreshCol
        public ActionResult ResetDay(int index = 0)
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                model.OrdinaryWorkProgramDayModels[index - 1] = new OrdinaryWorkProgramDayModel() { DayIndex = index };
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CoppyCol
        public ActionResult CoppyCol(int index = 0)
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = new OrdinaryWorkProgramModel();

                model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];

                var oldDay = model.OrdinaryWorkProgramDayModels[index - 1];

                OrdinaryWorkProgramDayModel newDay = new OrdinaryWorkProgramDayModel()
                {
                    DayIndex = oldDay.DayIndex + 1,
                    OrdinaryWorkProgramTimeModels = oldDay.OrdinaryWorkProgramTimeModels,
                    WorkType = oldDay.WorkType,
                };
                model.OrdinaryWorkProgramDayModels.Add(newDay);
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetDuration
        public ActionResult GetDuration(string startTime, string endTime)
        {
            return Json(TimeHelper.Duration(startTime, endTime), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CoppyCol
        public ActionResult AddDayTime(int programType = 0, int dayIndex = 0, string workTimeInDay = null, bool moreThan24Houres = false, int workType = 0, string startTime = null, string endTime = null)
        {
            /// 0 برای خطا
            /// 1 صحیح
            /// 2 تکراری بودن بازه زمانی در لیست
            int state = 0;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                var day = model.OrdinaryWorkProgramDayModels[dayIndex - 1];
                model.Type = (WorkProgramType)programType;
                model.WorkProgramType = programType;
                model.MoreThan24Houres = moreThan24Houres;


                /*برنامه شناور*/
                if (programType == 2)
                {
                    day = model.OrdinaryWorkProgramDayModels[0];
                    day.DayIndex = dayIndex;
                    day.OrdinaryWorkProgramTimeModels = new List<OrdinaryWorkProgramTimeModel>();
                    day.OrdinaryWorkProgramTimeModels.Add(new OrdinaryWorkProgramTimeModel()
                    {
                        /*مدت زمان کار بصورت شناور در روز*/
                        Duration = workTimeInDay,
                        TimeType = TimeType.WorkTime,
                        Id = 0
                    });
                    day.WorkType = WorkType.WorkDay;
                    state = 1;
                }

                /*برنامه منظم*/
                if (programType == 1)
                {
                    if (TimeHelper.DuratinAllow(day.OrdinaryWorkProgramTimeModels, startTime, endTime) == true)
                    {
                        day.OrdinaryWorkProgramTimeModels.Add(new OrdinaryWorkProgramTimeModel()
                        {
                            Duration = TimeHelper.Duration(startTime, endTime),
                            StartTime = startTime,
                            EndTime = endTime,
                            Id = 0,
                            TimeType = (TimeType)workType,
                        });
                        state = 1;
                    }
                    else
                        state = 2;
                }

                /*برنامه نگهبانی /بیمارستانی*/
                if (programType == 3)
                {
                    if (TimeHelper.DuratinAllow(day.OrdinaryWorkProgramTimeModels, startTime, endTime) == true)
                    {
                        day.OrdinaryWorkProgramTimeModels.Add(new OrdinaryWorkProgramTimeModel()
                        {
                            Duration = TimeHelper.Duration(startTime, endTime),
                            StartTime = startTime,
                            EndTime = endTime,
                            Id = 0,
                            TimeType = (TimeType)workType,
                        });
                        state = 1;
                    }
                    else
                        state = 2;
                }

                Session["OrdinaryWorkProgramModel"] = model;
            }
            catch (Exception e)
            {
            }
            return Json(state, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetDay
        public ActionResult GetDay(int dayIndex = 0)
        {
            OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
            var day = model.OrdinaryWorkProgramDayModels[dayIndex - 1];
            return PartialView("_GetDay", day);
        }
        #endregion
        #region DeleteTime
        public ActionResult DeleteTime(int index = 0, int dayIndex = 0)
        {
            bool result = false;
            try
            {
                OrdinaryWorkProgramModel model = (OrdinaryWorkProgramModel)Session["OrdinaryWorkProgramModel"];
                var selectedDay = model.OrdinaryWorkProgramDayModels[dayIndex].OrdinaryWorkProgramTimeModels[index];
                model.OrdinaryWorkProgramDayModels[dayIndex].OrdinaryWorkProgramTimeModels.Remove(selectedDay);
                Session["OrdinaryWorkProgramModel"] = model;
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetProgram
        public ActionResult GetProgram(int id = 0)
        {
            var entity = _workProgramService.FindById(id);
            entity.WorkProgramDays = entity.WorkProgramDays.OrderBy(x => x.DayIndex).ToList();
            return View(entity);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            bool result = false;
            try
            {
                WorkProgram entity = _workProgramService.FindById(id);
                _workProgramService.Delete(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrepareMethods Entity To Model
        private static void OrdinaryProgramEntityToModel(WorkProgram entity, OrdinaryWorkProgramModel modelSession)
        {
            entity.Type = WorkProgramType.Ordinary;
            foreach (var day in modelSession.OrdinaryWorkProgramDayModels)
            {
                var dayEntity = new WorkProgramDay();
                dayEntity.DayIndex = day.DayIndex;
                dayEntity.Remove = false;
                dayEntity.WorkType = (WorkType)day.WorkTypeId;
                dayEntity.WorkProgramTimes = new List<WorkProgramTime>();
                foreach (var time in day.OrdinaryWorkProgramTimeModels)
                {
                    WorkProgramTime timeEntity = new WorkProgramTime();
                    timeEntity.StartTime = time.StartTime;
                    timeEntity.EndTime = time.EndTime;
                    timeEntity.TimeType = time.TimeType;
                    timeEntity.Duration = time.Duration;
                    dayEntity.WorkProgramTimes.Add(timeEntity);
                }
                entity.WorkProgramDays.Add(dayEntity);
            }
        }
        private static void ComplexProgramEntityToModel(WorkProgram entity, OrdinaryWorkProgramModel modelSession)
        {
            entity.Type = WorkProgramType.Complex;
            foreach (var day in modelSession.OrdinaryWorkProgramDayModels)
            {
                var dayEntity = new WorkProgramDay();
                dayEntity.DayIndex = day.DayIndex;
                dayEntity.Remove = false;
                dayEntity.WorkType = (WorkType)day.WorkTypeId;
                dayEntity.WorkProgramTimes = new List<WorkProgramTime>();
                foreach (var time in day.OrdinaryWorkProgramTimeModels)
                {
                    WorkProgramTime timeEntity = new WorkProgramTime();
                    timeEntity.StartTime = time.StartTime;
                    timeEntity.EndTime = time.EndTime;
                    timeEntity.TimeType = time.TimeType;
                    timeEntity.Duration = time.StartTime == time.EndTime ? "24:00" : time.Duration;
                    dayEntity.WorkProgramTimes.Add(timeEntity);
                }
                entity.WorkProgramDays.Add(dayEntity);
            }
        }
        private static void FlowProgramEntityToModel(OrdinaryWorkProgramModel model, WorkProgram entity, OrdinaryWorkProgramModel modelSession)
        {
            entity.Type = WorkProgramType.Flow;
            entity.MoreThan24Houres = modelSession.MoreThan24Houres;
            if (modelSession.OrdinaryWorkProgramDayModels[0].OrdinaryWorkProgramTimeModels.Count() > 0)
                entity.WorkTimeInDay = modelSession.OrdinaryWorkProgramDayModels[0].OrdinaryWorkProgramTimeModels[0].Duration;
            else
                entity.WorkTimeInDay = "00:00";
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