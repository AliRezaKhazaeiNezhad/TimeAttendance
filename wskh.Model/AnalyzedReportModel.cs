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
    public class AnalyzedReportModel
    {
        public AnalyzedReportModel()
        {
            UserGroupList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
            var _userGroupService = DependencyResolver.Current.GetService<IUserGroupService>();
            var userGroupList = _userGroupService.List();

            UserGroupList.Add(new SelectListItem()
            {
                Text = "انتخاب نمایید",
                Value = null
            });

            if (userGroupList != null && userGroupList.Count() > 0)
            {
                userGroupList.ForEach(x => UserGroupList.Add(new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()
                }));
            }
        }

        [Display(Name = "گروه پرسنل")]
        public int UserGroupId { get; set; }
        public List<SelectListItem> UserGroupList { get; set; }


        [Display(Name = "کارمند")]
        public int UserId { get; set; }
        public List<SelectListItem> UserList { get; set; }


        [Display(Name = "تارخ شروع")]
        public string StartDatePersian { get; set; }
        [Display(Name = "تاریخ پایان")]
        public string EndDatePersian { get; set; }

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
    }
}
