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
    public class TradeSearchModel
    {
        #region Ctor
        public TradeSearchModel()
        {
            #region بروزرسانی لیست ها
            UserGroupList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
            TradeTypeList = new List<SelectListItem>();
            #endregion


            #region فراخوانی سرویس ها
            var _userGroupService = DependencyResolver.Current.GetService<IUserGroupService>();
            var _userService = DependencyResolver.Current.GetService<IUserService>();
            #endregion


            #region پرکردن مقادیر لیست ها
            var userGroupList = _userGroupService.List();
            UserGroupList.Add(new SelectListItem() { Value = "0", Text = "تمامی گروه ها" });
            if (userGroupList != null && userGroupList.Count() > 0)
            {
                userGroupList.ForEach(x => UserGroupList.Add(new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }


            UserList.Add(new SelectListItem() { Value = "", Text = "تمامی  کاربران" });

            TradeTypeList.Add(new SelectListItem() { Value = "0", Text = "تمامی موارد" });
            TradeTypeList.Add(new SelectListItem() { Value = "1", Text = "تردد کامل" });
            TradeTypeList.Add(new SelectListItem() { Value = "2", Text = "تردد ناقص" });


            #endregion
        }
        #endregion


        #region Propertices

        [Display(Name = "گروه پرسنل")]
        public int UserGroupId { get; set; }
        public List<SelectListItem> UserGroupList { get; set; }


        [Display(Name = "کاربر")]
        public string UserId { get; set; }
        public List<SelectListItem> UserList { get; set; }


        [Display(Name = "نوع تردد")]
        public int TradeTypeId { get; set; }
        public List<SelectListItem> TradeTypeList { get; set; }



        [Display(Name = "تاریخ شروع")]
        public string StartDate { get; set; }


        [Display(Name = "تاریخ پایان")]
        public string EndDate { get; set; }
        #endregion
    }
}
