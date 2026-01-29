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
    public class HourlyLeaveReportModel
    {
        #region Ctor
        public HourlyLeaveReportModel(int currentYear)
        {
            #region بروزرسانی لیست ها
            UserGroupList = new List<SelectListItem>();
            LeaveTypeList = new List<SelectListItem>();
            YearList = new List<SelectListItem>();
            StartMonthList = new List<SelectListItem>();
            EndMonthList = new List<SelectListItem>();
            #endregion


            #region فراخوانی سرویس ها
            var _userGroupService = DependencyResolver.Current.GetService<IUserGroupService>();
            var _leaveTypeService = DependencyResolver.Current.GetService<ILeaveTypeService>();
            #endregion


            #region پرکردن مقادیر لیست ها
            var userGroupList = _userGroupService.List();
            var leaveTypeList = _leaveTypeService.List();


            UserGroupList.Add(new SelectListItem() { Value = "0", Text = "همه گروه ها" });


            if (userGroupList != null && userGroupList.Count() > 0)
            {
                userGroupList.ForEach(x => UserGroupList.Add(new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }
            if (leaveTypeList != null && leaveTypeList.Count() > 0)
            {
                leaveTypeList.ForEach(x => LeaveTypeList.Add(new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }



            StartMonthList.Add(new SelectListItem() { Value = "1", Text = "فروردین" });
            StartMonthList.Add(new SelectListItem() { Value = "2", Text = "اردیبهشت" });
            StartMonthList.Add(new SelectListItem() { Value = "3", Text = "خرداد" });
            StartMonthList.Add(new SelectListItem() { Value = "4", Text = "تیر" });
            StartMonthList.Add(new SelectListItem() { Value = "5", Text = "مرداد" });
            StartMonthList.Add(new SelectListItem() { Value = "6", Text = "شهریور" });
            StartMonthList.Add(new SelectListItem() { Value = "7", Text = "مهر" });
            StartMonthList.Add(new SelectListItem() { Value = "8", Text = "آبان" });
            StartMonthList.Add(new SelectListItem() { Value = "9", Text = "آذر" });
            StartMonthList.Add(new SelectListItem() { Value = "10", Text = "دی" });
            StartMonthList.Add(new SelectListItem() { Value = "11", Text = "بهمن" });
            StartMonthList.Add(new SelectListItem() { Value = "12", Text = "اسفند" });
            EndMonthList = StartMonthList;

            YearList = YearList.Where(x => int.Parse(x.Value) <= currentYear).ToList();
            YearId = currentYear;


            for (int i = 1397; i <= 1470; i++)
            {
                string result = i.ToString();
                YearList.Add(new SelectListItem()
                {
                    Value = result,
                    Text = result.ToString()
                });
            }

            #endregion
        }
        #endregion


        #region Propertices

        [Display(Name = "گروه پرسنل")]
        public int UserGroupId { get; set; }
        public List<SelectListItem> UserGroupList { get; set; }


        [Display(Name = "نوع مرخصی")]
        public int LeaveTypeId { get; set; }
        public List<SelectListItem> LeaveTypeList { get; set; }


        [Display(Name = "سال")]
        public int YearId { get; set; }
        public List<SelectListItem> YearList { get; set; }




        [Display(Name = "تاریخ شروع")]
        public int StartMonthId { get; set; }
        public List<SelectListItem> StartMonthList { get; set; }
        [Display(Name = "روز")]
        [MaxLength(2)]
        public int StartDay { get; set; }



        [Display(Name = "تاریخ پایان")]
        public int EndMonthId { get; set; }
        public List<SelectListItem> EndMonthList { get; set; }
        [Display(Name = "روز")]
        [MaxLength(2)]
        public int EndDay { get; set; }

        #endregion
    }

    public class HourlyLeaveGridModel
    {
        #region Ctor
        public HourlyLeaveGridModel()
        {

        }
        #endregion


        #region Propertices
        public int? Id { get; set; }
        public int? Index { get; set; }

        public string GroupName { get; set; }
        public string NameAndFamily { get; set; }
        public int Year { get; set; }
        public string MaximumLeave { get; set; }
        public string UsedLeave { get; set; }
        public string RemainLeave { get; set; }
        public string Type { get; set; }
        #endregion
    }
}
