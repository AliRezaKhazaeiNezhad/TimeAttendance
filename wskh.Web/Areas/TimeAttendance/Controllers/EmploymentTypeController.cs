using AutoMapper;
using System;
using System.Web.Mvc;
using wskh.Core;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class EmploymentTypeController : Controller
    {
        #region Propertices
        private readonly IEmploymentTypeService _EmploymentTypeService;
        #endregion
        #region Ctor
        public EmploymentTypeController(IEmploymentTypeService EmploymentTypeService)
        {
            _EmploymentTypeService = EmploymentTypeService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "employmenttype";
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
            var modelItem = new DataTableResponse<EmploymentTypeModel>();
            modelItem.draw = request.draw;
            var data = _EmploymentTypeService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _EmploymentTypeService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new EmploymentTypeModel();
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
            EmploymentTypeModel model = new EmploymentTypeModel();
            if (id > 0)
            {
                EmploymentType entity = _EmploymentTypeService.FindById(id);
                model = Mapper.Map<EmploymentType, EmploymentTypeModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(EmploymentTypeModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    EmploymentType entity = new EmploymentType();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new EmploymentType();
                        entity = Mapper.Map<EmploymentTypeModel, EmploymentType>(model);
                        _EmploymentTypeService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<EmploymentTypeModel, EmploymentType>(model);
                        _EmploymentTypeService.Create(entity);
                    }
                    model = new EmploymentTypeModel();
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
            int result = 0;
            try
            {
                EmploymentType entity = _EmploymentTypeService.FindById(id);
                if (entity.Users != null && entity.Users.Count > 0)
                {
                    result = -1;
                }
                else
                {
                    _EmploymentTypeService.Delete(entity);
                    result = 1;
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