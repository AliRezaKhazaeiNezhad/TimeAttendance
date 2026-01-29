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
    public class OrganizationLevelController : Controller
    {
        #region Propertices
        private readonly IOrganizationLevelService _organizationLevelService;
        #endregion
        #region Ctor
        public OrganizationLevelController(IOrganizationLevelService OrganizationLevelService)
        {
            _organizationLevelService = OrganizationLevelService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "organizationlevel";
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
            var modelItem = new DataTableResponse<OrganizationLevelModel>();
            modelItem.draw = request.draw;
            var data = _organizationLevelService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _organizationLevelService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new OrganizationLevelModel();
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
            OrganizationLevelModel model = new OrganizationLevelModel();
            if (id > 0)
            {
                OrganizationLevel entity = _organizationLevelService.FindById(id);
                model = Mapper.Map<OrganizationLevel, OrganizationLevelModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(OrganizationLevelModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    OrganizationLevel entity = new OrganizationLevel();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new OrganizationLevel();
                        entity = Mapper.Map<OrganizationLevelModel, OrganizationLevel>(model);
                        _organizationLevelService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<OrganizationLevelModel, OrganizationLevel>(model);
                        _organizationLevelService.Create(entity);
                    }
                    model = new OrganizationLevelModel();
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
                OrganizationLevel entity = _organizationLevelService.FindById(id);
                if (entity.Users != null && entity.Users.Count > 0)
                {
                    result = -1;
                }
                else
                {
                    _organizationLevelService.Delete(entity);
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