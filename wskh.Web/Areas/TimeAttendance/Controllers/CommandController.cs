using System.Linq;
using System.Web.Mvc;
using TimeAttendance.Core;
using TimeAttendance.Model;
using TimeAttendance.Web.Helper;
using wskh.Core.Enumerator;
using wskh.Service;
using wskh.WebEssentials.DataTablePart;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Areas.TimeAttendance.Controllers
{
    [Authorize]
    public class CommandController : Controller
    {
        #region Services
        private readonly ICommandService _commandService;
        private readonly IUserService _userService;
        #endregion
        #region Ctor
        public CommandController(ICommandService CommandService, IUserService userService)
        {
            _commandService = CommandService;
            _userService = userService;
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            ViewBag.MenuName = "command";
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
            var modelItem = new DataTableResponse<CommandModel>();
            modelItem.draw = request.draw;
            var data = _commandService.FilterData(request.start, request.length, CommandCategory.DeviceCommand);
            modelItem.recordsTotal = _commandService.Count(CommandCategory.DeviceCommand);
            modelItem.recordsFiltered = modelItem.recordsTotal;
            #endregion
            #region Prepare model
            var add = request.start;

            data.ForEach(x =>
            {
                var model = new CommandModel();
                model.Index = ++add;
                switch (x.State)
                {
                    case CommandState.Pending:
                        model.State = "در صف پردازش";
                        break;
                    case CommandState.Analyzing:
                        model.State = "درحال پردازش";
                        break;
                    case CommandState.Analyzed:
                        model.State = "پردازش شده";
                        break;
                    case CommandState.Fraction:
                        model.State = "پردازش با خطا  روبرو شده";
                        break;
                    default:
                        break;
                }
                var findUser = UserHelper.FullInformation(x.UserId);
                model.Title = x.Title;
                model.Count = x.Count;
                model.CreateDateTime = x.CreateDateTime != null ? DateTimeHelper.TopersianFull(x.CreateDateTime.GetValueOrDefault()) : "-";
                model.StartingDateTime = x.StartingDateTime != null ? DateTimeHelper.TopersianFull(x.StartingDateTime.GetValueOrDefault()) : "-";
                model.FinishDateTime = x.FinishDateTime != null ? DateTimeHelper.TopersianFull(x.FinishDateTime.GetValueOrDefault()) : "-";
                model.User = findUser;

                if (model.Count > 0)
                {
                    model.Title = model.Title + $" ({model.Count} مورد دریافت شد)";
                }



                modelItem.data.Add(model);
            });
            #endregion

            return Json(modelItem, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}