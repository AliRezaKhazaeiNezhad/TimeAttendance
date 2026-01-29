using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using wskh.Core;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class LeaveTypeController : Controller
    {
        #region Propertices
        private readonly ILeaveTypeService _LeaveTypeService;
        #endregion
        #region Ctor
        public LeaveTypeController(ILeaveTypeService LeaveTypeService)
        {
            _LeaveTypeService = LeaveTypeService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "leavetype";

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
            var modelItem = new DataTableResponse<LeaveTypeModel>();
            modelItem.draw = request.draw;
            var data = _LeaveTypeService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _LeaveTypeService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new LeaveTypeModel();
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
            LeaveTypeModel model = new LeaveTypeModel();
            if (id > 0)
            {
                LeaveType entity = _LeaveTypeService.FindById(id);
                model.Title = entity.Title;
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(LeaveTypeModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    LeaveType entity = new LeaveType();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = _LeaveTypeService.FindById(model.Id.GetValueOrDefault());
                        if (entity.AllowRemove)
                        {
                            entity.Title = model.Title;
                            _LeaveTypeService.Update(entity);
                            jsonModel.Success();
                        }
                        else
                            jsonModel.State = "-2";


                    }
                    else
                    {
                        entity.Title = model.Title;
                        entity.AllowRemove = true;
                        _LeaveTypeService.Create(entity);
                    }
                    model = new LeaveTypeModel();
                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }
            jsonModel.Html = HtmlToJsonHelper.RenderPartialView(this, "_AddOrUpdate", model);
            return Json(jsonModel);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int id = 0)
        {
            int result = 0;
            try
            {
                LeaveType entity = _LeaveTypeService.FindById(id);
                if (entity.Requests != null && entity.Requests.Where(x => x.Remove == false).ToList().Count > 0)
                {
                    result = -1;
                }
                else
                {
                    if (entity.AllowRemove)
                    {
                        entity.Remove = true;
                        _LeaveTypeService.Update(entity);
                        result = 1;
                    }
                    else
                    {
                        result = -4;
                    }
                }
            }
            catch (Exception e)
            {
                result = -2;
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