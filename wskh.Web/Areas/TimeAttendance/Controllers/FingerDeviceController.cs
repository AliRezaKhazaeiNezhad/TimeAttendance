using AutoMapper;
using BioBridgeSDKDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.StringAndNumber;
using wskh.Core;
using wskh.FingerTec;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class FingerDeviceController : Controller
    {
        #region Services
        private readonly IFingerDeviceService _fingerDeviceService;
        #endregion
        #region Ctor
        public FingerDeviceController(IFingerDeviceService fingerDeviceService)
        {
            _fingerDeviceService = fingerDeviceService;
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            ViewBag.MenuName = "fingerdevice";
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
            var modelItem = new DataTableResponse<FingerDeviceModel>();
            modelItem.draw = request.draw;
            var data = _fingerDeviceService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _fingerDeviceService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new FingerDeviceModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = $"{x.Title.IfNull()}";
                model.IP = $"{x.IP.IfNull()}";
                model.PortNo = $"{x.PortNo.IfNull()}";
                model.SerialNo = $"{x.SerialNo.IfNull()}";
                model.MacAddress = $"{x.MacAddress.IfNull()}";
                model.ModelName = $"{x.ModelName.IfNull()}";
                model.IsColorScreenString = x.IsColorScreen ? "رنگی" : "سیاه و سفید";


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
            FingerDeviceModel model = new FingerDeviceModel();
            if (id > 0)
            {
                FingerDevice entity = _fingerDeviceService.FindById(id);
                model = Mapper.Map<FingerDevice, FingerDeviceModel>(entity);
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(FingerDeviceModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    FingerDevice entity = new FingerDevice();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                        entity = new FingerDevice();
                        entity = Mapper.Map<FingerDeviceModel, FingerDevice>(model);
                        _fingerDeviceService.Update(entity);
                    }
                    else
                    {
                        entity = Mapper.Map<FingerDeviceModel, FingerDevice>(model);
                        _fingerDeviceService.Create(entity);
                    }
                    model = new FingerDeviceModel();
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
                FingerDevice entity = _fingerDeviceService.FindById(id);
                entity.Remove = true;
                _fingerDeviceService.Update(entity);
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