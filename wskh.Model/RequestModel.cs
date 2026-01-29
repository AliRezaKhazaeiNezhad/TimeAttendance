using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Core.Enumerator;
using wskh.Model;
using wskh.Service;

namespace TimeAttendance.Model
{
    public class RequestModel : BaseModel
    {
        #region Ctor
        public RequestModel()
        {

        }
        #endregion

        #region Propertices
        public string Error { get; set; }
        public string Buttom { get; set; }

        [Display(Name = "زمان شروع")]
        [MaxLength(5)]
        public string StartHour { get; set; }
        [Display(Name = "زمان پایان")]
        [MaxLength(5)]
        public string EndHour { get; set; }


        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string CreateDateStriing { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public RequestState State { get; set; }
        public string StateString { get; set; }


        /// <summary>
        /// نوع درخواست
        /// </summary>
        public wskh.Core.Enumerator.LeaveType Type { get; set; }
        public string TypeString { get; set; }

        /// <summary>
        /// دلیل رد درخواست
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// تاریخ شروع درخواست
        /// </summary>
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }

        /// <summary>
        /// تاریخ پایان درخواست
        /// </summary>
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }



        /// <summary>
        /// کاربر درخواست کننده
        /// </summary>
        public string UserRequesterId { get; set; }
        public string UserRequester { get; set; }


        /// <summary>
        /// کاربر تایید/رد کننده
        /// </summary>
        public string UserRequesteManagerId { get; set; }
        public string UserRequesteManager { get; set; }


        #endregion
    }

    public class HourlyRequestModel : BaseModel
    {
        #region Ctor
        public HourlyRequestModel()
        {
            #region لیست کاربران
            UsersList = new List<SelectListItem>();
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            var userList = _userService.GetList;

            if (userList != null && userList.Count() > 0)
                foreach (var user in userList)
                {
                    UsersList.Add(new SelectListItem()
                    {
                        Text = $"{user.FirstName} {user.Lastname}",
                        Value = user.Id
                    });
                }
            #endregion


            #region لیست انواع مرخصی
            LeaveTypeList = new List<SelectListItem>();
            var _leaveTypeList = DependencyResolver.Current.GetService<ILeaveTypeService>();
            var leaveTypeList = _leaveTypeList.GetList;

            LeaveTypeList.Add(new SelectListItem() {
                Text = "نوع مرخصی را انتخاب نمایید",
                Value = ""
            });
            if (leaveTypeList != null && leaveTypeList.Count() > 0)
                foreach (var leaveTpe in leaveTypeList)
                {
                    LeaveTypeList.Add(new SelectListItem()
                    {
                        Text = $"{leaveTpe.Title}",
                        Value = leaveTpe.Id.ToString()
                    });
                }
            #endregion

        }
        #endregion

        #region Propertices

        [Display(Name = "تاریخ")]
        [MaxLength(10)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }


        public string Error { get; set; }

        [Display(Name = "زمان شروع")]
        [MaxLength(5)]
        public string StartHour { get; set; }
        [Display(Name = "زمان پایان")]
        [MaxLength(5)]
        public string EndHour { get; set; }


        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime Date { get; set; }
        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string DateStriing { get; set; }


        [Display(Name = "کاربر درخواست کننده")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string UserId { get; set; }
        public List<SelectListItem> UsersList { get; set; }



        [Display(Name = "نوع مرخصی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int LeaveTypeId { get; set; }
        public List<SelectListItem> LeaveTypeList { get; set; }
        #endregion
    }




    public class HourlyMissionRequestModel : BaseModel
    {
        #region Ctor
        public HourlyMissionRequestModel()
        {
            #region لیست کاربران
            UsersList = new List<SelectListItem>();
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            var userList = _userService.GetList;

            if (userList != null && userList.Count() > 0)
                foreach (var user in userList)
                {
                    UsersList.Add(new SelectListItem()
                    {
                        Text = $"{user.FirstName} {user.Lastname}",
                        Value = user.Id
                    });
                }
            #endregion

        }
        #endregion

        #region Propertices

        [Display(Name = "تاریخ")]
        [MaxLength(10)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }


        public string Error { get; set; }

        [Display(Name = "زمان شروع")]
        [MaxLength(5)]
        public string StartHour { get; set; }
        [Display(Name = "زمان پایان")]
        [MaxLength(5)]
        public string EndHour { get; set; }



        [Display(Name = "کاربر درخواست کننده")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string UserId { get; set; }
        public List<SelectListItem> UsersList { get; set; }

        #endregion
    }


    public class DailyRequestModel : BaseModel
    {
        #region Ctor
        public DailyRequestModel()
        {
            #region لیست کاربران
            UsersList = new List<SelectListItem>();
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            var userList = _userService.GetList;

            if (userList != null && userList.Count() > 0)
                foreach (var user in userList)
                {
                    UsersList.Add(new SelectListItem()
                    {
                        Text = $"{user.FirstName} {user.Lastname}",
                        Value = user.Id
                    });
                }
            #endregion
        }
        #endregion

        #region Propertices

        [Display(Name = "تاریخ شروع")]
        [MaxLength(10)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        [MaxLength(10)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string EndDate { get; set; }



        [Display(Name = "کاربر درخواست کننده")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string UserId { get; set; }
        public List<SelectListItem> UsersList { get; set; }


        public string Error { get; set; }

        #endregion
    }



    public class DailyRequestCRUDModel : BaseModel
    {
        #region Ctor
        public DailyRequestCRUDModel()
        {
            #region لیست کاربران
            UsersList = new List<SelectListItem>();
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            var userList = _userService.GetList;

            if (userList != null && userList.Count() > 0)
                foreach (var user in userList)
                {
                    UsersList.Add(new SelectListItem()
                    {
                        Text = $"{user.FirstName} {user.Lastname}",
                        Value = user.Id
                    });
                }
            #endregion


            #region لیست مرخصی ها
            LeaveTypeList = new List<SelectListItem>();
            var _leaveTypeService = DependencyResolver.Current.GetService<ILeaveTypeService>();
            var list = _leaveTypeService.GetList;
            LeaveTypeList.Add(new SelectListItem() {
                Text = "انتخاب نمایید",
                Value = null
            });
            if (list != null && list.Count() > 0)
                foreach (var leave in list)
                {
                    LeaveTypeList.Add(new SelectListItem()
                    {
                        Text = leave.Title,
                        Value = leave.Id.ToString()
                    });
                }
            #endregion
        }
        #endregion

        #region Propertices

        [Display(Name = "تاریخ شروع")]
        [MaxLength(10)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }
        [Display(Name = "تاریخ پایان")]
        [MaxLength(10)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string EndDate { get; set; }



        [Display(Name = "کاربر درخواست کننده")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string UserId { get; set; }
        public List<SelectListItem> UsersList { get; set; }



        [Display(Name = "نوع مرخصی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int LeaveTypeId { get; set; }
        public List<SelectListItem> LeaveTypeList { get; set; }


        public string Error { get; set; }

        #endregion
    }


    public class RequestReport
    {
        public RequestReport()
        {

        }

        public string TotalLastYearMin { get; set; }
        public string RemainTime { get; set; }
        public string RequestTime { get; set; }

        //public string 
    }
}
