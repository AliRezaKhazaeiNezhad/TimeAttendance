using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.NotificationHelper;
using wskh.Core;
using wskh.Data;
using wskh.Model;
using wskh.Service;
using wskh.Web.Helper;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class TicketToDevelopersController : Controller
    {
        #region Propertices
        private IOrganizationInformationService _informationService;
        #endregion
        #region Ctor
        public TicketToDevelopersController(IOrganizationInformationService informationService)
        {
            _informationService = informationService;
        }
        #endregion
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MenuName = "tickettodevelopers";

            return View();
        }
        #endregion


        #region Response
        [HttpPost]
        public ActionResult Response(string categoryId, string description)
        {
            JsonModel jsonModel = new JsonModel();
            string message = "";
            string telegramMessage = "";
            string subString = description.Length > 20 ? description.Substring(0, 19) : "";
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = UserHelper.CurrentUser();
                    var organizationInformation = _informationService.GetList.FirstOrDefault();
                    var currenDateTime = DateTimeHelper.TopersianFull(DateTime.Now);

                    message = $"تاریخ : {currenDateTime}";
                    message = message + $" \r\nاطلاعات شرکت: \r\n {organizationInformation.Title}\r\n {organizationInformation.Phone}\r\n {organizationInformation.Category}\r\n {organizationInformation.Address}";
                    message = message + $" \r\nاطلاعات کاربر: \r\n {HashHelper.Decrypt(currentUser.UserName)}";
                    message = message + $" \r\nشرح مختصر درخواست: \r\n {subString}\r\n";


                    telegramMessage = message;
                    telegramMessage = telegramMessage + $" \r\nگروه: \r\n{categoryId}";
                    telegramMessage = telegramMessage + $" \r\nشرح درخواست: \r\n{description}";


                    jsonModel.Success();
                    try
                    {
                        Telegram.Send(telegramMessage);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        SMSHelper.Send(message);
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception e)
                {
                    jsonModel.Exception();
                }
            }
            return Json(jsonModel);
        }
        #endregion
    }
}