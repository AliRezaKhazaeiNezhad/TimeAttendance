using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Core.Enumerator;
using wskh.Model;
using wskh.Service;

namespace TimeAttendance.Model
{
    public class ContractGridModel : BaseModel
    {
        #region Ctor
        public ContractGridModel()
        {
        }
        #endregion
        #region Propertices
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string Title { get; set; }

        public int? Id { get; set; }
        public string UserId { get; set; }
        public string CalendarTitle { get; set; }
        public string WorkRuleTitle { get; set; }
        #endregion
    }
    public class ContractModel : BaseModel
    {
        #region Ctor
        public ContractModel()
        {
            CalendarList = new List<SelectListItem>();
            WorkRuleList = new List<SelectListItem>();

            var _calendarService = DependencyResolver.Current.GetService<ICalendarService>();


            var calendarEntity = _calendarService.GetList;

            if (calendarEntity != null && calendarEntity.Count() > 0)
                foreach (var entity in calendarEntity)
                {
                    CalendarList.Add(new SelectListItem()
                    {
                        Value = entity.Id.ToString(),
                        Text = entity.Title
                    });
                }

            DelayActions = new List<SelectListItem>();
            DelayActions.Add(new SelectListItem() {
                Text = "هیچ عملیاتی انجام نشود",
                Value = "0"
            });
            DelayActions.Add(new SelectListItem()
            {
                Text = "استحقاقی از ابتدای ساعت",
                Value = "1"
            });
            DelayActions.Add(new SelectListItem()
            {
                Text = "استحقاقی از ابتدای تاخیر",
                Value = "2"
            });




            Holidays = new List<SelectListItem>();
            var _specialDayService = DependencyResolver.Current.GetService<ISpecialDayGroupingService>();


            var epecialDayEntity = _specialDayService.GetList;

            if (epecialDayEntity != null && epecialDayEntity.Count() > 0)
                foreach (var entity in epecialDayEntity)
                {
                    Holidays.Add(new SelectListItem()
                    {
                        Value = entity.Id.ToString(),
                        Text = entity.Title
                    });
                }

            FlowTimeMinDay = "0";
            YearlyLeaveInMonth = "0";
            DailyLeaveInMonth = "0";
            DelayActionPercent = "0";


            WorkPrograms = new List<SelectListItem>();
            var _workProgramService = DependencyResolver.Current.GetService<IWorkProgramService>();
            var wpEntity = _workProgramService.GetList;
            if (wpEntity != null && wpEntity.Count() > 0)
                foreach (var entity in wpEntity)
                {
                    WorkPrograms.Add(new SelectListItem()
                    {
                        Value = entity.Id.ToString(),
                        Text = entity.Title
                    });
                }

        }
        #endregion
        #region Propertices
        [MaxLength(10)]
        [Display(Name = "تاریخ شروع"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }
        [MaxLength(10)]
        [Display(Name = "تاریخ پایان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string EndDate { get; set; }
        [MaxLength(75)]
        [Display(Name = "عنوان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }
        #endregion


        #region Propertices From WorkRule
        [Display(Name = "آیا کلید تابعی اعمال شوند")]
        public bool UseFuncionKey { get; set; }
        [Display(Name = "آیا کارت تردد اعمال شوند")]
        public bool UseCard { get; set; }
        [Display(Name = "حداكثر تاخير در ماه (دقيقه)"), Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(7)]
        public string MaxDelyInMonth { get; set; }
        [Display(Name = "حداكثر تاخير در روز (دقيقه)"), Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(8)]
        public string MaxDelyInDay { get; set; }
        [Display(Name = "شناوری در روز (دقیقه)"), Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(6)]
        public string FlowTimeMinDay { get; set; }
        [Display(Name = "ميزان مرخصي استحقاقی در قرارداد (روز)"), Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(6)]
        public string YearlyLeaveInMonth { get; set; }
        [Display(Name = "انتقال مرخصی استحقاقی به قرارداد بعدی؟")]
        public bool DailyLeavePassToNextContract { get; set; }
        [Display(Name = "ميزان مرخصي استحقاقی در ماه (روز)"), Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(6)]
        public string DailyLeaveInMonth { get; set; }



        [Display(Name = "انتقال مرخصی استحقاقی به ماه بعد؟")]
        public bool HourlyLeavePassToNextMonth { get; set; }
        public NoExitLog NoExitLog { get; set; }


        [Display(Name = "مدیریت تاخیر ورود"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int DelayActionId { get; set; }
        public List<SelectListItem> DelayActions { get; set; }
        [Display(Name = "ضریب کسر"), Required(ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(3)]
        public string DelayActionPercent { get; set; }
        #endregion


        #region Relations

        [Display(Name = "شیفت کاری")]
        public int? CalendarId { get; set; }
        public List<SelectListItem> CalendarList { get; set; }

        [Display(Name = "قوانین کاری"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int WorkRuleId { get; set; }
        public List<SelectListItem> WorkRuleList { get; set; }


        [Display(Name = "تقویم تعطیلی و روز خاص"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int HolidayId { get; set; }
        public List<SelectListItem> Holidays { get; set; }


        [Display(Name = "برنامه کاری"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int? WorkProgramId { get; set; }
        public List<SelectListItem> WorkPrograms { get; set; }

        #endregion
    }
}
