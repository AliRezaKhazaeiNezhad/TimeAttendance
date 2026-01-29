using AutoMapper;
using System;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using wskh.Core;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        #region Propertices
        private readonly ITicketService _ticketService;
        #endregion
        #region Ctor
        public TicketController(ITicketService TicketService)
        {
            _ticketService = TicketService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PanelName = "ticket";
            ViewBag.MenuName = "ticket";

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
            var modelItem = new DataTableResponse<TicketModel>();
            modelItem.draw = request.draw;
            var data = _ticketService.FilterData(request.start, request.length, filter.Search);
            modelItem.recordsTotal = _ticketService.Count(filter.Search);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new TicketModel();
                model.Index = ++add;
                model.Id = x.Id;
                model.Title = x.Title;
                model.StateString = x.State == Core.Enumerator.TicketState.Pending ? "درانتظار پاسخ" : "پاسخ داده شده";
                model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Delete2({x.Id})'><span class='dropdown-item '> حذف </span></li><li onclick='Show({x.Id})'><span class='dropdown-item '> نمایش </span></li><li onclick='Response({x.Id})'><span class='dropdown-item '> پاسخ </span></li></ul></div>";


                var currentUser = UserHelper.CurrentUser();
                if (currentUser.UserRoleType != "610064006D0069006E00n" || (currentUser.UserRoleType == "610064006D0069006E00n" && x.State == Core.Enumerator.TicketState.Responsed))
                    model.Buttom = $"<div class='dropdown mPointer'><button class='btn btn-dark btn-sm dropdown-toggle' type='button' data-toggle='dropdown' aria-expanded='true'>عملیات<span class='caret'></span></button><ul class='dropdown-menu'><li onclick='Show({x.Id})'><span class='dropdown-item '> نمایش </span></li></ul></div>";


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
            TicketModel model = new TicketModel();
            if (id > 0)
            {
                Ticket entity = _ticketService.FindById(id);
                model.Title = entity.Title;
                model.Response = entity.Response;
                model.State = entity.State;
            }
            return PartialView("_AddOrUpdate", model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(TicketModel model)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    Ticket entity = new Ticket();
                    if (model.Id.GetValueOrDefault() > 0)
                    {
                    }
                    else
                    {
                        var currentUser = UserHelper.CurrentUser();
                        entity = new Ticket();
                        entity.Title = model.Title;
                        entity.Response = model.Response;
                        entity.Description = model.Description;
                        entity.State = Core.Enumerator.TicketState.Pending;
                        entity.RequestUserId = currentUser.Id;
                        _ticketService.Create(entity);
                    }
                    model = new TicketModel();
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
                Ticket entity = _ticketService.FindById(id);
                entity.Remove = true;
                _ticketService.Update(entity);
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


        #region Response
        [HttpGet]
        public ActionResult Response(int id = 0)
        {
            TicketModel model = new TicketModel();
            if (id > 0)
            {
                Ticket entity = _ticketService.FindById(id);
                model.Title = entity.Title;
                model.Response = entity.Response;
                model.State = entity.State;
                model.Id = entity.Id;
            }
            return PartialView("_Response", model);
        }
        [HttpPost]
        public ActionResult Response(int id, string response)
        {
            JsonModel jsonModel = new JsonModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = UserHelper.CurrentUser();
                    var entity = _ticketService.FindById(id);
                    entity.Response = response;
                    entity.State = Core.Enumerator.TicketState.Responsed;
                    entity.ResponseUserId = currentUser.Id;
                    _ticketService.Update(entity);

                    jsonModel.Success();
                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }
            return Json(jsonModel);
        }
        #endregion
        #region Show
        public ActionResult Show(int id)
        {
            var entity = _ticketService.FindById(id);
            TicketModel model = new TicketModel();
            model.Title = entity.Title;
            model.Response = entity.Response;
            model.State = entity.State;
            return PartialView("_Show", model);
        }
        #endregion
    }
}