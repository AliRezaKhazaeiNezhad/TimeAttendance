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
    public class EducationLevelController : Controller
    {
        #region Propertices
        private readonly IEducationLevelService _EducationLevelService;
        #endregion
        #region Ctor
        public EducationLevelController(IEducationLevelService EducationLevelService)
        {
            _EducationLevelService = EducationLevelService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "educationlevel";

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
            var modelItem = new DataTableResponse<EducationLevelModel>();
            modelItem.draw = request.draw;
            var data = _EducationLevelService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _EducationLevelService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new EducationLevelModel();
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
            EducationLevelModel model = new EducationLevelModel();
            if (id > 0)
            {
                EducationLevel entity = _EducationLevelService.FindById(id);
                model = Mapper.Map<EducationLevel, EducationLevelModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(EducationLevelModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    EducationLevel entity = new EducationLevel();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new EducationLevel();
                        entity = Mapper.Map<EducationLevelModel, EducationLevel>(model);
                        _EducationLevelService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<EducationLevelModel, EducationLevel>(model);
                        _EducationLevelService.Create(entity);
                    }
                    model = new EducationLevelModel();
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
                EducationLevel entity = _EducationLevelService.FindById(id);
                if (entity.Users != null && entity.Users.Count > 0)
                {
                    result = -1;
                }
                else
                {
                    _EducationLevelService.Delete(entity);
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