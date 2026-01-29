using BioBridgeSDKDLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Service;
using TimeAttendance.Web.Helper;
using TimeAttendance.WebEssentials.CommandPart;
using wskh.Core.Enumerator;
using wskh.FingerTec;
using wskh.FingerTec.Models;
using wskh.LogAndEnrlol.analyzer.CRUD;
using wskh.LogAndEnrlol.analyzer.SQL;
using wskh.Model;
using wskh.Service;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class DeviceCommandController : Controller
    {
        #region Propertices
        private readonly IFingerDeviceService _fingerDeviceService;
        private readonly IRawEnrollService _rawEnrollService;
        private readonly IRawLogService _rawLogService;
        private readonly IDeviceCardService _deviceCardService;
        private readonly ILogService _logService;
        private readonly IEnrollService _enrollService;
        private readonly SQLRepository _sqlRepository;
        private readonly IDeviceWorkCodeService _deviceWorkCodeService;
        #endregion
        #region Ctor
        public DeviceCommandController(IFingerDeviceService fingerDeviceService, IRawEnrollService rawEnrollService, IRawLogService rawLogService, IDeviceCardService deviceCardService, ILogService logService, IEnrollService enrollService, IDeviceWorkCodeService deviceWorkCodeService)
        {
            _fingerDeviceService = fingerDeviceService;
            _rawEnrollService = rawEnrollService;
            _rawLogService = rawLogService;
            _deviceCardService = deviceCardService;
            _logService = logService;
            _enrollService = enrollService;
            _deviceWorkCodeService = deviceWorkCodeService;

            _sqlRepository = new SQLRepository(ConfigurationManager.ConnectionStrings["wskhContext"].ConnectionString);
        }
        #endregion
        #region Actions
        public ActionResult Actions(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_Actions", entity);
        }
        #endregion
        #region SetDateTime
        public ActionResult SetDateTime(int deviceId)
        {
            bool result = false;
            try
            {
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                fingerTecConnector.SetDateTime(DateTime.Now);
                fingerTecConnector.DisConnect();
                result = true;


                var entity = _fingerDeviceService.FindById(deviceId);
                CommandHelper.Create(CommandCategory.DeviceCommand, true, $"بروزرسانی تاریخ و زمان - در دستگاه {entity.Title}", 0, UserHelper.CurrentUser(), deviceId);
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ClearAdministrator
        public ActionResult ClearAdministrator(int deviceId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                fingerTecConnector.ClearAdministrator();
                fingerTecConnector.DisConnect();
                result = true;


                CommandHelper.Create(CommandCategory.DeviceCommand, true, $"حذف سطح دسترسی مدیران دستگاه - در دستگاه {entity.Title}", 0, UserHelper.CurrentUser(), deviceId);
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DeleteGeneralLog
        public ActionResult DeleteGeneralLog(int deviceId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                fingerTecConnector.DeleteGeneralLog();
                fingerTecConnector.DisConnect();
                result = true;




                CommandHelper.Create(CommandCategory.DeviceCommand, true, $"حذف همه ورود و خروج های دستگاه - در دستگاه {entity.Title}", 0, UserHelper.CurrentUser(), deviceId);
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ClearAllData
        public ActionResult ClearAllData(int deviceId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                fingerTecConnector.ClearAllData();
                fingerTecConnector.RestartDevice();
                fingerTecConnector.DisConnect();
                result = true;

                CommandHelper.Create(CommandCategory.DeviceCommand, true, $"حذف تمام اطلاعات دستگاه (Reset Factory) - در دستگاه {entity.Title}", 0, UserHelper.CurrentUser(), deviceId);
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region RestartDevice
        public ActionResult RestartDevice(int deviceId)
        {
            bool result = false;
            try
            {
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                fingerTecConnector.RestartDevice();
                fingerTecConnector.DisConnect();
                result = true;

                var entity = _fingerDeviceService.FindById(deviceId);

                CommandHelper.Create(CommandCategory.DeviceCommand, true, $"راه اندازی مجدد دستگاه (Restart) - در دستگاه {entity.Title}", 0, UserHelper.CurrentUser(), deviceId);

            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region IsConnected
        public ActionResult IsConnected(int deviceId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);

                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                result = fingerTecConnector.Connect();
                fingerTecConnector.DisConnect();

                CommandHelper.Create(CommandCategory.DeviceCommand, true, $"اتصال به دستگاه - در دستگاه {entity.Title}", 0, UserHelper.CurrentUser(), deviceId);
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update
        public ActionResult Update(int deviceId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();

                entity.SDKVersion = fingerTecConnector.GetSDKVersion();
                entity.FirmwareVersion = fingerTecConnector.GetFirmwareVersion();
                entity.SerialNo = fingerTecConnector.GetSerialNumber();
                entity.MacAddress = fingerTecConnector.GetMacAddress();
                entity.ModelName = fingerTecConnector.GetModel();
                entity.FTPDescription = fingerTecConnector.GetFTPDescription();
                entity.IsColorScreen = fingerTecConnector.IsColorScreen();
                entity.Manufacturer = fingerTecConnector.GetManufacturer();



                fingerTecConnector.DisConnect();

                _fingerDeviceService.Update(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetEnroll
        public ActionResult GetEnroll(int deviceId)
        {
            bool result = false;
            var rawEnrollList = new List<RawEnroll>();
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);

                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                var list = fingerTecConnector.GetEnrolls();
                fingerTecConnector.DisConnect();
                list.ForEach(x =>
                     _rawEnrollService.Create(new RawEnroll()
                     {
                         Enabled = x.Enable,
                         EnrollNo = x.EnrollNo,
                         Name = x.Name,
                         Password = x.PassWord,
                         Privileg = x.Privilage,
                         DeviceId = deviceId
                     })
                    );

                CommandHelper.Create(CommandCategory.EnrollCommand, false, $"دریافت لیست کاربران سخت افزار - در دستگاه {entity.Title}", list.Count, UserHelper.CurrentUser(), deviceId);
                //_sqlRepository.EnrollAnalyze();

                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetLog
        public ActionResult GetLog(int deviceId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, deviceId);
                fingerTecConnector.Connect();
                BulkCRUD.InsertLog(fingerTecConnector, entity);
                fingerTecConnector.DisConnect();


                
                //_sqlRepository.LogPreAnalyze();
                //LogToReportDay.Start();
                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CheckConnection
        public ActionResult CheckConnection(int portNo, int commKey, string iP, int deviceInnerId)
        {
            FingerDeviceModel model = new FingerDeviceModel()
            {
                Success = false
            };
            try
            {
                FingerTec.FingerTec fingerTecConnector = FingerTecHelper.Connector(ConnectionType.TCP, "", deviceInnerId, iP, portNo, commKey);
                model.Success = fingerTecConnector.Connect();
                if (model.Success)
                {
                    model.SDKVersion = fingerTecConnector.GetSDKVersion();
                    model.FirmwareVersion = fingerTecConnector.GetFirmwareVersion();
                    model.SerialNo = fingerTecConnector.GetSerialNumber();
                    model.MacAddress = fingerTecConnector.GetMacAddress();
                    model.ModelName = fingerTecConnector.GetModel();
                    model.FTPDescription = fingerTecConnector.GetFTPDescription();
                    model.IsColorScreen = fingerTecConnector.IsColorScreen();
                    model.Manufacturer = fingerTecConnector.GetManufacturer();
                }
                fingerTecConnector.DisConnect();
            }
            catch (Exception e)
            {
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region IP Duplicate
        public ActionResult IPDuplicate(string ip)
        {
            FingerDeviceModel model = new FingerDeviceModel()
            {
                Success = false
            };
            try
            {
                var list = _fingerDeviceService.GetList;
                if (list != null && list.Count() > 0)
                    list = list.Where(x => x.Remove == false).ToList();

                if (list != null && list.Count() > 0)
                    model.Success = list.Where(x => x.IP.Contains(ip)).Count() > 0 ? false : true;
                else
                    model.Success = true;
            }
            catch (Exception e)
            {
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ChangeFunctionKey
        public ActionResult ChangeFunctionKey(int deviceId, string keyName, int dropId)
        {
            bool result = false;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                switch (keyName)
                {
                    case "F1":
                        entity.F1 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "F2":
                        entity.F2 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "F3":
                        entity.F3 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "F4":
                        entity.F4 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "XXX":
                        entity.F5 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "F6":
                        entity.F6 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "F7":
                        entity.F7 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    case "F8":
                        entity.F8 = (Core.Enumerator.FunctionKeyType)dropId;
                        break;
                    default:
                        break;
                }
                _fingerDeviceService.Update(entity);
                result = true;
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AddCard
        public ActionResult AddCard(int deviceId, int dropId, int enrollNo)
        {
            int result = 0;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                if (entity.DeviceCards.Where(x => x.EnrollNo == enrollNo).Count() > 0)
                    result = 2;
                else
                {
                    var entity2 = new DeviceCard() {
                        EnrollNo = enrollNo,
                        FingerDeviceId = deviceId,
                        Type = (Core.Enumerator.FunctionKeyType)dropId
                    };
                    _deviceCardService.Create(entity2);
                result = 1;
                }
            }
            catch (Exception e)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AddWorkCode
        public ActionResult AddWorkCode(int deviceId, int dropId, int codeNo)
        {
            int result = 0;
            try
            {
                var entity = _fingerDeviceService.FindById(deviceId);
                if (entity.DeviceWorkCodes.Where(x => x.CodeNo == codeNo).Count() > 0)
                    result = 2;
                else
                {
                    var entity2 = new DeviceWorkCode()
                    {
                        CodeNo = codeNo,
                        FingerDeviceId = deviceId,
                        Type = (Core.Enumerator.FunctionKeyType)dropId
                    };
                    _deviceWorkCodeService.Create(entity2);
                    result = 1;
                }
            }
            catch (Exception e)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetWorkCodeList
        public ActionResult GetWorkCodeList(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_GetWorkCodeList", entity);
        }
        #endregion
        #region GetCardList
        public ActionResult GetCardList(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_GetCardList", entity);
        }
        #endregion
        #region DeleteCard
        public ActionResult DeleteCard(int cardId)
        {
            bool result = false;
            try
            {
                _deviceCardService.Delete(_deviceCardService.FindById(cardId));
                result = true;
            }
            catch (Exception)
            {

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DeleteWorkCode
        public ActionResult DeleteWorkCode(int workCodeId)
        {
            bool result = false;
            try
            {
                _deviceWorkCodeService.Delete(_deviceWorkCodeService.FindById(workCodeId));
                result = true;
            }
            catch (Exception)
            {

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region GetLogs
        public ActionResult GetLogs(int deviceId)
        {
            ViewBag.DeviceIdGetLogs = deviceId;
            return PartialView("_GetLogs");
        }
        #endregion
        #region GetLogsList
        [HttpGet]
        public JsonResult GetLogsList(DataTableRequest request, [ModelBinder(typeof(DataTableModelBinder))]DataTableRequestFilter filter, int deviceId)
        {
            #region Grid configuration
            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<FullLogModel>();
            modelItem.draw = request.draw;
            var data = _logService.FilterData(request.start, request.length, filter.Search, deviceId);
            modelItem.recordsTotal = _logService.Count(deviceId);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new FullLogModel();
                //model.Index = ++add;
                //model.DayOfWeek = DateTimeHelper.GetPersianDayName(x.LogDate.GetValueOrDefault());
                //model.EnrollNo = x.EnrollNo.GetValueOrDefault();
                //model.LogDate = DateTimeHelper.TopersianDate(x.LogDate.GetValueOrDefault());
                //model.LogTime = x.LogTime;
                //model.VerifyMode = x.VerifyMode;
                //model.InOutMode = x.InOutMode;
                //model.WorkCode = x.WorkCode;

                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DeviceCommands
        public ActionResult DeviceCommands(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_DeviceCommands", entity);
        }
        #endregion
        #region FunctionKeys
        public ActionResult FunctionKeys(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_FunctionKeys", entity);
        }
        #endregion
        #region WorkCodes
        public ActionResult WorkCodes(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_WorkCodes", entity);
        }
        #endregion
        #region Cards
        public ActionResult Cards(int deviceId)
        {
            var entity = _fingerDeviceService.FindById(deviceId);
            return PartialView("_Cards", entity);
        }
        #endregion

        #region GetEnrolls
        public ActionResult GetEnrolls(int deviceId)
        {
            ViewBag.DeviceIdGetLogs = deviceId;
            return PartialView("_GetEnrolls");
        }
        #endregion
        #region GetLogsList
        [HttpGet]
        public JsonResult GetEnrollsList(DataTableRequest request, [ModelBinder(typeof(DataTableModelBinder))]DataTableRequestFilter filter, int deviceId)
        {
            #region Grid configuration
            if (request.length == -1)
            {
                request.length = int.MaxValue;
            }
            var modelItem = new DataTableResponse<EnrollModels>();
            modelItem.draw = request.draw;
            var data = _enrollService.FilterData(request.start, request.length, filter.Search, deviceId);
            modelItem.recordsTotal = _enrollService.Count(deviceId);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;
            data.ForEach(x =>
            {
                var model = new EnrollModels();
                model.Index = ++add;
                model.EnrollNo = x.EnrollNo;
                model.Name = x.Name;
                model.Password = x.Password;
                model.Privileg = x.Privileg;
                model.Enabled = x.Enabled == true ? "<i class='fa fa-checked' style='color:green'></i>" : "<i class='fa fa-cross' style='color:red'></i>";
                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}