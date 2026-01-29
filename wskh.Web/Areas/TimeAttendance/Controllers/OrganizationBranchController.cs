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
    public class OrganizationBranchController : Controller
    {
        #region Propertices
        private readonly IOrganizationBranchService _OrganizationBranchService;
        #endregion
        #region Ctor
        public OrganizationBranchController(IOrganizationBranchService OrganizationBranchService)
        {
            _OrganizationBranchService = OrganizationBranchService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "organizationbranch";
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
            var modelItem = new DataTableResponse<OrganizationBranchModel>();
            modelItem.draw = request.draw;
            var data = _OrganizationBranchService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _OrganizationBranchService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new OrganizationBranchModel();
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
            OrganizationBranchModel model = new OrganizationBranchModel();
            if (id > 0)
            {
                OrganizationBranch entity = _OrganizationBranchService.FindById(id);
                model = Mapper.Map<OrganizationBranch, OrganizationBranchModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(OrganizationBranchModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    OrganizationBranch entity = new OrganizationBranch();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new OrganizationBranch();
                        entity = Mapper.Map<OrganizationBranchModel, OrganizationBranch>(model);
                        _OrganizationBranchService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<OrganizationBranchModel, OrganizationBranch>(model);
                        _OrganizationBranchService.Create(entity);
                    }
                    model = new OrganizationBranchModel();
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
                OrganizationBranch entity = _OrganizationBranchService.FindById(id);
                if (entity.Users != null && entity.Users.Count > 0)
                {
                    result = -1;
                }
                else
                {
                    _OrganizationBranchService.Delete(entity);
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