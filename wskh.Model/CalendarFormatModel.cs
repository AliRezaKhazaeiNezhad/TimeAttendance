using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using wskh.Service;

namespace TimeAttendance.Model
{
    public class CalendarFormatModel
    {
        #region Ctor
        public CalendarFormatModel()
        {
            #region Days
            StartDateDays = new List<SelectListItem>();
            EndDateDays = new List<SelectListItem>();

            StartDateDays.Add(new SelectListItem() { Value = "1", Text = "1" });
            StartDateDays.Add(new SelectListItem() { Value = "2", Text = "2" });
            StartDateDays.Add(new SelectListItem() { Value = "3", Text = "3" });
            StartDateDays.Add(new SelectListItem() { Value = "4", Text = "4" });
            StartDateDays.Add(new SelectListItem() { Value = "5", Text = "5" });
            StartDateDays.Add(new SelectListItem() { Value = "6", Text = "6" });
            StartDateDays.Add(new SelectListItem() { Value = "7", Text = "7" });
            StartDateDays.Add(new SelectListItem() { Value = "8", Text = "8" });
            StartDateDays.Add(new SelectListItem() { Value = "9", Text = "9" });
            StartDateDays.Add(new SelectListItem() { Value = "10", Text = "10" });
            StartDateDays.Add(new SelectListItem() { Value = "11", Text = "11" });
            StartDateDays.Add(new SelectListItem() { Value = "12", Text = "12" });
            StartDateDays.Add(new SelectListItem() { Value = "13", Text = "13" });
            StartDateDays.Add(new SelectListItem() { Value = "14", Text = "14" });
            StartDateDays.Add(new SelectListItem() { Value = "15", Text = "15" });
            StartDateDays.Add(new SelectListItem() { Value = "16", Text = "16" });
            StartDateDays.Add(new SelectListItem() { Value = "17", Text = "17" });
            StartDateDays.Add(new SelectListItem() { Value = "18", Text = "18" });
            StartDateDays.Add(new SelectListItem() { Value = "19", Text = "19" });
            StartDateDays.Add(new SelectListItem() { Value = "20", Text = "20" });
            StartDateDays.Add(new SelectListItem() { Value = "21", Text = "21" });
            StartDateDays.Add(new SelectListItem() { Value = "22", Text = "22" });
            StartDateDays.Add(new SelectListItem() { Value = "23", Text = "23" });
            StartDateDays.Add(new SelectListItem() { Value = "24", Text = "24" });
            StartDateDays.Add(new SelectListItem() { Value = "25", Text = "25" });
            StartDateDays.Add(new SelectListItem() { Value = "26", Text = "26" });
            StartDateDays.Add(new SelectListItem() { Value = "27", Text = "27" });
            StartDateDays.Add(new SelectListItem() { Value = "28", Text = "28" });
            StartDateDays.Add(new SelectListItem() { Value = "29", Text = "29" });
            StartDateDays.Add(new SelectListItem() { Value = "30", Text = "30" });
            StartDateDays.Add(new SelectListItem() { Value = "31", Text = "31" });

            EndDateDays = StartDateDays;
            #endregion
            #region Months
            StartDateMonths = new List<SelectListItem>();
            EndDateMonths = new List<SelectListItem>();

            StartDateMonths.Add(new SelectListItem() { Value = "1", Text = "فروردین" });
            StartDateMonths.Add(new SelectListItem() { Value = "2", Text = "اردیبهشت" });
            StartDateMonths.Add(new SelectListItem() { Value = "3", Text = "خرداد" });
            StartDateMonths.Add(new SelectListItem() { Value = "4", Text = "تیر" });
            StartDateMonths.Add(new SelectListItem() { Value = "5", Text = "مرداد" });
            StartDateMonths.Add(new SelectListItem() { Value = "6", Text = "شهریور" });
            StartDateMonths.Add(new SelectListItem() { Value = "7", Text = "مهر" });
            StartDateMonths.Add(new SelectListItem() { Value = "8", Text = "آبان" });
            StartDateMonths.Add(new SelectListItem() { Value = "9", Text = "آذر" });
            StartDateMonths.Add(new SelectListItem() { Value = "10", Text = "دی" });
            StartDateMonths.Add(new SelectListItem() { Value = "11", Text = "بهمن" });
            StartDateMonths.Add(new SelectListItem() { Value = "12", Text = "اسفند" });

            EndDateMonths = StartDateMonths;
            #endregion
            #region CalendarDayModel
            CalendarDayModels = new List<CalendarDayModel>();
            #endregion
            #region WorkProgram
            WorkPrograms = new List<SelectListItem>();
            var _workProgramService = DependencyResolver.Current.GetService<IWorkProgramService>();
            var _wpList = _workProgramService.GetList;
            if (_wpList != null && _wpList.Count() > 0)
            {
                foreach (var wp in _wpList)
                {
                    WorkPrograms.Add(new SelectListItem()
                    {
                        Text = wp.Title,
                        Value = wp.Id.ToString()
                    });
                }
            }
            #endregion
        }
        #endregion
        #region Propertices
        public string StartDate { get; set; }
        public string EndDate { get; set; }



        [Display(Name = "تاریخ شروع"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int StartDateMonthId { get; set; }
        public List<SelectListItem> StartDateMonths { get; set; }
        [Display(Name = "تاریخ شروع"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int StartDateDayId { get; set; }
        public List<SelectListItem> StartDateDays { get; set; }



        [Display(Name = "تاریخ پایان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int EndDateMonthId { get; set; }
        public List<SelectListItem> EndDateMonths { get; set; }
        [Display(Name = "تاریخ پایان"), Required(ErrorMessage = "{0} را وارد نمایید")]
        public int EndDateDayId { get; set; }
        public List<SelectListItem> EndDateDays { get; set; }
        #endregion
        #region Relations
        public List<CalendarDayModel> CalendarDayModels { get; set; }
        [Display(Name = "برنامه کاری")]
        public int WorkProgramId { get; set; }
        public List<SelectListItem> WorkPrograms { get; set; }
        #endregion

    }
}
