using AutoMapper;
using System;
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
    public class SpecialDayGroupingController : Controller
    {
        #region Propertices
        private readonly ISpecialDayGroupingService _SpecialDayGroupingService;
        #endregion
        #region Ctor
        public SpecialDayGroupingController(ISpecialDayGroupingService SpecialDayGroupingService)
        {
            _SpecialDayGroupingService = SpecialDayGroupingService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "specialdaygrouping";

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
            var modelItem = new DataTableResponse<SpecialDayGroupingModel>();
            modelItem.draw = request.draw;
            var data = _SpecialDayGroupingService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _SpecialDayGroupingService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new SpecialDayGroupingModel();
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
            SpecialDayGroupingModel model = new SpecialDayGroupingModel();
            if (id > 0)
            {
                SpecialDayGrouping entity = _SpecialDayGroupingService.FindById(id);
                model = Mapper.Map<SpecialDayGrouping, SpecialDayGroupingModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(SpecialDayGroupingModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    SpecialDayGrouping entity = new SpecialDayGrouping();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new SpecialDayGrouping();
                        entity = Mapper.Map<SpecialDayGroupingModel, SpecialDayGrouping>(model);
                        _SpecialDayGroupingService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<SpecialDayGroupingModel, SpecialDayGrouping>(model);
                        _SpecialDayGroupingService.Create(entity);
                    }
                    model = new SpecialDayGroupingModel();
                    jsonModel.Success();
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
            bool result = false;
            try
            {
                SpecialDayGrouping entity = _SpecialDayGroupingService.FindById(id);
                _SpecialDayGroupingService.Delete(entity);
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