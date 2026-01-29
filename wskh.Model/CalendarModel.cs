using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Model;
using wskh.Service;

namespace TimeAttendance.Model
{
    public class CalendarModel : BaseModel
    {
        #region Ctor
        public CalendarModel(List<string> persianYears)
        {
            WorkRuleModel = new WorkRuleModel();
            CalendarFormatModel = new CalendarFormatModel();

            #region PersianYears
            PersianYears = new List<SelectListItem>();
            persianYears.ForEach(x => PersianYears.Add(new SelectListItem() { Text = x, Value = x }));

            HolidayAndSpecialDays = new List<SelectListItem>();
            var _specialDayGroupingService = DependencyResolver.Current.GetService<ISpecialDayGroupingService>();
            var specialDayList = _specialDayGroupingService.List();
            if(specialDayList.Count() > 0 || specialDayList != null)
            {
                specialDayList.ForEach(x => HolidayAndSpecialDays.Add(new SelectListItem() {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }
            #endregion


            RequestRules = new List<SelectListItem>();
            var _requestRuleService = DependencyResolver.Current.GetService<IRequestRuleService>();
            var list = _requestRuleService.GetList;
            if (list != null && list.Count() > 0)
            {
                list.ForEach(x => RequestRules.Add(new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }
        }
        #endregion
        #region Propertices
        [MaxLength(50)]
        [Display(Name = "عنوان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Title { get; set; }


        public string StateString { get; set; }


        public bool IsEdit { get; set; }


        [Display(Name = "سال")]
        public int PersianYearId { get; set; }
        public List<SelectListItem> PersianYears { get; set; }


        [Display(Name = "قانون تردد")]
        public int? RequestRuleId { get; set; }
        public List<SelectListItem> RequestRules { get; set; }



        [Display(Name = "گروه تعطیلات و ایام خاص")]
        public int HolidayAndSpecialDayId { get; set; }
        public List<SelectListItem> HolidayAndSpecialDays { get; set; }

        public WorkRuleModel WorkRuleModel { get; set; }

        public CalendarFormatModel CalendarFormatModel { get; set; }
        #endregion

    }
}
