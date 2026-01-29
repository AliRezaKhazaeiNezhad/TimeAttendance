using AutoMapper;
using System;
using System.IO;
using System.Linq;
using System.Web;
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
    public class OrganizationInformationController : Controller
    {
        #region Propertices
        private readonly IOrganizationInformationService _organizationInformationService;
        #endregion
        #region Ctor
        public OrganizationInformationController(IOrganizationInformationService OrganizationInformationService)
        {
            _organizationInformationService = OrganizationInformationService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index(string message = null, string errors = null)
        {
            ViewBag.MenuName = "organizationinformation";

            OrganizationInformationModel model = new OrganizationInformationModel();

            var entity = _organizationInformationService.GetList.FirstOrDefault();
            model.Title = entity.Title;
            model.Category = entity.Category;
            model.Address = entity.Address;
            model.Phone = entity.Phone;
            model.LogoPath = entity.LogoPath;
            model.Message = message;
            model.Errors = errors;
            model.Id = entity.Id;

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(OrganizationInformationModel model, HttpPostedFileBase LogoPath)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrganizationInformation entity = _organizationInformationService.GetList.FirstOrDefault();
                    entity.Title = model.Title;
                    entity.Category = model.Category;
                    entity.Address = model.Address;
                    entity.Phone = model.Phone;
                    if (LogoPath != null)
                    {
                        var path = Path.Combine(Server.MapPath("/wwwroot/dashboard/img/"), LogoPath.FileName);
                        LogoPath.SaveAs(path);
                        entity.LogoPath = $"/wwwroot/dashboard/img/{LogoPath.FileName}";
                    }



                    _organizationInformationService.Update(entity);
                    model.Message = "اطلاعات با موفقیت بروزرسانی شد";
                }
                catch (Exception e)
                {
                    model.Errors = "خطایی رخ داده است. در فرصتی دیگر تلاش نمایید";
                }
            }
            else
                model.Errors = "اطلاعات ضروری را تکمیل نمایید";


            return RedirectToAction("Index", new { id = model.Id, message = model.Message, errors = model.Errors });
        }
        #endregion
    }
}